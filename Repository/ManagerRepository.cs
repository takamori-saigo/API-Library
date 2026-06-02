using Contracts;
using Infrastructure;

namespace Repository;

public class ManagerRepository: IManagerRepository
{
    private RestorDbContext _context;
    private CompanyRepository _companyRepository;
    private EmployeeRepository _employeeRepository;
    public ManagerRepository(RestorDbContext restorDbContext)
    {
        _context = restorDbContext;
    }

    public ICompanyRepository CompanyRepository
    {
        get
        {
            if (_companyRepository == null) _companyRepository = new CompanyRepository(_context);
            return _companyRepository;
        }
    }

    public IEmployeeRepository EmployeeRepository
    {
        get
        {
            if (_employeeRepository == null) _employeeRepository = new EmployeeRepository(_context);
            return _employeeRepository;
        }
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}