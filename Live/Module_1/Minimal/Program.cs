
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;

var app = WebApplication.Create();
app.Urls.Add("https://localhost:7000/");
app.Urls.Add("https://localhost:7500/");

var inst = new MyClass();

//SetRoutes(app);


// QueryString benadering
app.MapGet("ddd", (int? x = null)=>{
    if (!x.HasValue)
    {
        
    }
    return $"Domain-Driven Design {x ?? 33}";
    });
app.MapGet("eee/{x:required}", (int? x = null, int? y = null)=>{
    if (!x.HasValue)
    {
        
    }
    return $"Event-Driven Design {x ?? 33} {y ?? 11}";
    });
app.MapPost("ppp",  (HttpContext ctx, Person p) =>
{
   
    System.Console.WriteLine($"{p.Name}");
    p.ID = 300;
    return Results.Created<Person>($"insert/{p.ID}", p);

});




app.MapGroup("/home").MapHomeApi();




app.Run();


void SetRoutes(WebApplication app)
{
    app.MapGet("/", () => "Dit is de index");

    app.MapGet("aaa", inst.DoeOokWat).Produces(StatusCodes.Status200OK);

}

string DoeIets()
{
    return "Doet iets";
}
