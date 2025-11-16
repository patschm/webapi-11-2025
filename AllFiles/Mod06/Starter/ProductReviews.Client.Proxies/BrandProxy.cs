using Microsoft.Extensions.Options;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.Client.Proxies
{
    public class BrandProxy: BaseProxy<Brand>, IBrandProxy
    {
        public BrandProxy(IOptions<RestOptions> options)
            :base(options)
        {
            BaseAddress =$"{options.Value!.RestUrl!.TrimEnd('/')}/{nameof(Brand).ToLower()}/";
        }
    }
}