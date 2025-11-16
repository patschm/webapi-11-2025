using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductReviews.Client.Proxies
{
    public interface IProxy<Entity> where Entity: class
    {
        IProxy<Entity> WithBearer(string token);
        Task<List<Entity>?> GetAsync(int page = 1, int count = 10);
        Task<Entity?> GetByIdAsync(int id);
        Task<Entity?> PutAsync(int id, Entity entity);
        Task<Entity?> PostAsync(Entity entity);
        Task DeleteAsync(int id);
    }
}