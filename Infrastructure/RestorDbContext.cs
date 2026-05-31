using Domains;
using Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class RestorDbContext: DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }
    
    public RestorDbContext(DbContextOptions<RestorDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
    }
}