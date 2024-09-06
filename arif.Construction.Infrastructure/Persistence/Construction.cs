using arif.Construction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace arif.Construction.Infrastructure.Persistence
{
    public class ConstructionData
    {
        public Guid Id { get; set; }
        public string UniqueId { get; set; }
        public string ProjectName { get; set; }
        public Stage ProjectStage { get; set; }
        public Category ProjectCategory { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public string Description { get; set; }
        public DateTime createdAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime updatedAt { get; set; } 
    }
}
