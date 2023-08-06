using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Riktam.GroupChat.Domain.Common;

namespace Riktam.GroupChat.Apis.Filters;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        _logger = logger;
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An error occurred while processing the request.");

        if (context.Exception is ClientException clientException)
        {
            context.Result = new BadRequestObjectResult(GetProblemDetails(clientException));
        }
        else if (context.Exception is ConflictException conflictException)
        {
            context.Result = new ConflictObjectResult(GetProblemDetails(conflictException));
        }
        else if (context.Exception is UnauthorizedAccessException)
        {
            context.Result = new UnauthorizedResult();
        }
        else
        {
            context.Result = new ObjectResult(new { Message = "An error occurred while processing the request." })
            {
                StatusCode = 500
            };
        }

        context.ExceptionHandled = true;

        return Task.CompletedTask;
    }

    private static ProblemDetails GetProblemDetails(ConflictException exception)
    {
        return new ProblemDetails
        {
            Type = exception.ErrorCode.ToString(),
            Title = exception.ErrorCode.GetDescription(),
            Status = StatusCodes.Status409Conflict,
        };
    }
    private static ProblemDetails GetProblemDetails(ClientException exception)
    {
        return new ProblemDetails
        {
            Type = exception.ErrorCode.ToString(),
            Title = exception.ErrorCode.GetDescription(),
            Status = StatusCodes.Status400BadRequest
        };
    }
}


