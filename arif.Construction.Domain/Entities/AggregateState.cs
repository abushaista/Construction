using arif.Construction.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Entities;

public abstract class AggregateState
{
    public Guid Id { get; set; }
    public int Version { get; private set; } = -1;

    public void Mutate(IEvent @event)
    {
        Apply(@event);
        Version++;
    }

    protected abstract void Apply(IEvent @event);
}
