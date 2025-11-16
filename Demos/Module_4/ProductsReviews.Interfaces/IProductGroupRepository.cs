using ProductsReviews.DAL.Entities;

namespace ProductsReviews.DAL.Interfaces;

public interface IProductGroupRepository: IRepository<ProductGroup>
{
    Task<ICollection<Product>> GetProductsAsync(int productgroupId, int page = 1, int count = 10);
}