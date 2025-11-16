using Microsoft.EntityFrameworkCore;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ProductsReviews.DAL.Repositories;
public class BrandRepository : IBrandRepository
{
    private readonly ProductReviewsContext _context;

    public BrandRepository(ProductReviewsContext context)
    {
        _context = context;
    }

    public async Task<Brand> AddAsync(Brand entity)
    {
        await _context.Brands.AddAsync(entity);
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

    public async Task<ICollection<Brand>> GetAsync(int page = 1, int count = 10)
    {
        return await _context.Brands.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<Brand> GetByIdAsync(int id)
    {
        return await _context.Brands.FindAsync(id);
    }

    public async Task<ICollection<Product>> GetProductsAsync(int brandId, int page = 1, int count = 10)
    {
        return await _context.Products
            .Where(p=>p.BrandId == brandId)
            .Skip((page-1)*count).Take(count)
            .ToListAsync();
    }

    public async Task<Brand> UpdateAsync(Brand entity)
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
