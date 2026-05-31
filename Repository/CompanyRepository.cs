using Contracts;
using Domains;
using Domains.DTO;
using Infrastructure;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
    
    public IEnumerable<Company> GetCompanies()
    {
        return GetAll(false);
    }

    public Company GetCompany(Guid companyId, bool trackChanges)
    {
        return GetByCondition(o => o.Id.Equals(companyId), trackChanges).SingleOrDefault();
    }

    public void CreateCompany(Company company)
    {
        Add(company);
    }

    public IEnumerable<Company> GetCompaniesByIdes(IEnumerable<Guid> userId, bool trackChanges)
    {
        return GetByCondition(o => userId.Contains(o.Id), trackChanges);
    }
}