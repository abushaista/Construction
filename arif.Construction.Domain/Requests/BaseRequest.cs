using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Requests
{
    public abstract class BaseRequest
    {
        public Guid UserId { get; set; }
    }
}
