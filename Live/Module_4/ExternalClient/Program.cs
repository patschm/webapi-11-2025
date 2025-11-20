using System.Net.Http.Headers;
using Microsoft.Identity.Client;

internal class Program
{
    private static string ServiceUrl = "https://localhost:7209/";
    private static async Task Main(string[] args)
    {
        await DoTheCodeFlowAsync();
        //await DoTheCredentialFlowAsync();
        
        Console.ReadLine();
    }

    private static async Task DoTheCodeFlowAsync()
    {
        // To make this work do the following:
        // 1) Create an application registration for platform Mobile and Desktop Application.
        //    This prepares Code Grant Flow
        // 2) Set Redirect Uri to http://localhost (must be http. Port is optional)
        var bld = PublicClientApplicationBuilder
            .Create("15c193fe-d2ee-4ec5-96a0-e788ed2fee41")
            .WithAuthority(AzureCloudInstance.AzurePublic, "030b09d5-7f0f-40b0-8c01-03ac319b2d71")
            .WithRedirectUri("http://localhost:9898/");  // http scheme only!

        var app = bld.Build();
        // .AcquireTokenByUsernamePassword
        var token = await app.AcquireTokenInteractive([ "api://f0c5f3ae-582c-4cfb-956c-ffb826836af4/Access" ])
            .ExecuteAsync();

        Console.WriteLine(token.AccessToken);
        Console.WriteLine(new string('=', 80));
        Console.WriteLine(token.IdToken);

        Console.WriteLine(new string('=', 80));
        var client = new HttpClient();
        client.BaseAddress = new Uri(ServiceUrl);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.AccessToken);

        string data = await client.GetStringAsync("weatherforecast");
        Console.WriteLine(data);
    }
    private static async Task DoTheCredentialFlowAsync()
    {
        // To make this work do the following:
        // 1) On the application registration of the webapi define an App Role
        //    for Application. 
        // 2) Create a new Application Registration for the servie app.
        //    a) Certificates & secrets: Generate a new Client Secret
        //    b) API permissions: Add Permission -> My Apis -> Select your webapi registration.
        //    c) Select Application Permissions (if disabled you forgot or wrongly did step 1)
        //    d) Select the roles you defined in webapi registration
        //    e) Grant Admin consent on the newly created permission.
        var bld = ConfidentialClientApplicationBuilder
            .Create("c2177b60-ef45-4bd8-9dea-6152bbe1b84a")
            .WithTenantId("030b09d5-7f0f-40b0-8c01-03ac319b2d71")
            .WithClientSecret("knzcQX");

        var app = bld.Build();
        var token = await app
            .AcquireTokenForClient(
                new string[]{ "api://e96ce23c-91ff-407d-92e2-4aefb321d62e/.default" }) // Api ID Uri from webapi regstration. Add /.default to it
            .ExecuteAsync();
        Console.WriteLine(token.AccessToken);
       
        //app.Build().AcquireTokenSilent();
        
        var client = new HttpClient();
        client.BaseAddress = new Uri(ServiceUrl);

        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token.AccessToken);

        string data = await client.GetStringAsync("weatherforecast");
        Console.WriteLine(data);
    }
}