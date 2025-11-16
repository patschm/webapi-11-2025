using System.ComponentModel.DataAnnotations;

namespace ControllerWeb.DTO;

public class BrandDTO   
{
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
}