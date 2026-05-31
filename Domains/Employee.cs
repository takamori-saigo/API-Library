using System.ComponentModel.DataAnnotations.Schema;

namespace Domains;

public class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    [ForeignKey("Company")]
    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
}