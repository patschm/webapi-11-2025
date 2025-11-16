using ProductsReviews.DAL.Entities;

namespace ProductsReviews.DAL.Interfaces;

public interface IBrandRepository: IRepository<Brand>
{
    Task<ICollection<Product>> GetProductsAsync(int brandId, int page = 1, int count = 10);
}