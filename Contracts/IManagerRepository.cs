namespace Contracts;

public interface IManagerRepository
{
    ICompanyRepository CompanyRepository { get; }
    IEmployeeRepository EmployeeRepository { get; }
    void Save();
}