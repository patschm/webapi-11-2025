using Microsoft.EntityFrameworkCore;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.DAL.EntityFramework.Database;

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

    public DbSet<ProductGroup>? ProductGroups { get; set; }
    public DbSet<Product>? Products { get; set; }
    public DbSet<Brand>? Brands { get; set; }
    public DbSet<Review>? Reviews { get; set; }
    
    private void InitialDBContext()
    {
        DbInitializer.Initialize(this);
    }
}