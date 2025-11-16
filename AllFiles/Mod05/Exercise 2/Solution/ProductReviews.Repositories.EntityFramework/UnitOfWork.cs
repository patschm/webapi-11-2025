using Microsoft.Extensions.Logging;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.Interfaces;

namespace ProductReviews.Repositories.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ProductReviewsContext _context;

        public UnitOfWork(ProductReviewsContext context, ILoggerFactory logger)
        {
            _context = context;
            _loggerFactory = logger;
            ProductGroupRepository = new ProductGroupRepository(_context, _loggerFactory.CreateLogger<ProductGroupRepository>());
            ProductRepository = new ProductRepository(_context, _loggerFactory.CreateLogger<ProductRepository>());
            BrandRepository = new BrandRepository(_context, _loggerFactory.CreateLogger<BrandRepository>());
            ReviewRepository = new ReviewRepository(_context, _loggerFactory.CreateLogger<ReviewRepository>());
        }

        public IProductGroupRepository ProductGroupRepository { get; }

        public IProductRepository ProductRepository {get;}

        public IBrandRepository BrandRepository {get;}

        public IReviewRepository ReviewRepository {get;}

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}