using arif.Construction.Domain.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Application.Interfaces
{
    public interface IConstructionEventStore : IEventStore<ConstructionAggregate, ConstructionAggregateState>
    {
    }
}
