
using ProductsReviews.DAL.Entities;

namespace ProductsReviews.DAL.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<ICollection<Review>> GetReviewsAsync(int productId, int page = 1, int count = 10);
}