using arif.Construction.Application.Interfaces;
using arif.Construction.Domain.Events;
using arif.Construction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.Repositories
{
    public class ConstructionRepository : IConstructionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ConstructionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertData(ConstructionCreatedEvent createdEvent)
        {
            var construction = new ConstructionData
            {
                Id = createdEvent.Id,
                UniqueId = createdEvent.UniqueProjectId,
                ProjectName = createdEvent.ProjectName,
                ProjectStage = createdEvent.ProjectStage,
                ProjectCategory = createdEvent.ProjectCategory, 
                ConstructionStartDate = createdEvent.ConstructionStartDate,
                Description = createdEvent.Description,
                createdAt = createdEvent.CreatedAt,
                CreatedBy = createdEvent.UserId
            };
            _dbContext.Constructions.Add(construction);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateData(ConstructionUpdatedEvent updatedEvent)
        {
            var construction = await _dbContext.Constructions.Where(x=> x.Id ==  updatedEvent.Id).FirstOrDefaultAsync();
            if (construction != null) { 
                construction.ProjectStage = updatedEvent.ProjectStage;
                construction.ProjectName = updatedEvent.ProjectName;
                construction.ProjectCategory = updatedEvent.ProjectCategory;
                construction.Description = updatedEvent.Description;
                construction.ConstructionStartDate = updatedEvent.ConstructionStartDate;
                construction.updatedAt = updatedEvent.CreatedAt;
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetCount()
        {
            var data = await _dbContext.Constructions.Select(x=> x.Id).CountAsync();
            return data;
        }
    }
}
