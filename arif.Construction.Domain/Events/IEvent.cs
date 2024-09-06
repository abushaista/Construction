using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Events;

public interface IEvent
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Version { get; set; }
    public Guid UserId { get; set; }
}
