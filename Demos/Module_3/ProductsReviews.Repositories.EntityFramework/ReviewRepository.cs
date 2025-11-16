using Microsoft.EntityFrameworkCore;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ProductsReviews.DAL.Repositories;
public class ReviewRepository : IReviewRepository
{
    private readonly ProductReviewsContext _context;

    public ReviewRepository(ProductReviewsContext context)
    {
        _context = context;
    }

    public async Task<Review> AddAsync(Review entity)
    {
        await _context.Reviews.AddAsync(entity);
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

    public async Task<ICollection<Review>> GetAsync(int page = 1, int count = 10)
    {
        return await _context.Reviews.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<Review> GetByIdAsync(int id)
    {
        return await _context.Reviews.FindAsync(id);
    }

  
    public async Task<Review> UpdateAsync(Review entity)
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
