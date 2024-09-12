using arif.Construction.Domain.Config;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.RabbitMq;

public class MassTransitFilter : IFilter<ConsumeContext>
{
    private readonly MassTransitSettings _massTransitConfig;
    private readonly Dictionary<string, MessageType> _messageTypes;
    private readonly Dictionary<string, int> count = new Dictionary<string, int>();

    public MassTransitFilter(MassTransitSettings massTransitOptions)
    {
        _massTransitConfig = massTransitOptions;
        _messageTypes = massTransitOptions?.MessageTypes?.ToDictionary(x => x.Type);
    }

    public void Probe(ProbeContext context) => context.CreateFilterScope("massTransitCustomMiddleware");

    public Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
    {
        throw new NotImplementedException();
    }


    private async Task PassMessageForward(ConsumeContext context, IPipe<ConsumeContext> next) => await next.Send(context).ConfigureAwait(false);


}
