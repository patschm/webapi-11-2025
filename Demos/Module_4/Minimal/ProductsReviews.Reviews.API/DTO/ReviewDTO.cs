namespace ProductsReviews.Reviews.API.DTO;

public class ReviewDTO
{
    public int Id { get; set; }
    public string? Author { get; set; }
    public string? Email { get; set; }
    public string? Text { get; set; }
    public int Score { get; set; }
    public int ProductId { get; set; }
}
