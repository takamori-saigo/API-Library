using Domains;
using Domains.DTO;

namespace Contracts;

public interface ICompanyRepository
{
    Task<IEnumerable<Company>> GetCompaniesAsync(bool tackChanges);
    Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    Task<IEnumerable<Company>> GetCompaniesByIdesAsync(IEnumerable<Guid> userId, bool trackChanges);
    void DeleteCompany(Company company);
}