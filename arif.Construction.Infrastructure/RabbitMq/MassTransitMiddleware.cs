using MassTransit.Configuration;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using arif.Construction.Domain.Config;

namespace arif.Construction.Infrastructure.RabbitMq
{
    public class MassTransitMiddleware : IPipeSpecification<ConsumeContext>
    {
        public readonly MassTransitSettings _massTransitOptions;
        public MassTransitMiddleware(MassTransitSettings massTransitOptions)
        {
            _massTransitOptions = massTransitOptions;
        }
        public void Apply(IPipeBuilder<ConsumeContext> builder)
        {
            builder.AddFilter(new MassTransitFilter(_massTransitOptions));
        }

        public IEnumerable<ValidationResult> Validate()
        {
            yield break;
        }
    }
}
