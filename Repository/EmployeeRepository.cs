using Contracts;
using Domains;
using Infrastructure;

namespace Repository;

public class EmployeeRepository: RepositoryBase<Employee> , IEmployeeRepository
{
    public EmployeeRepository(RestorDbContext restorDbContext) : base(restorDbContext) { }
}