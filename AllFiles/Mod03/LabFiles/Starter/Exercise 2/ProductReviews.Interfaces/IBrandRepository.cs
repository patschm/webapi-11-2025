using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Interfaces
{
    public interface IBrandRepository: IRepository<Brand>
    {
        Task<ICollection<Product>> GetProductsAsync(int brandId, int page = 1, int count = 10);
    }
}