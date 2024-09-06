using arif.Construction.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Entities;

public abstract class AggregateRoot<TState> where TState : AggregateState
{
    private readonly List<IEvent> _domainEvents = new List<IEvent>();

    public Guid Id { get; set; }
    public TState State { get; set; }
    public abstract TState CreateState();
    public int Version { get; set; }

    protected void Apply(IEvent @event)
    {
        State ??= CreateState();
        @event.Version = State.Version + 1;
        _domainEvents.Add(@event);
        State.Mutate(@event);
    }
    public abstract void Rehydrate(IEnumerable<IEvent> events);
    public IEnumerable<IEvent> GetUncommittedEvents() => _domainEvents.AsEnumerable();

    public void ClearUncommittedEvents() => _domainEvents.Clear();
}
