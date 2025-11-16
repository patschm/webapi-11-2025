using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductReviews.Client.Wpf.Utils
{
    public class AzureAd
    {
            public string? Instance  { get; set; }
            public string? Domain { get; set; }
            public string? TenantId  { get; set; }
            public string? ClientId  { get; set; }
            public string? RedirectUri  { get; set; }
            public string Authority
            {
                get
                {
                    return $"https://login.microsoftonline.com/{TenantId}/v2.0";
                }
            }
    }
}