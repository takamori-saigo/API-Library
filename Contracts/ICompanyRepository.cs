using Domains;
using Domains.DTO;

namespace Contracts;

public interface ICompanyRepository
{
    IEnumerable<Company> GetCompanies();
    Company GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    IEnumerable<Company> GetCompaniesByIdes(IEnumerable<Guid> userId, bool trackChanges);
}