using arif.Construction.Application.Consumers;
using arif.Construction.Application.Interfaces;
using arif.Construction.Application.Services;
using arif.Construction.Domain.Config;
using arif.Construction.Domain.Events;
using arif.Construction.Domain.Requests;
using arif.Construction.Infrastructure.Persistence;
using arif.Construction.Infrastructure.RabbitMq;
using arif.Construction.Infrastructure.Repositories;
using arif.Construction.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure;

public static class Extension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var dbOptions = new DbOptions();
        config.Bind(DbOptions.SectionName, dbOptions);
        services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(dbOptions.ConnectionString));
        services.AddScoped<IConstructionEventStore, ConstructionEventStore>();
        services.AddScoped<IConstructionRepository, ConstructionRepository>();
        services.AddScoped<IUniqueNumberService, UniqueNumberService>();
        var rabbitMq = new RabbitMqSettings();
        config.Bind("RabbitMq", rabbitMq);
        var massTransit = new MassTransitSettings();
        config.Bind("MassTransit", massTransit);

        
        services.AddMediator(rabbitMq, x =>
        {
            x.ConfigMediator(rabbitMq);
        }, massTransit);
        return services;
    }

    public static IServiceCollection AddMediator(this IServiceCollection services, RabbitMqSettings rabbitMqOptions, Action<IMediatorRegistrationConfigurator> configure = null, MassTransitSettings? massTransitConfig = null)
    {
        var serviceProvider = services.BuildServiceProvider();
        DependencyInjectionRegistrationExtensions.AddMediator(services, x =>
        {
            x.ConfigureMediator((contex, config) =>
            {
                config.AddMiddleware(massTransitConfig);
                config.AddLogCorrelation(contex);
            });
            configure?.Invoke(x);
        });
        return services;
    }
    public static void AddMiddleware(this IPipeConfigurator<ConsumeContext> configurator,
            MassTransitSettings massTransitOptions)
    {
        if (configurator == null)
            throw new ArgumentNullException(nameof(configurator));

        configurator.AddPipeSpecification(new MassTransitMiddleware(massTransitOptions));
    }

    public static void ConfigMediator(this IMediatorRegistrationConfigurator x, RabbitMqSettings rabbitMq)
    {
        x.AddConsumer<ConstructionConsumer>(rabbitMq);
        x.AddRequestClient<ConstructionUpdatedEvent>(rabbitMq);
        x.AddRequestClient<ConstructionCreatedEvent>(rabbitMq);
        x.AddRequestClient<UpdateProjectRequest>(rabbitMq);
        x.AddRequestClient<CreateProjectRequest>(rabbitMq);
    }



    public static void AddRequestClient<T>(this IRegistrationConfigurator registrationConfigurator, RabbitMqSettings rabbitMqSettings)
        where T : class
    {
        var requestTimeout = RequestTimeout.Default;

        if (rabbitMqSettings?.TimeoutSettings != null)
        {
            var settings = rabbitMqSettings.TimeoutSettings;
            if (settings.Days > 0 ||
                settings.Hours > 0 ||
                settings.Minutes > 0 ||
                settings.Seconds > 0 ||
                settings.Milliseconds > 0)
            {
                requestTimeout = RequestTimeout.After(settings.Days, settings.Hours, settings.Minutes, settings.Seconds, settings.Milliseconds);
            }
        }

        registrationConfigurator.AddRequestClient<T>(requestTimeout);
    }

    public static void AddConsumer<T>(this IRegistrationConfigurator registrationConfigurator, RabbitMqSettings rabbitMqSettings)
            where T : class, IConsumer
    {
        //Add Mediator consummer. No config needed at the moment
        registrationConfigurator.AddConsumer<T>();
    }

}


