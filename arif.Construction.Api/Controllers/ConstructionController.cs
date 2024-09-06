using arif.Construction.Api.Base;
using arif.Construction.Domain.Requests;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace arif.Construction.Api.Controllers
{
    public class ConstructionController : BaseController
    {
        public ConstructionController(IMediator mediator, IClientFactory clientFactory, ILogger<ConstructionController> logger) 
            : base(mediator, clientFactory, logger) { }
        [HttpPost("create")]
        public async Task<IActionResult> Post(CreateProjectRequest command)
        {
            return await GetCommandResult(command);
        }

        [HttpPost("post")]
        public async Task<IActionResult> Update(UpdateProjectRequest command)
        {
            return await GetCommandResult(command);
        }

    }
}
