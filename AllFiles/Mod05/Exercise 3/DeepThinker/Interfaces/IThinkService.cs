namespace DeepThinker.Interfaces
{
    public interface IThinkService
    {
        Task<string> DeepThinkingAsync(string url, int sslport=7052);
    }
}
