using Microsoft.EntityFrameworkCore;
using Products.DAL.Entities;

namespace Products.DAL.Database;

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