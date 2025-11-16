using DeepThinker;
using DeepThinker.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IThinkService, ThinkService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
