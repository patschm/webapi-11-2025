using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace ProductReviews.Client.Wpf.Utils
{
    public class AzureAdClient
    {
        private readonly AzureAd _aad;
        private IPublicClientApplication? _app;
        private IAccount? _account;

        private void Initialize()
        {
            if (_app == null)
            {
                _app = PublicClientApplicationBuilder
                                .Create(_aad.ClientId)
                                .WithAuthority(_aad.Authority)
                                .WithRedirectUri(_aad.RedirectUri)
                                .Build();
            }
        }
        public async Task<string> GetAccessTokenForAsync(string[] scopes)
        {
            Initialize();
            var accounts = await _app!.GetAccountsAsync();
            _account = accounts.FirstOrDefault();
            AuthenticationResult  token;
            try
            {
                token = await _app.AcquireTokenSilent(scopes, _account).ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                token = await _app.AcquireTokenInteractive(scopes).ExecuteAsync();
            }
            return token.AccessToken;
        }
       

        public AzureAdClient(IOptions<AzureAd> aad)
        {
            _aad = aad.Value;
        }
    }
}