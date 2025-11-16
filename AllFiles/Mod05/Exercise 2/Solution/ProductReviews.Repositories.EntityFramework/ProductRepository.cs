using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.DAL.EntityFramework.Entities;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework;
public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly ProductReviewsContext _context;

    public ProductRepository(ProductReviewsContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product> AddAsync(Product entity)
    {
        _logger.LogInformation($"Add Product");
        await _context.Products!.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).GetDatabaseValuesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Delete Product {id}");
        var dbEntity = await GetByIdAsync(id);
        if (dbEntity != null)
        {
            _context.Remove(dbEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Product>> GetAsync(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Get Products (page={page}, count={count})");
        return await _context.Products!.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get Product {id}");
        return await _context.Products!.FindAsync(id);
    }

    public async Task<ICollection<Review>> GetReviewsAsync(int productId, int page = 1, int count = 10)
    {
        _logger.LogInformation($"Get Reviews by ProductId: {productId}");
        return await _context.Reviews
            .Where(p=>p.ProductId == productId)
            .Skip((page-1)*count).Take(count)
            .ToListAsync();
    }

    public async Task<Product> UpdateAsync(Product entity)
    {
        _logger.LogInformation($"Update Product {entity.Id}");
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity != null)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
        return dbEntity;
    }
}
