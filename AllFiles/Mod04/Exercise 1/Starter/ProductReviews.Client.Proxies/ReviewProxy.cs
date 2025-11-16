using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Client.Proxies
{
    public class ReviewProxy: BaseProxy<Review>, IReviewProxy
    {
        public ReviewProxy(IOptions<RestOptions> options)
            :base(options)
        {
            BaseAddress =$"{options.Value!.RestUrl!.TrimEnd('/')}/{nameof(Review).ToLower()}/";
        }
    }
}