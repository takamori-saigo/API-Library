using Contracts;
using Domains;
using Infrastructure;

namespace Repository;

public class EmployeeRepository: RepositoryBase<Employee> , IEmployeeRepository
{
    public EmployeeRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges)
    {
        return GetByCondition(o => o.CompanyId.Equals(companyId) , trackChanges)
            .OrderBy(x => x.Name);
    }

    public Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return GetByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefault();
    }

    public void CreateEmployee(Employee employee, Guid companyId)
    {
        employee.CompanyId = companyId;
        Add(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        Delete(employee);
    }
}