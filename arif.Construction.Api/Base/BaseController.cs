using MassTransit.Mediator;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using MassTransit.Internals.Caching;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using arif.Construction.Domain.Response;
using FluentValidation;
using System.Text.Json;

namespace arif.Construction.Api.Base
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : ControllerBase
    {
        private readonly ICache<string, Assembly> _newCache;
        protected readonly ILogger<BaseController> _logger;
        protected readonly IMediator _mediator;
        protected IClientFactory _clientFactory;

        public BaseController(IMediator mediator, IClientFactory clientFactory, ILogger<BaseController> logger)
        {
            _mediator = mediator;
            _clientFactory = clientFactory;
            _logger = logger;
            _newCache = new MassTransitCache<string, Assembly, CacheValue<Assembly>>(new UsageCachePolicy<Assembly>());
        }

        private ServiceResponse Validate<T>(T command) where T : class
        {
            try
            {
                var assembly = _newCache.GetOrAdd("Assembly", _ => Task.FromResult(typeof(T).Assembly));
                var validatorName = typeof(T).Name + "Validator";
                var types = Array.Find(assembly.Result.GetTypes(), e => e.IsClass && e.Name == validatorName);
                var validator = Activator.CreateInstance(types) as AbstractValidator<T>;
                var result = validator.Validate(command);
                if (result != null && !result.IsValid)
                {
                    _logger.LogError($"Error Validated! \n\nErrors: \n{JsonSerializer.Serialize(result.Errors)}");
                    return ServiceResponse.ErrorResponse(
                        JsonSerializer.Serialize(result.Errors));
                }
                return ServiceResponse.SuccessResponse();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return ServiceResponse.ErrorResponse(
                    $"Validator doesn't exist. Validator name must be [commandName]+\"Validator\". \n\nMore detail: \n{ex.Message}");
            }
        }

        protected async Task<ActionResult> GetCommandResult<T>(T command) where T : class
        {
            var validate = Validate(command);
            if (validate.Success)
            {
                var client = _mediator.CreateRequestClient<T>();
                var response = await client.GetResponse<ServiceResponse>(command);
                if (response.Message != null && response.Message.Success)
                {
                    return new OkObjectResult(response.Message);
                }
                return new BadRequestObjectResult(response.Message);
            }

            return new BadRequestObjectResult(validate);
        }

    }
}
