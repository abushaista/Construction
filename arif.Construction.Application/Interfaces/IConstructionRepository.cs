using arif.Construction.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Application.Interfaces
{
    public interface IConstructionRepository
    {
        Task InsertData(ConstructionCreatedEvent createdEvent);
        Task UpdateData(ConstructionUpdatedEvent updatedEvent);
        Task<int> GetCount();
    }
}
