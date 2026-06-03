using Contracts;
using Domains;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace restor.ActionFilters;

public class ValidateCompanyExistsAttribute: IAsyncActionFilter
{
    private readonly IManagerRepository _repositoryManager;
    private readonly ILogger<Company> _logger;
    
    public ValidateCompanyExistsAttribute(IManagerRepository managerRepository, ILogger<Company> logger)
    {
        _repositoryManager = managerRepository;
        _logger = logger;
    }
    
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
        var id = (Guid)context.ActionArguments["id"];
        var company = await _repositoryManager.CompanyRepository.GetCompanyAsync(id, trackChanges);
        if (company == null) context.Result = new NotFoundResult();
        else
        {
            context.HttpContext.Items["Company"] = company; 
            await next();
        }
    }
}