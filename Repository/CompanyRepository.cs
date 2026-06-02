using Contracts;
using Domains;
using Domains.DTO;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
    
    public async Task<IEnumerable<Company>> GetCompaniesAsync(bool tackChanges)
    {
        return await GetAll(tackChanges).OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        return await GetByCondition(o => o.Id.Equals(companyId), trackChanges).SingleOrDefaultAsync();
    }
    
    public void CreateCompany(Company company)
    {
        Add(company);
    }

    public async Task<IEnumerable<Company>> GetCompaniesByIdesAsync(IEnumerable<Guid> userId, bool trackChanges)
    {
        return await GetByCondition(o => userId.Contains(o.Id), trackChanges).ToListAsync();
    }

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
}