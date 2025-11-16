using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Interfaces
{
    public interface IReviewRepository: IRepository<Review>
    {
        
    }
}