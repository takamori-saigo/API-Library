using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class EmployeeConfiguration: IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        var seed = new DateTime(2024,10,12);
        var seedOfCompanies = new DateTime(2024,2,12);
        builder.HasData(
            new Employee
            {
                Id = GenerateGuid.GetGuid(seed, 1),
                Name = "John Doe",
                Age = 13,
                CompanyId = GenerateGuid.GetGuid(seedOfCompanies, 1)
            }
            ,new Employee
            {
                Id = GenerateGuid.GetGuid(seed, 2),
                Name = "George",
                Age = 75,
                CompanyId = GenerateGuid.GetGuid(seedOfCompanies, 2)
            }
            ,new Employee
            {
                Id = GenerateGuid.GetGuid(seed, 3),
                Name = "Mishal",
                Age = 130,
                CompanyId = GenerateGuid.GetGuid(seedOfCompanies, 3)
            }
            );
    }
}