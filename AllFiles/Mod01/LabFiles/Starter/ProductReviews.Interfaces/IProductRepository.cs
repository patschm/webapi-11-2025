using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Interfaces;

public interface IProductRepository : IRepository<Product>
{
    Task<ICollection<Review>> GetReviewsAsync(int productId, int page = 1, int count = 10);
}