using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddKeyedSingleton<ICounter, Counter>(1);
builder.Services.AddKeyedSingleton<ICounter, MisCounter>(2);

var app = builder.Build();


    app.MapGet("/counter", ([FromKeyedServices(1)]ICounter counter) =>
    {
        // var counter = new Counter();
        counter.Increment();
        //counter.Increment();

        return counter.Value;
    });

using (var scope = app.Services.CreateScope())
{ 
    var cnt = scope.ServiceProvider.GetRequiredKeyedService<ICounter>(1);
    cnt.Increment();
    cnt.Increment();
    Console.WriteLine(  cnt.Value);
}

using (var scope = app.Services.CreateScope())
{
    var cnt = scope.ServiceProvider.GetRequiredKeyedService<ICounter>(2);
    cnt.Increment();
    cnt.Increment();
    Console.WriteLine(cnt.Value);
}

app.Run();
