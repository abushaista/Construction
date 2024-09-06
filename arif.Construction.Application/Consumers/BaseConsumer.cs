using arif.Construction.Application.Interfaces;
using arif.Construction.Domain.Construction;
using arif.Construction.Domain.Events;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Application.Consumers
{
    public class BaseConsumer<T> where T : class
    {
        protected readonly IConstructionEventStore _store;
        protected readonly IMediator _mediator;
        protected readonly ILogger<T> _logger;

        public BaseConsumer(IConstructionEventStore store, IMediator mediator, ILogger<T> logger)
        {
            _store = store;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<ConstructionAggregate> GetAggregate(Guid id)
        {
            var events = await _store.GetAsync(id);
            var aggregate = new ConstructionAggregate() { Id = id };
            aggregate.Rehydrate(events);
            return aggregate;
        }

        public async Task Save(ConstructionAggregate aggregate)
        {
            try
            {
                _logger.LogDebug(new EventId(1), $"Event Saving! \n\n{aggregate}");
                var events = aggregate.GetUncommittedEvents();
                await _store.Save(aggregate, events);
                foreach (var item in events)
                {
                    await _mediator.Send(item);
                }
                aggregate.ClearUncommittedEvents();
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(3), $"Event exception \n\n{ex.Message}");
                throw;
            }
            

        }
    }
}
