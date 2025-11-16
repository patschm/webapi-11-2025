using System.ComponentModel.DataAnnotations;

namespace MinimalWeb.DTO;

public class ReviewDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string? Author { get; set; }
    [Required]
    [MaxLength(255)]
    [RegularExpression("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    public string? Email { get; set; }
    [Required]
    [MaxLength(1024)]
    public string? Text { get; set; }
    [Required]
    [Range(1, 5)]
    public int Score { get; set; }
    [Required]
    public int ProductId { get; set; }
}
