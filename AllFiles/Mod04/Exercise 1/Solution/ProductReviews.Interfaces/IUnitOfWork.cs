using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductReviews.Interfaces
{
    public interface IUnitOfWork
    {
        IProductGroupRepository ProductGroupRepository { get; }
        IProductRepository ProductRepository { get; }
        IBrandRepository BrandRepository { get; }
        IReviewRepository ReviewRepository { get; }
        Task SaveAsync();
    }
}