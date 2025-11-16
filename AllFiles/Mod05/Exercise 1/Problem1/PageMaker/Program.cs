using PageMaker.Interfaces;
using PageMaker.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITableService, TableService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseRouting();
app.MapDefaultControllerRoute();

app.Run();
