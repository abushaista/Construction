using arif.Construction.Domain.Entities;
using arif.Construction.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Application.Interfaces;

public interface IEventStore<TAggregate, TState>
     where TAggregate : AggregateRoot<TState>
        where TState : AggregateState
{
    Task Save(TAggregate aggregate, IEnumerable<IEvent> events);
    Task<IEnumerable<IEvent>> GetAsync(Guid aggregateIdentifier);
}
