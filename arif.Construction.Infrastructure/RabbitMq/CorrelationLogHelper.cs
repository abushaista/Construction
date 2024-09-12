using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.RabbitMq;

internal static class CorrelationLogHelper
{
    public static void ValidateConversationId<T>(ILogger logger, Guid? masstransitConversationId, Uri? sourceAddress, Uri? destinationAddress, T message)
    {
        var type = message?.GetType() ?? typeof(T);
        var conversationIdFromlogContext = ContextCorrelator.GetValue("ConversationId");
        if (GuidNullOrEmpty(masstransitConversationId) && conversationIdFromlogContext == Guid.Empty)
        {
            logger.LogDebug($"ConversationId is null or empty. This will lead to incorrect CorrelationId. " +
                $"Message of type: {type} | SourceAddress: {sourceAddress} | DestinationAddress: {destinationAddress} " +
                $"| Message content: {JsonConvert.SerializeObject(message)}");
        }

        if (conversationIdFromlogContext != Guid.Empty && !GuidNullOrEmpty(masstransitConversationId) && conversationIdFromlogContext != masstransitConversationId)
        {
            logger.LogError($"ConversationId from masstransit not match with ConversationId from LogContext. " +
                $"ConversationId from Masstransit {masstransitConversationId?.ToString("N")} | ConversationId from LogContext {conversationIdFromlogContext.ToString("N")} " +
                $"| Message of type: {type} | SourceAddress: {sourceAddress} | DestinationAddress: {destinationAddress}");
        }
    }

    //Returns ConversationId base on priority: HTTP headers -> Masstransit -> ConversationId from LogContext
    public static Guid GetConversationId(Guid? masstransitConversationId, IHeaderDictionary headers)
    {
        if (headers != null && headers.ContainsKey("X-Conversation-Id") && Guid.TryParse(headers["X-Conversation-Id"], out Guid conversationId))
            return conversationId;

        if (!GuidNullOrEmpty(masstransitConversationId)) return masstransitConversationId.Value;

        var conversationIdFromlogContext = ContextCorrelator.GetValue("ConversationId");
        return conversationIdFromlogContext;
    }

    private static bool GuidNullOrEmpty(Guid? guid)
    {
        return guid == null || guid == Guid.Empty;
    }
}
