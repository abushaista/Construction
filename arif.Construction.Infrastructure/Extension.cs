using arif.Construction.Application.Consumers;
using arif.Construction.Application.Interfaces;
using arif.Construction.Application.Services;
using arif.Construction.Domain.Config;
using arif.Construction.Domain.Events;
using arif.Construction.Domain.Requests;
using arif.Construction.Infrastructure.Persistence;
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

namespace arif.Construction.Infrastructure
{
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

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ConstructionConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost");

                    // Configure endpoints for consumers
                    cfg.ReceiveEndpoint("construction-queue", e =>
                    {
                        e.ConfigureConsumer<ConstructionConsumer>(context);
                    });
                });
                x.AddRequestClient<ConstructionUpdatedEvent>();
                x.AddRequestClient<ConstructionCreatedEvent>();
                x.AddRequestClient<UpdateProjectRequest>();
                x.AddRequestClient<CreateProjectRequest>();
            });
            return services;
        }

        
    }
}
