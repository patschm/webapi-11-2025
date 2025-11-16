
namespace ProductsReviews.DAL.Entities;

public class ProductGroup
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
}