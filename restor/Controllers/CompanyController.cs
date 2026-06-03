using AutoMapper;
using Contracts;
using Domains;
using Domains.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using restor.ActionFilters;
using restor.ModelBinders;

namespace restor.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController: ControllerBase
{
    private readonly IManagerRepository _repository;
    private readonly ILogger<CompanyController> _logger;
    private readonly IMapper _mapper;
    public CompanyController(IManagerRepository repository, IMapper mapper, ILogger<CompanyController> logger)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async  Task<IActionResult> GetCompanies()
    {
        _logger.LogInformation("Get Companies");
        var companies = await _repository.CompanyRepository.GetCompaniesAsync(false);
        var dto = companies.Select(x => new CompanyDTO{Name = x.Name, Address = x.Address, Country = x.Country});
        return Ok(dto);
    }

    [HttpGet("{id}", Name = "CompanyById")]
    public async Task<IActionResult> GetCompany(Guid id)
    {
        var company = await _repository.CompanyRepository.GetCompanyAsync(id, trackChanges: false);
        if (company == null)
        {
            _logger.LogInformation("$Company {id} not found");
            return NotFound();
        }

        var companyDto = new CompanyDTO { Name = company.Name, Address = company.Address, Country = company.Country };
        return Ok(companyDto);
    }

    [HttpGet("collection/({ids})", Name = "CollectionsByCompanyId")]
    public async Task<IActionResult> GetCompaniesByIds([ModelBinder(BinderType = typeof(ArrayModelBuilder))]IEnumerable<Guid> ids)
    {
        if (ids == null) return BadRequest();

        var companies = await _repository.CompanyRepository.GetCompaniesByIdesAsync(ids, trackChanges: false);
        
        if (companies.Count() != ids.Count()) return NotFound();
        
        var dto = companies.Select(x => new CompanyDTO { Name = x.Name, Address = x.Address, Country = x.Country });

        return Ok(dto);
    }

    [HttpPost("collection")]
    public async Task<IActionResult> CreateCollectionCompany([FromBody]IEnumerable<CompanyForCreationDto> dto)
    {
        if (dto == null) return BadRequest();
        
        var comapnies = dto.Select(x =>
            new Company {Id = new Guid(), Name = x.Name, Address = x.Address, Country = x.Country});

        foreach (var c in comapnies)
        {
            _repository.CompanyRepository.CreateCompany(c);
        }
        await _repository.SaveAsync();
        if (comapnies.Count() == 0) return NotFound();
        
        var companyDto = comapnies.Select(x => new CompanyDTO { Name = x.Name, Address = x.Address, Country = x.Country });
        return CreatedAtRoute("CollectionsByCompanyId", comapnies.Select(x => x.Id), companyDto);
    }
    
    [HttpPost]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> CreateCompany([FromBody] CompanyForCreationDto createCompanyDto)
    {
        if (createCompanyDto == null)
        {
            return BadRequest("CompanyDto is null");
        }
        
        var companyEntity = new Company()
        {
            Id = Guid.NewGuid(),
            Name = createCompanyDto.Name,
            Address = createCompanyDto.Address,
            Country = createCompanyDto.Country,
        };
        _repository.CompanyRepository.CreateCompany(companyEntity);
        await _repository.SaveAsync();
        var companyDto = new CompanyDTO
            {Name = companyEntity.Name,
                Address = companyEntity.Address,
                Country = companyEntity.Country
            };
        return CreatedAtRoute("CompanyById", new {id = companyEntity.Id}, companyDto);
    }

    [HttpDelete("{id}")]
    [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
    [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
    public async Task<IActionResult> DeleteCompany(Guid id)
    {
        var company = HttpContext.Items["Company"] as Company;
        _repository.CompanyRepository.DeleteCompany(company);
        await _repository.SaveAsync();
        return NoContent();
    }
    
    [HttpPut("{id}")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] CompanyForUpdateDTO company)
    {
        var companyEntity = HttpContext.Items["Company"] as Company;
        _mapper.Map(company, companyEntity);
        await _repository.SaveAsync();
        return NoContent();
    }
}