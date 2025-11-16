
using DeepThinker.Interfaces;

namespace DeepThinker
{
    public class ThinkService : IThinkService
    {
        public async Task<string> DeepThinkingAsync(string url, int sslport = 7052)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"https://localhost:{sslport}/api/");
                var result = await client.GetStringAsync("values").ConfigureAwait(true);
                return result;
            }
        }

    }
}
