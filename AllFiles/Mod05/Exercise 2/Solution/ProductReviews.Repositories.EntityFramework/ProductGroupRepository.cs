using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.DAL.EntityFramework.Entities;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework;
public class ProductGroupRepository : IProductGroupRepository
{
    private readonly ILogger<ProductGroupRepository> _logger;
    private readonly ProductReviewsContext _context;

    public ProductGroupRepository(ProductReviewsContext context, ILogger<ProductGroupRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ProductGroup> AddAsync(ProductGroup entity)
    {
        await _context.ProductGroups!.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry<ProductGroup>(entity).GetDatabaseValuesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Delete ProductGroup {id}");
        var dbEntity = await GetByIdAsync(id);
        if (dbEntity != null)
        {
            _context.Remove(dbEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<ProductGroup>> GetAsync(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Get ProductGroups (page={page}, count={count}");
        return await _context.ProductGroups!.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<ProductGroup> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get ProductGroup {id}");
        return await _context.ProductGroups!.FindAsync(id);
    }

    public async Task<ICollection<Product>> GetProductsAsync(int productgroupId, int page = 1, int count = 10)
    {
        _logger.LogInformation($"Get Products by ProductGroupId {productgroupId}");
        return await _context.Products
            .Where(p=>p.ProductGroupId == productgroupId)
            .Skip((page-1)*count).Take(count)
            .ToListAsync();
    }

    public async Task<ProductGroup> UpdateAsync(ProductGroup entity)
    {
        _logger.LogInformation($"Update ProductGroup {entity.Id}");
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity != null)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
        return dbEntity;
    }
}
