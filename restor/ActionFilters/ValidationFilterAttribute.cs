using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace restor.ActionFilters;

public class ValidationFilterAttribute: IActionFilter
{
    private readonly ILogger<ValidationFilterAttribute> _logger;
    public ValidationFilterAttribute(ILogger<ValidationFilterAttribute> logger)
    {
        _logger = logger;
    }
    
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var action = context.RouteData.Values["action"];
        var controller = context.RouteData.Values["controller"];
        
        var param = context.ActionArguments
            .SingleOrDefault(x => x.Value.ToString().Contains("DTO") || x.Value.ToString().Contains("Dto")).Value;

        if (param == null)
        {
            _logger.LogError("object sent from client is null, Controller: {controller}, action: {action}", action, controller);
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
        }
    } 

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}