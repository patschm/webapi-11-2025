namespace ProductsReviews.DAL.Interfaces;

public interface IUnitOfWork
{
    IProductGroupRepository ProductGroupRepository { get; }
    IProductRepository ProductRepository { get; }
    IBrandRepository BrandRepository { get; }
    IReviewRepository ReviewRepository { get; }
    Task SaveAsync();
}