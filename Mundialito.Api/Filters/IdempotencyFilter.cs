using Mundialito.Application.Abstractions;
using Mundialito.Domain.Entities;
using System.Text.Json;

namespace Mundialito.Api.Filters
{
    public class IdempotencyFilter : IEndpointFilter
    {
        private readonly IIdempotencyService _idempotencyService;

        public IdempotencyFilter(IIdempotencyService idempotencyService)
        {
            _idempotencyService = idempotencyService;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Idempotency-Key", out var keyValue) || !Guid.TryParse(keyValue, out var key))
            {
                return Results.BadRequest("El header 'Idempotency-Key' es requerido y debe ser un GUID válido.");
            }

            var existingRequest = await _idempotencyService.GetAsync(key, context.HttpContext.RequestAborted);
            if (existingRequest != null)
            {
                var cachedResponse = JsonSerializer.Deserialize<object>(existingRequest.ResponseBody);
                return Results.Ok(cachedResponse);
            }

            var result = await next(context);

            var responseBody = JsonSerializer.Serialize(result);
            var idempotencyRequest = new IdempotencyRequest(key, string.Empty, responseBody);
            await _idempotencyService.SaveAsync(idempotencyRequest, context.HttpContext.RequestAborted);

            return result;
        }
    }
}
