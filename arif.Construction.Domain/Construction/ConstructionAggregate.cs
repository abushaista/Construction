using arif.Construction.Domain.Entities;
using arif.Construction.Domain.Events;
using arif.Construction.Domain.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Construction;

public class ConstructionAggregate : AggregateRoot<ConstructionAggregateState>
{
    public override ConstructionAggregateState CreateState() => new();

    public ConstructionCreatedEvent StartProject(CreateProjectRequest request, string uniqueId)
    {
        var @event = new ConstructionCreatedEvent();
        @event.Id = Guid.NewGuid();
        @event.UniqueProjectId = uniqueId;
        @event.ProjectCategory = request.ProjectCategory;
        @event.ProjectName = request.ProjectName;
        @event.Description = request.Description;
        @event.ProjectStage = request.ProjectStage;
        @event.ConstructionStartDate = request.ConstructionStartDate;
        @event.UserId = request.UserId;
        Apply(@event);
        return @event;
    }

    public ConstructionUpdatedEvent UpdateProject(UpdateProjectRequest request)
    {
        var @event = new ConstructionUpdatedEvent();
        @event.Id = Id;
        @event.ProjectCategory = request.ProjectCategory;
        @event.ProjectName = request.ProjectName;
        @event.Description = request.Description;
        @event.ProjectStage = request.ProjectStage;
        @event.ConstructionStartDate = request.ConstructionStartDate;
        Apply(@event);
        return @event;
    }

    public override void Rehydrate(IEnumerable<IEvent> events)
    {
        foreach (var @event in events)
        {
            State.Mutate(@event);  // Apply each event to mutate the aggregate state
            Version++;
        }
    }

}
