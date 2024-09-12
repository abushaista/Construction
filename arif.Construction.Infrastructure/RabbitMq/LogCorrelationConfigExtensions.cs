using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.RabbitMq;

public static class LogCorrelationConfigExtensions
{
    public static void AddLogCorrelation(this IReceiveEndpointConfigurator configurator, IRegistrationContext context)
    {
        configurator.UseSendFilter(typeof(CorrelationMessageSendFilter<>), context);
        configurator.UsePublishFilter(typeof(CorrelationMessagePublishFilter<>), context);
        configurator.UseConsumeFilter(typeof(CorrelationMessageConsumeFilter<>), context);
    }

    public static void AddLogCorrelation<T>(this IBusFactoryConfigurator<T> configurator, IRegistrationContext context) where T : class, IReceiveEndpointConfigurator
    {
        configurator.UseSendFilter(typeof(CorrelationMessageSendFilter<>), context);
        configurator.UsePublishFilter(typeof(CorrelationMessagePublishFilter<>), context);
        configurator.UseConsumeFilter(typeof(CorrelationMessageConsumeFilter<>), context);
    }
}
