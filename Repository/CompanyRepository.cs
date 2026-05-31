using Contracts;
using Domains;
using Infrastructure;

namespace Repository;

public class CompanyRepository: RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
    
    public IEnumerable<Company> GetCompanies()
    {
        return GetAll(false);
    }
}