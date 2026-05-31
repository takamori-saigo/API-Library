using Contracts;
using Domains;
using Domains.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace restor.Controllers;

[ApiController]
[Route("[controller]")]
public class CompanyController: ControllerBase
{
    private IManagerRepository _repository;
    private ILogger<CompanyController> _logger;
    public CompanyController(IManagerRepository repository, ILogger<CompanyController> logger)
    {
        _repository = repository;
        _logger = logger;
        _logger.LogInformation("Get in Controller");
    }

    [HttpGet]
    public IActionResult GetCompanies()
    {
        _logger.LogInformation("Get Companies");
        var companies = _repository.CompanyRepository.GetCompanies();
        var dto = companies.Select(x => new CompanyDTO{Name = x.Name, Address = x.Address, Country = x.Country});
        return Ok(dto);
    }
}