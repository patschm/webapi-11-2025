// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using System.Text.Json;
using ProductReviews.DAL.EntityFramework.Entities;

var serOpts = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

await ReadProductGroupAsync();
await AddProductGroupAsync();
await UpdateProductGroupAsync();

Console.WriteLine("Done");
Console.ReadLine();


async Task ReadProductGroupAsync()
{
    var client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7298/");
    var response = await client.GetAsync($"productgroup?page=1&count=10");
    if (response.IsSuccessStatusCode)
    {
        var stringdata = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<List<ProductGroup>>(stringdata, serOpts);
        ShowData(data);
    }
}

async Task AddProductGroupAsync()
{
   var client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7298/");

    var pg = new ProductGroup { Name = "Test Group"};
    var  stringdata = JsonSerializer.Serialize(pg);
    var content = new StringContent(stringdata);
    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    var result = await client.PostAsync("productgroup", content);
    if(result.IsSuccessStatusCode)
    {
        stringdata = await result.Content.ReadAsStringAsync();
        var pgn = JsonSerializer.Deserialize<ProductGroup>(stringdata, serOpts);
        Console.WriteLine($"[{pgn?.Id}] {pgn?.Name}");
    }
}


async Task UpdateProductGroupAsync()
{
    var client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7298/");
 
    var response = await client.GetAsync($"productgroup/1");
    if (response.IsSuccessStatusCode)
    {
        var stringdata = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<ProductGroup>(stringdata, serOpts);
        data!.Name = "Updated group";

        stringdata = JsonSerializer.Serialize(data);
        var content = new StringContent(stringdata);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        response = await client.PutAsync("productgroup/1", content);
        if (response.IsSuccessStatusCode)
        {
            await ReadProductGroupAsync();
        }

    }

}

void ShowData(List<ProductGroup>? data)
{
    foreach(var pg in data!)
    {
        Console.WriteLine($"{pg.Name}");
    }
}