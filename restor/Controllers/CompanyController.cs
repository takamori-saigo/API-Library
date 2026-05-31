using Contracts;
using Domains;
using Domains.DTO;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using restor.ModelBinders;

namespace restor.Controllers;

[ApiController]
[Route("api/companies")]
public class CompanyController: ControllerBase
{
    private readonly IManagerRepository _repository;
    private readonly ILogger<CompanyController> _logger;
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

    [HttpGet("{id}", Name = "CompanyById")]
    public IActionResult GetCompany(Guid id)
    {
        var company = _repository.CompanyRepository.GetCompany(id, trackChanges: false);
        if (company == null)
        {
            _logger.LogInformation("$Company {id} not found");
            return NotFound();
        }

        var companyDto = new CompanyDTO { Name = company.Name, Address = company.Address, Country = company.Country };
        return Ok(companyDto);
    }

    [HttpGet("collection/({ids})", Name = "CollectionsByCompanyId")]
    public IActionResult GetCompaniesByIds([ModelBinder(BinderType = typeof(ArrayModelBuilder))]IEnumerable<Guid> ids)
    {
        if (ids == null) return BadRequest();

        var companies = _repository.CompanyRepository.GetCompaniesByIdes(ids, trackChanges: false);
        
        if (companies.Count() != ids.Count()) return NotFound();
        
        var dto = companies.Select(x => new CompanyDTO { Name = x.Name, Address = x.Address, Country = x.Country });

        return Ok(dto);
    }

    [HttpPost("collection")]
    public IActionResult CreateCollectionCompany([FromBody]IEnumerable<CompanyForCreationDto> dto)
    {
        if (dto == null) return BadRequest();
        
        var comapnies = dto.Select(x =>
            new Company {Id = new Guid(), Name = x.Name, Address = x.Address, Country = x.Country});
        
        if (comapnies.Count() == 0) return NotFound();
        
        var companyDto = comapnies.Select(x => new CompanyDTO { Name = x.Name, Address = x.Address, Country = x.Country });
        return CreatedAtRoute("CollectionsByCompanyId", comapnies.Select(x => x.Id), companyDto);
    }
    
    [HttpPost]
    public IActionResult CreateCompany([FromBody] CompanyForCreationDto createCompanyDto)
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
        _repository.Save();
        var companyDto = new CompanyDTO
            {Name = companyEntity.Name,
                Address = companyEntity.Address,
                Country = companyEntity.Country
            };
        return CreatedAtRoute("CompanyById", new {id = companyEntity.Id}, companyDto);
    }
}