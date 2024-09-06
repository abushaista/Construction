using arif.Construction.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Requests
{
    public class UpdateProjectRequest: BaseRequest
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; }
        public Stage ProjectStage { get; set; }
        public Category ProjectCategory { get; set; }
        public DateTime ConstructionStartDate { get; set; }
        public string Description { get; set; }
    }
}
