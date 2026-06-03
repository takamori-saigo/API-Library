using Contracts;
using Domains;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class EmployeeRepository: RepositoryBase<Employee> , IEmployeeRepository
{
    public EmployeeRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        return await GetByCondition(o => o.CompanyId.Equals(companyId) , trackChanges)
            .OrderBy(x => x.Name).ToListAsync();
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        return await GetByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(employeeId), trackChanges).SingleOrDefaultAsync();
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