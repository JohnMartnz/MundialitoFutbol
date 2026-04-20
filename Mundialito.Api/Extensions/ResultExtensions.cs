using Mundialito.Domain.Common;

namespace Mundialito.Api.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToHttpResult<T>(this Result<T> result, string? createdAtUri = null)
        {
            if (result.IsSuccess)
            {
                if (createdAtUri is not null)
                {
                    return Results.Created(createdAtUri, result.Value);
                }

                return Results.Ok(result.Value);
            }

            return result.ErrorCode switch
            {
                Errors.NotFound => Results.NotFound(new ErrorResponse(result.ErrorMessage!)),
                Errors.Conflict => Results.Conflict(new ErrorResponse(result.ErrorMessage!)),
                Errors.BadRequest => Results.BadRequest(new ErrorResponse(result.ErrorMessage!)),
                Errors.Validation => Results.BadRequest(new ErrorResponse(result.ErrorMessage!)),
                _ => Results.BadRequest(new ErrorResponse(result.ErrorMessage!))
            };
        }

        public static IResult ToHttpResult(this Result result)
        {
            if (result.IsSuccess)
            {
                return Results.Ok();
            }
            return result.ErrorCode switch
            {
                Errors.NotFound => Results.NotFound(new ErrorResponse(result.ErrorMessage!)),
                Errors.Conflict => Results.Conflict(new ErrorResponse(result.ErrorMessage!)),
                Errors.BadRequest => Results.BadRequest(new ErrorResponse(result.ErrorMessage!)),
                Errors.Validation => Results.BadRequest(new ErrorResponse(result.ErrorMessage!)),
                _ => Results.BadRequest(new ErrorResponse(result.ErrorMessage!))
            };
        }
    }
}
