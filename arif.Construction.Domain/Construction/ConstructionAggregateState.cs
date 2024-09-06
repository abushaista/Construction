using arif.Construction.Domain.Entities;
using arif.Construction.Domain.Enums;
using arif.Construction.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Construction
{
    public class ConstructionAggregateState : AggregateState
    {
        public string UniqueProjectId { get;set; }
        public string ProjectName { get; set; }
        public Stage ProjectStage { get; set; }
        public Category ProjectCategory { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        protected override void Apply(IEvent @event)
        {
            switch (@event) {
                case ConstructionCreatedEvent e:
                    Id = Guid.NewGuid();
                    UniqueProjectId = e.UniqueProjectId;
                    ProjectName = e.ProjectName;
                    ProjectStage = e.ProjectStage;
                    ProjectCategory = e.ProjectCategory;
                    Description = e.Description;
                    ConstructionStartDate = e.ConstructionStartDate;
                    UserId = e.UserId;
                    break;
                case ConstructionUpdatedEvent e:
                    ProjectName = e.ProjectName;
                    ProjectStage = e.ProjectStage;
                    ProjectCategory = e.ProjectCategory;
                    Description = e.Description;
                    ConstructionStartDate = e.ConstructionStartDate;
                    break;
            }
        }
    }
}
