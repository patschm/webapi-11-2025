using Microsoft.EntityFrameworkCore;
using ProductsReviews.DAL.Entities;

namespace ProductsReviews.DAL.EntityFramework;

public class ProductReviewsContext: DbContext
{
    public ProductReviewsContext(DbContextOptions<ProductReviewsContext> options): base(options)
    {
        InitialDBContext();
    }
    public ProductReviewsContext()
    {
        InitialDBContext();
    }

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Review> Reviews => Set<Review>();
    
    private void InitialDBContext()
    {
        DbInitializer.Initialize(this);
    }
}