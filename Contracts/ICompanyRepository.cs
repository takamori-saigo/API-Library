using Domains;
using Domains.DTO;

namespace Contracts;

public interface ICompanyRepository
{
    IEnumerable<Company> GetCompanies();
}