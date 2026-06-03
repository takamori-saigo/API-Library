using Contracts;
using Domains;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using restor;

namespace Repository;

public class EmployeeRepository: RepositoryBase<Employee> , IEmployeeRepository
{
    public EmployeeRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameter, bool trackChanges)
    {
        var employee = await GetByCondition(e => e.CompanyId.Equals(companyId) && 
                (e.Age >= employeeParameter.MinAge && e.Age <= employeeParameter.MaxAge), trackChanges) 
            .OrderBy(x => x.Name).ToListAsync();
        return PagedList<Employee>.ToPagedList(employee, employeeParameter.PageNumber, employeeParameter.PageSize);
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