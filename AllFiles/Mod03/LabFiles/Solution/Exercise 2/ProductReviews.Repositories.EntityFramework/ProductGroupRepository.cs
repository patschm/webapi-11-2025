using Microsoft.EntityFrameworkCore;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.DAL.EntityFramework.Entities;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework;
public class ProductGroupRepository : IProductGroupRepository
{
    private readonly ProductReviewsContext _context;

    public ProductGroupRepository(ProductReviewsContext context)
    {
        _context = context;
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
        var dbEntity = await GetByIdAsync(id);
        if (dbEntity != null)
        {
            _context.Remove(dbEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<ProductGroup>> GetAsync(int page = 1, int count = 10)
    {
        return await _context.ProductGroups!.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<ProductGroup> GetByIdAsync(int id)
    {
        return await _context.ProductGroups!.FindAsync(id);
    }

    public async Task<ICollection<Product>> GetProductsAsync(int productgroupId, int page = 1, int count = 10)
    {
        return await _context.Products
            .Where(p=>p.ProductGroupId == productgroupId)
            .Skip((page-1)*count).Take(count)
            .ToListAsync();
    }

    public async Task<ProductGroup> UpdateAsync(ProductGroup entity)
    {
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity != null)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
        return dbEntity;
    }
}
