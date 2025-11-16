namespace ProductsReviews.DAL.Interfaces;

public interface IRepository<T> where T: class
{
    Task<ICollection<T>> GetAsync(int page = 1, int count = 10);
    Task<T> GetByIdAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task DeleteAsync(int id);
}