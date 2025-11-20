using Citas.Domain.Exceptions;

namespace WebApi.Middlewares;

internal class ErrorsMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ErrorsMiddleware> _logger;

  public ErrorsMiddleware(RequestDelegate next, ILogger<ErrorsMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (CitasException ex)
    {
      _logger.LogError(ex, ex.Message);
      context.Response.StatusCode = ex switch
      {
        AlreadyExistException => StatusCodes.Status409Conflict,
        NotFoundException => StatusCodes.Status404NotFound,
        CitasInternalException => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
      };
      await context.Response.WriteAsJsonAsync(new
      {
        type = ex.Type,
        message = ex.Message
      });
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, ex.Message);
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      var customEx = new CitasInternalException();
      await context.Response.WriteAsJsonAsync(new
      {
        type = customEx.Type,
        message = customEx.Message
      });
    }
  }
}
