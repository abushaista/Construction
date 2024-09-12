using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.RabbitMq;

public class CorrelationMessageSendFilter<T> :
     IFilter<SendContext<T>>
        where T : class
{
    private readonly IHttpContextAccessor _context;
    private readonly ILogger<CorrelationMessageSendFilter<T>> _logger;

    public CorrelationMessageSendFilter(IHttpContextAccessor context, ILogger<CorrelationMessageSendFilter<T>> logger)
    {
        _context = context;
        _logger = logger;
    }
    public void Probe(ProbeContext context)
    {
    }

    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        var headers = _context?.HttpContext?.Request?.Headers;
        var conversationId = CorrelationLogHelper.GetConversationId(context.ConversationId, headers);
        context.ConversationId = conversationId;
        CorrelationLogHelper.ValidateConversationId(_logger, context.ConversationId, context.SourceAddress, context.DestinationAddress, context.Message);
        using (ContextCorrelator.BeginCorrelationScope("ConversationId", context.ConversationId ?? Guid.Empty))
        {
            await next.Send(context);
        }
    }
}
