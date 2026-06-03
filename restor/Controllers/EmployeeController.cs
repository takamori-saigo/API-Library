using AutoMapper;
using Contracts;
using Domains;
using Domains.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace restor.Controllers;

[ApiController]
[Route("api/companies/{companyID}/employees")]
public class EmployeeController: ControllerBase
{
    private readonly ILogger<EmployeeController> _logger;
    private readonly IManagerRepository _repository;
    private readonly IMapper _mapper;
    public EmployeeController(IManagerRepository repository, IMapper mapper, ILogger<EmployeeController> logger)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployees(Guid companyID)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyID, false);
        if (company == null)
        {
            _logger.LogInformation($"Company {companyID} does not exist");
            return NotFound();
        }

        var employeesFromDb = await _repository.EmployeeRepository.GetEmployeesAsync(company.Id, false);

        if (employeesFromDb == null)
        {
            return NotFound();
        }
        
        var employeeDto = employeesFromDb.Select(x => new EmployeeDTO { Name = x.Name, Age = x.Age });
        return Ok(employeeDto);
    }

    [HttpGet("{id}", Name = "GetEmployee")]
    public async Task<IActionResult> GetEmployee(Guid companyID, Guid id)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyID, false);
        if (company == null)
        {
            return NotFound();
        }
        var employee = await _repository.EmployeeRepository.GetEmployeeAsync(company.Id, id,  false);
        if (employee == null)
        {
            return NotFound();
        }
        var dtoEmployee = new EmployeeDTO { Name = employee.Name, Age = employee.Age };
        return Ok(dtoEmployee);
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployee([FromBody] EmployeeForCreatingDTO employeeForCreatingDtoDto, Guid companyID)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyID, false);
        if (company == null) return NotFound();
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        var entity = new Employee
        {
            Id = Guid.NewGuid(),
            Name = employeeForCreatingDtoDto.Name,
            Age = employeeForCreatingDtoDto.Age,
        };
        _repository.EmployeeRepository.CreateEmployee(entity, companyID);
        await _repository.SaveAsync();
        var employeeDto = new EmployeeDTO
        {
            Name = employeeForCreatingDtoDto.Name,
            Age = employeeForCreatingDtoDto.Age,
        };
        return CreatedAtRoute("GetEmployee", new { companyID, id = entity.Id}, employeeDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(Guid companyID, Guid id)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyID, false);
        if (company == null) return NotFound();
        var employee = await _repository.EmployeeRepository.GetEmployeeAsync(company.Id, id, false);
        if (employee == null) return NotFound();
        _repository.EmployeeRepository.DeleteEmployee(employee);
        await _repository.SaveAsync();
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyID, Guid id, [FromBody] EmployeeForUpdateDto employee)
    {
        if (employee == null) return BadRequest();

        var company = await _repository.CompanyRepository.GetCompanyAsync(companyID, false);
        if (company == null) return NotFound();
        var employeeEntity = _repository.EmployeeRepository.GetEmployeeAsync(company.Id, id, true);
        if (employeeEntity == null) return NotFound();
        _mapper.Map(employee, employeeEntity);
        await _repository.SaveAsync();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PartiallyUpdateEmployeeCompany(Guid companyId, Guid id,
        [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
    {
        if (patchDoc == null) return BadRequest();
        var company = await _repository.CompanyRepository.GetCompanyAsync(companyId, false);
        if (company == null) return NotFound();
        var employeeEntity = _repository.EmployeeRepository.GetEmployeeAsync(company.Id, id, true);
        var patchEntity = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        patchDoc.ApplyTo(patchEntity, ModelState);
        TryValidateModel(patchEntity);
        if (!ModelState.IsValid) return UnprocessableEntity(ModelState);
        _mapper.Map(patchEntity, employeeEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}