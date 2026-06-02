using Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class CompanyConfiguration: IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {

        
        var seed = new DateTime(2024,2,12);
        builder.HasData(
            new Company
            {
                Id = GenerateGuid.GetGuid(seed, 1),
                Name = "Apple",
                Address = "avenue",
                Country = "USA",
            },new Company
            {
                Id = GenerateGuid.GetGuid(seed, 2),
                Name = "Google",
                Address = "Moscow",
                Country = "Russia",
            },new Company
            {
                Id = GenerateGuid.GetGuid(seed, 3),
                Name = "Telegram",
                Address = "Leningradka",
                Country = "Germany",
            }
            );
    }
}