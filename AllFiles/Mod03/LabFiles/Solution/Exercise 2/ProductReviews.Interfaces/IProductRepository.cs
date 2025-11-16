using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<ICollection<Review>> GetReviewsAsync(int productId, int page = 1, int count = 10);
    }
}