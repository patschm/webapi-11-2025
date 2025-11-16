using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ProductReviews.Client.Proxies
{
    public class BaseProxy<Entity> : IProxy<Entity> where Entity: class
    {
        protected NamingStrategy Strategy { get; } = new CamelCaseNamingStrategy();
        protected string MediaType { get; } = "application/json";
        protected string BaseAddress {get; set;}
        private static Dictionary<string, (DateTime registerTime, HttpClient client)> _clients = new Dictionary<string, (DateTime registerTime, HttpClient client)>();
        private string AccessToken = string.Empty;
      
        public BaseProxy(IOptions<RestOptions> options)
        {
            BaseAddress = options.Value!.RestUrl!;
            if (!BaseAddress!.EndsWith('/')) BaseAddress+="/";
        }
        protected HttpClient CreateHttpClient()
        {
            if (_clients.TryGetValue(BaseAddress, out (DateTime registerTime, HttpClient http)client))
            {
                if (client.registerTime < DateTime.Now.AddMinutes(-10))
                {
                    // Too old. Renew;
                    client.registerTime = DateTime.Now;
                    client.http = new HttpClient { BaseAddress = new Uri(BaseAddress)};
                    return client.http;
                }
                return client.http;
            }
            client = (DateTime.Now, new HttpClient {BaseAddress = new Uri(BaseAddress)});
            _clients.Add(BaseAddress,  client);
            if (!string.IsNullOrEmpty(AccessToken))
            {
                client.http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            }
            return client.http;
        }
        protected virtual async Task<T?> DeserializeAsync<T>(HttpContent content)
        {
            var serializer = new JsonSerializer{
                ContractResolver = new DefaultContractResolver{
                    NamingStrategy = Strategy
                }
            };
            using (Stream stream = await content.ReadAsStreamAsync())
            using (StreamReader sr = new StreamReader(stream))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(reader);
            }
        }
        protected virtual HttpContent Serialize<T>(T data)
        {
            var settings = new JsonSerializerSettings{
                ContractResolver = new DefaultContractResolver {
                    NamingStrategy = Strategy
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(data, settings));
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaType);
            return content;
        }
        public virtual async Task<List<Entity>?> GetAsync(int page = 1, int count = 10)
        {
            var client = CreateHttpClient();
            var result = await client.GetAsync($"?page={page}&count={count}");
            result.EnsureSuccessStatusCode();
            return await DeserializeAsync<List<Entity>>(result.Content);
        }
        public virtual async Task<Entity?> GetByIdAsync(int id)
        {
            var client = CreateHttpClient();
            var result = await client.GetAsync($"{id}");
            result.EnsureSuccessStatusCode();
            return await DeserializeAsync<Entity>(result.Content);
        }
        public virtual async Task<Entity?> PutAsync(int id, Entity entity)
        {
            var client = CreateHttpClient();
            var content = Serialize<Entity>(entity);
            var result = await client.PutAsync($"{id}", content);
            result.EnsureSuccessStatusCode();
            return await DeserializeAsync<Entity>(result.Content);
        }
        public virtual async Task<Entity?> PostAsync(Entity entity)
        {
            var client = CreateHttpClient();
            var content = Serialize<Entity>(entity);
            var result = await client.PostAsync($"", content);
            result.EnsureSuccessStatusCode();    
            return await DeserializeAsync<Entity>(result.Content);
        }
        public virtual async Task DeleteAsync(int id)
        {
            var client = CreateHttpClient();
            var result = await client.DeleteAsync($"{id}");
        }
        public IProxy<Entity> WithBearer(string token)
        {
           AccessToken = token;
           return this;
        }
    }
}