using Microsoft.EntityFrameworkCore;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.DAL.EntityFramework.Entities;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework;
public class ProductRepository : IProductRepository
{
    private readonly ProductReviewsContext _context;

    public ProductRepository(ProductReviewsContext context)
    {
        _context = context;
    }

    public async Task<Product> AddAsync(Product entity)
    {
        await _context.Products!.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).GetDatabaseValuesAsync();
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

    public async Task<ICollection<Product>> GetAsync(int page = 1, int count = 10)
    {
        return await _context.Products!.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        return await _context.Products!.FindAsync(id);
    }

    public async Task<ICollection<Review>> GetReviewsAsync(int productId, int page = 1, int count = 10)
    {
        return await _context.Reviews
            .Where(p=>p.ProductId == productId)
            .Skip((page-1)*count).Take(count)
            .ToListAsync();
    }

    public async Task<Product> UpdateAsync(Product entity)
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
