using System.ComponentModel.DataAnnotations;

namespace Domains.DTO;

public abstract class EmployeeForManipulationDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Name is required")]
    [Range(18,30, ErrorMessage = "Name must be between 18 and 30")]
    public int Age { get; set; }
}