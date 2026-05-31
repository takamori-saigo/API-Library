using Domains;

namespace Contracts;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
    Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployee(Employee employee, Guid companyId);
    void DeleteEmployee(Employee employee);
}