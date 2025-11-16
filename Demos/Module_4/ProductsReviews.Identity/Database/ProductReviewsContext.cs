using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProductsReviews.Identity.Database;

public class ProductReviewsContext : IdentityDbContext<IdentityUser>
{
    public ProductReviewsContext(DbContextOptions<ProductReviewsContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
}
