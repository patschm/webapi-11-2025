using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using Proxies;
using System.Net.Http.Json;

namespace ConsoleClient;

internal class Program
{
    private const string baseAddress = "https://localhost:7297";

    static async Task Main(string[] args)
    {
        //await SimpleAsync();
        //await AdvancedAsync();

        //await GenerateProxiesAsync(baseAddress);
        await NSwagStudioAsync();
        Console.ReadLine();
    }

  

    private static async Task SimpleAsync()
    {
        var client = new HttpClient();
        client.BaseAddress = new Uri(baseAddress);

        await DoGetAllAsync(client);
        var id = await DoPostAsync(client);
        await DoGetAsync(client, id);
        await DoUpdateAsync(client, id);
        await DoGetAsync(client, id);
        await DoDeleteAsync(client, id);
    }

    private static async Task AdvancedAsync()
    {
        var factory = new DefaultServiceProviderFactory();
        var services = new ServiceCollection();
        var builder = factory.CreateBuilder(services);
        builder.AddHttpClient("brands", conf =>
        {
            conf.BaseAddress = new Uri(baseAddress);
        });
        var provider = builder.BuildServiceProvider();
        var cfact = provider.GetRequiredService<IHttpClientFactory>();
        var client = cfact.CreateClient("brands");

        await DoGetAllAsync(client);
        var id = await DoPostAsync(client);
        await DoGetAsync(client, id);
        await DoUpdateAsync(client, id);
        await DoGetAsync(client, id);
        await DoDeleteAsync(client, id);
    }

    private static async Task NSwagStudioAsync()
    {
        // Make sure ProxyClient.cs is generated.
        // If not, call GenerateProxiesAsync method first.
        var http = new HttpClient();
        var client = new Client(baseAddress, http);
        var data = await client.BrandsAllAsync(1, 10);
        foreach (var item in data)
        {
            Console.WriteLine($"[{item.Id}] {item.Name}");
        }
    }

    private static async Task DoDeleteAsync(HttpClient client, int id)
    {
        Console.WriteLine("Delete Brand");
        var response = await client.DeleteAsync($"brands/{id}");
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode);
            var resp2 = await client.GetAsync($"brands/{id}");
            Console.WriteLine(resp2.StatusCode);
        }
        Console.WriteLine("===========================================");
    }

    private static async Task DoGetAsync(HttpClient client, int id)
    {
        Console.WriteLine("Get Brand");
        var result = await client.GetAsync($"brands/{id}");
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine($"Content-Type: {result.Content.Headers.ContentType}");
            var data = await result.Content.ReadFromJsonAsync<Brand>();
            Console.WriteLine($"[{data?.Id}] {data?.Name}");
        }
        Console.WriteLine("===========================================");
    }

    private static async Task DoUpdateAsync(HttpClient client, int id)
    {
        Console.WriteLine("Update Brand");
        var brand = new Brand { Name = "Test 2" };
        var content = JsonContent.Create(brand);
        var response = await client.PutAsync($"brands/{id}", content);
        if (response.IsSuccessStatusCode)
        {
           Console.WriteLine(response.StatusCode);
        }
        Console.WriteLine("===========================================");
    }

    private static async Task<int> DoPostAsync(HttpClient client)
    {
        Console.WriteLine("Insert Brand");
        var brand = new Brand { Name = "Test 1" };
        var content = JsonContent.Create(brand);
        var response = await client.PostAsync("brands",  content);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Location: {response.Headers.Location}");
            var result = await response.Content.ReadFromJsonAsync<Brand>();
            Console.WriteLine($"[{result?.Id}] {result?.Name}");
            Console.WriteLine("===========================================");
            return result!.Id;
        }
        return 0;
    }
    private static async Task DoGetAllAsync(HttpClient client)
    {
        Console.WriteLine("Get Brands");
        var result = await client.GetAsync("brands");
        if (result.IsSuccessStatusCode)
        {
            Console.WriteLine($"Content-Type: {result.Content.Headers.ContentType}");
            var data = await result.Content.ReadFromJsonAsync<Brand[]>();
            foreach(var item in data!)
            {
                Console.WriteLine($"[{item.Id}] {item.Name}");
            }         
        }
        Console.WriteLine("===========================================");
    }

    private static async Task GenerateProxiesAsync(string specUrl)
    {
        // NuGet: NSwag.CodeGeneration.CSharp
        var http = new HttpClient { BaseAddress = new Uri(specUrl + "/swagger/v1/") };
        var data = await http.GetStringAsync("swagger.json");
        var document = await OpenApiDocument.FromJsonAsync(data);

        var settings = new CSharpClientGeneratorSettings
        {
            ClassName = "Client",
            CSharpGeneratorSettings = {
                Namespace = "Proxies"
            }
        };

        var generator = new CSharpClientGenerator(document, settings);
        var code = generator.GenerateFile();
        await File.WriteAllTextAsync("../../../ProxyClient.cs", code);
        Console.WriteLine("Proxies generated");
    }
}
