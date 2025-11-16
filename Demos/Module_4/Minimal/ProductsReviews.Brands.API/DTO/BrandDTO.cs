using System.ComponentModel.DataAnnotations;

namespace ProductsReviews.Brands.API.DTO;

public class BrandDTO   
{
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Name { get; set; }
}