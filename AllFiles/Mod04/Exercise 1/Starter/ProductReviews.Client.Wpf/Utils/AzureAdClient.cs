using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace ProductReviews.Client.Wpf.Utils;

public class AzureAdClient
{
    private readonly AzureAd _aad;
    private IPublicClientApplication? _app;
    private IAccount? _account;

    private void Initialize()
    {
       
    }
    public async Task<string> GetAccessTokenForAsync(string[] scopes)
    {
        Initialize();
      
        return "";
    }
   

    public AzureAdClient(IOptions<AzureAd> aad)
    {
        _aad = aad.Value;
    }
}