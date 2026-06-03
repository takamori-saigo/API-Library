using Domains;
using restor;

namespace Contracts;

public interface IEmployeeRepository
{
    Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameter, bool trackChanges);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Employee employee, Guid companyId);
    void DeleteEmployee(Employee employee);
}

