using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.DAL.EntityFramework.Entities;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework;
public class BrandRepository : IBrandRepository
{
    private readonly ILogger<BrandRepository> _logger;
    private readonly ProductReviewsContext _context;

    public BrandRepository(ProductReviewsContext context, ILogger<BrandRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Brand> AddAsync(Brand entity)
    {
        _logger.LogInformation("Add Brand");
        await _context.Brands!.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).GetDatabaseValuesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Delete Brand ({id})");
        var dbEntity = await GetByIdAsync(id);
        if (dbEntity != null)
        {
            _context.Remove(dbEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Brand>> GetAsync(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Get all Brands (page={page}, count={count})");
        return await _context.Brands!.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<Brand> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get Brand ({id})");
        return await _context.Brands!.FindAsync(id);
    }

    public async Task<ICollection<Product>> GetProductsAsync(int brandId, int page = 1, int count = 10)
    {
        _logger.LogInformation($"Products for brand {brandId}");
        return await _context.Products
            .Where(p=>p.BrandId == brandId)
            .Skip((page-1)*count).Take(count)
            .ToListAsync();
    }

    public async Task<Brand> UpdateAsync(Brand entity)
    {
        _logger.LogInformation($"Update Brand ({entity.Id})");
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity != null)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
        return dbEntity;
    }
}
