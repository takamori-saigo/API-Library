using Contracts;
using Domains;
using Domains.DTO;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace restor.Controllers;

[Route("api/companies/{companyID}/employees")]
public class EmployeeController: ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IManagerRepository _repository;
    public EmployeeController(IManagerRepository repository, ILogger<EmployeeController> logger)
    {
        _logger = logger;
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetEmployees(Guid companyID)
    {
        var company = _repository.CompanyRepository.GetCompany(companyID, false);
        if (company == null)
        {
            _logger.LogInformation($"Company {companyID} does not exist");
            return NotFound();
        }

        var employeesFromDb = _repository.EmployeeRepository.GetEmployees(company.Id, false);

        if (employeesFromDb == null)
        {
            return NotFound();
        }
        
        var employeeDto = employeesFromDb.Select(x => new EmployeeDTO { Name = x.Name, Age = x.Age });
        return Ok(employeeDto);
    }

    [HttpGet("{id}", Name = "GetEmployee")]
    public IActionResult GetEmployee(Guid companyID, Guid id)
    {
        var company = _repository.CompanyRepository.GetCompany(companyID, false);
        if (company == null)
        {
            return NotFound();
        }
        var employee = _repository.EmployeeRepository.GetEmployee(company.Id, id,  false);
        if (employee == null)
        {
            return NotFound();
        }
        var dtoEmployee = new EmployeeDTO { Name = employee.Name, Age = employee.Age };
        return Ok(dtoEmployee);
    }

    [HttpPost]
    public IActionResult CreateEmployee([FromBody] EmployeeForCreating employeeForCreatingDto, Guid companyID)
    {
        var company = _repository.CompanyRepository.GetCompany(companyID, false);
        if (company == null) return NotFound();
        var entity = new Employee
        {
            Id = Guid.NewGuid(),
            Name = employeeForCreatingDto.Name,
            Age = employeeForCreatingDto.Age,
        };
        _repository.EmployeeRepository.CreateEmployee(entity, companyID);
        _repository.Save();
        var employeeDto = new EmployeeDTO
        {
            Name = employeeForCreatingDto.Name,
            Age = employeeForCreatingDto.Age,
        };
        return CreatedAtRoute("GetEmployee", new { companyID, id = entity.Id}, employeeDto);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteEmployee(Guid companyID, Guid id)
    {
        var company = _repository.CompanyRepository.GetCompany(companyID, false);
        if (company == null) return NotFound();
        var employee = _repository.EmployeeRepository.GetEmployee(company.Id, id, false);
        if (employee == null) return NotFound();
        _repository.EmployeeRepository.DeleteEmployee(employee);
        _repository.Save();
        return NoContent();
    }
}