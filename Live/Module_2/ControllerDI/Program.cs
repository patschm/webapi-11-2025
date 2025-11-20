using Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<Counter, Counter>();
//builder.Services.AddScoped<Counter, Counter>();
builder.Services.AddSingleton<MisCounter, MisCounter>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
