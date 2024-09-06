using arif.Construction.Application.Interfaces;
using arif.Construction.Application.Services;
using arif.Construction.Domain.Construction;
using arif.Construction.Domain.Events;
using arif.Construction.Domain.Requests;
using arif.Construction.Domain.Response;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Application.Consumers
{
    public class ConstructionConsumer : BaseConsumer<ConstructionConsumer>,
        IConsumer<ConstructionCreatedEvent>,
        IConsumer<ConstructionUpdatedEvent>,
        IConsumer<CreateProjectRequest>
    {
        private readonly IConstructionRepository _constructionRepository;
        private readonly IUniqueNumberService _numberService;
        public ConstructionConsumer(IConstructionRepository constructionRepository,
            IUniqueNumberService numberService,
            IConstructionEventStore store, 
            IMediator mediator, 
            ILogger<ConstructionConsumer> logger) : base(store, mediator, logger)
        {
            _constructionRepository = constructionRepository;
            _numberService = numberService;
        }

        public async Task Consume(ConsumeContext<ConstructionCreatedEvent> context)
        {
            try
            {
                await _constructionRepository.InsertData(context.Message);
                await context.RespondAsync(ServiceResponse.SuccessResponse());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await context.RespondAsync(ServiceResponse.ErrorResponse(e.Message));
            }
            
        }

        public async Task Consume(ConsumeContext<ConstructionUpdatedEvent> context)
        {
            try
            {
                await _constructionRepository.UpdateData(context.Message);
                await context.RespondAsync(ServiceResponse.SuccessResponse());
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await context.RespondAsync(ServiceResponse.ErrorResponse(e.Message));
            }
        }

        public async Task Consume(ConsumeContext<CreateProjectRequest> context)
        {
            try
            {
                var aggregate = new ConstructionAggregate() { Id = Guid.NewGuid() };
                var uniqueId = await _numberService.GetUniqueNumber();
                var @event = aggregate.StartProject(context.Message, uniqueId);
                await Save(aggregate);
                context.Respond(ServiceResponse.SuccessResponse());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                context.Respond(ServiceResponse.ErrorResponse(ex.Message));
            }
        }



    }
}
