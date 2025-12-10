using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CleanModelStateFilter : IActionFilter
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    if (!context.ModelState.IsValid)
    {
      var errors = context.ModelState
          .Where(x => x.Value?.Errors.Count > 0)
          .Select(x => new
          {
            Field = CleanFieldName(x.Key),
            Messages = x.Value?.Errors.Select(e => e.ErrorMessage)
          });

      context.Result = new BadRequestObjectResult(new
      {
        type = "ValidationError",
        title = "Error de validación de datos.",
        status = 400,
        errors = errors
      });
    }
  }

  public void OnActionExecuted(ActionExecutedContext context) { }

  private string CleanFieldName(string key)
  {
    var idx = key.IndexOf('.');
    return idx >= 0 ? key[(idx + 1)..] : key;
  }
}