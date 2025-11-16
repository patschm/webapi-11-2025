using System.ComponentModel.DataAnnotations;

namespace ControllerWeb.DTO;

public class ProductDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    [RegularExpression("[^\\s]+(\\.(?i)(jpe?g|png|gif|bmp))$")]
    public string? Image { get; set; }
    [Required]
    public int? BrandId { get; set; }
    [Required]  
    public int? ProductGroupId { get; set; }
}