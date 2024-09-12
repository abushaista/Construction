using arif.Construction.Domain.Response;
using MassTransit;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.RabbitMq;

public class CorrelationMessageConsumeFilter<T> :
        IFilter<ConsumeContext<T>>
        where T : class
{
    private readonly ILogger<CorrelationMessageConsumeFilter<T>> _logger;
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        var type = context.Message?.GetType() ?? typeof(T);
        CorrelationLogHelper.ValidateConversationId(_logger, context.ConversationId, context.SourceAddress, context.DestinationAddress, context.Message);
        using (ContextCorrelator.BeginCorrelationScope("ConversationId", context.ConversationId ?? Guid.Empty))
        {
            LogRequest(context, type);
            try
            {
                await next.Send(context);
            }
            catch (MessageNotConsumedException ex)
            {
                _logger.LogError($"MessageNotConsumedException with message type of {type}", ex);
                await context.RespondAsync(ServiceResponse.ErrorResponse(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError("Unhandled exception in consumer. This can lead to message not consumed.", ex);
                //If ResponseAddress null means we use mediator send and mediator publish and don't expect response being return
                //rethrow exeption in this case for upper lever try catch handler
                if (context.ResponseAddress == null && context.DestinationAddress.AbsolutePath.Contains("mediator"))
                {
                    throw;
                }
                await context.RespondAsync(ServiceResponse.ErrorResponse(ex.Message));
            }
        };
    }

    private void LogRequest(ConsumeContext<T> context, Type type)
    {
        try
        {
            var jsonSerializationSetting = new JsonSerializerSettings
            {
                ContractResolver = new IgnorePropertiesResolver()
            };
            _logger.LogDebug($"Consume message of type {type.Name} with data: {JsonConvert.SerializeObject(context.Message, jsonSerializationSetting)}");
        }
        catch
        {
            _logger.LogError($"Cannot serialize message of type {type.Name}");
        }
    }
}

public class IgnorePropertiesResolver : DefaultContractResolver
{
    private readonly HashSet<string> _ignoreProps;
    public IgnorePropertiesResolver(params string[] propNamesToIgnore)
    {
        _ignoreProps = new HashSet<string>(propNamesToIgnore);
    }
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        //Ignore byte array and properties defined in property name
        if (_ignoreProps.Contains(property.PropertyName) || property.PropertyType == typeof(byte[]))
        {
            property.ShouldSerialize = _ => false;
        }
        return property;
    }
}
