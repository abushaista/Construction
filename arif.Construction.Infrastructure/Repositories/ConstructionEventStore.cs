using arif.Construction.Application.Interfaces;
using arif.Construction.Domain.Construction;
using arif.Construction.Domain.Events;
using arif.Construction.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace arif.Construction.Infrastructure.Repositories
{
    public class ConstructionEventStore : IConstructionEventStore
    {
        private readonly ApplicationDbContext _dbContext;

        public ConstructionEventStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<IEvent>> GetAsync(Guid aggregateIdentifier)
        {
           var records = await _dbContext.Events.Where(e => e.AggregateId == aggregateIdentifier)
            .OrderBy(e => e.Version)
            .ToListAsync();
            var events = records.Select(e => JsonSerializer.Deserialize<Domain.Events.Event>(e.Data)).ToList();

            return events;
        }

        public async Task Save(ConstructionAggregate aggregate, IEnumerable<IEvent> events)
        {
            var currentVersion = await GetVersionAsync(aggregate.Id);
            if (currentVersion != aggregate.Version)
            {
                throw new ConcurrencyException("Version mismatch - Aggregate has been modified.",aggregate.GetType(), aggregate.Id);
            }

            foreach (var @event in events)
            {
                var eventData = new EventData
                {
                    AggregateId = aggregate.Id,
                    Version = ++currentVersion,
                    Type = @event.GetType().Name,
                    CreatedAt = DateTime.UtcNow,
                    Data = SerializeEvent(@event),
                    UserId = @event.UserId
                };
                _dbContext.Events.Add(eventData);
            }
            await _dbContext.SaveChangesAsync();
        }

        private string SerializeEvent(IEvent @event)
        {
            return JsonSerializer.Serialize(@event); // JSON serialization of the event
        }


        private async Task<int> GetVersionAsync(Guid aggregateId)
        {
            var lastEvent = await _dbContext.Events
                .Where(e => e.AggregateId == aggregateId)
                .OrderByDescending(e => e.Version)
                .FirstOrDefaultAsync();

            return lastEvent?.Version ?? -1;
        }
    }
}
