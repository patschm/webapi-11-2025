// See https://aka.ms/new-console-template for more information

using System.Net.Http.Headers;
using Newtonsoft.Json;
using ProductReviews.DAL.EntityFramework.Entities;

await ReadProductGroupAsync();
await AddProductGroupAsync();
await UpdateProductGroupAsync();

Console.WriteLine("Done");
Console.ReadLine();


async Task ReadProductGroupAsync()
{
    var client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7016/api/");
    var response = await client.GetAsync($"productgroup?page=1&count=10");
    if (response.IsSuccessStatusCode)
    {
        var stringdata = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<List<ProductGroup>>(stringdata);
        ShowData(data);
    }
}

async Task AddProductGroupAsync()
{
   var client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7016/api/");

    var pg = new ProductGroup { Name = "Test Group"};
    var  stringdata = JsonConvert.SerializeObject(pg);
    var content = new StringContent(stringdata);
    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    var result = await client.PostAsync("productgroup", content);
    if(result.IsSuccessStatusCode)
    {
        stringdata = await result.Content.ReadAsStringAsync();
        var pgn = JsonConvert.DeserializeObject<ProductGroup>(stringdata);
        Console.WriteLine($"[{pgn?.Id}] {pgn?.Name}");
    }
}


async Task UpdateProductGroupAsync()
{
    var client = new HttpClient();
    client.BaseAddress = new Uri("https://localhost:7016/api/");
 
    var response = await client.GetAsync($"productgroup/1");
    if (response.IsSuccessStatusCode)
    {
        var stringdata = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<ProductGroup>(stringdata);
        data!.Name = "Updated group";

        stringdata = JsonConvert.SerializeObject(data);
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