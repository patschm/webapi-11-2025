using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.DAL.EntityFramework.Entities;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework;
public class ReviewRepository : IReviewRepository
{
    private readonly ILogger<ReviewRepository> _logger;
    private readonly ProductReviewsContext _context;

    public ReviewRepository(ProductReviewsContext context, ILogger<ReviewRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Review> AddAsync(Review entity)
    {
        _logger.LogInformation($"Add Review");
        await _context.Reviews!.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _context.Entry(entity).GetDatabaseValuesAsync();
        return entity;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.LogInformation($"Delete Review {id}");
        var dbEntity = await GetByIdAsync(id);
        if (dbEntity != null)
        {
            _context.Remove(dbEntity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ICollection<Review>> GetAsync(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Get Reviews (page={page}, count={count}");
        return await _context.Reviews!.Skip((page-1) * count).Take(count).ToListAsync();
    }

    public async Task<Review> GetByIdAsync(int id)
    {
        _logger.LogInformation($"Get Review {id}");
        return await _context.Reviews!.FindAsync(id);
    }

  
    public async Task<Review> UpdateAsync(Review entity)
    {
        _logger.LogInformation($"Update Review {entity.Id}");
        var dbEntity = await GetByIdAsync(entity.Id);
        if (dbEntity != null)
        {
            _context.Entry(dbEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
        return dbEntity;
    }
}
