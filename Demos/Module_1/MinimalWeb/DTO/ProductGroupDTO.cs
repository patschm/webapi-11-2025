using System.ComponentModel.DataAnnotations;

namespace MinimalWeb.DTO;

public class ProductGroupDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
    [RegularExpression("[^\\s]+(\\.(?i)(jpe?g|png|gif|bmp))$")]
    public string? Image { get; set; }
}