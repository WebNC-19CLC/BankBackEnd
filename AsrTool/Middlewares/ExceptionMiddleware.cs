using System.Net;
using System.Security;
using AsrTool.Infrastructure.Exceptions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AsrTool.Middlewares
{
  public class ExceptionMiddleware
  {
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
      _logger = logger;
      _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "An error has been occurred");
        await HandleExceptionAsync(context, ex);
      }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
      string message = exception.Message;

      switch (exception)
      {
        case DbUpdateConcurrencyException:
          context.Response.StatusCode = (int)HttpStatusCode.PreconditionFailed;
          message = "concurrentUpdate";
          break;

        case UnauthorizerException:
          context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
          message = exception.Message;
          break;

        case NotFoundException:
          context.Response.StatusCode = (int)HttpStatusCode.NotFound;
          break;

        case BusinessException:
          context.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
          break;

        case SecurityException:
          context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
          break;

        case ArgumentException:
          context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
          break;

        default:
          context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
          message = exception.Message;
          break;
      }

      var response = context.Response;
      response.ContentType = "application/json";
      return response.WriteAsync(JsonConvert.SerializeObject(new { Message = message }));
    }
  }
}
