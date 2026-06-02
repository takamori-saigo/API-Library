using System.ComponentModel.DataAnnotations;

namespace Domains.DTO;

public class CompanyForCreationDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    [Required(ErrorMessage = "Phone number is required")]
    public string Country { get; set; } 
}