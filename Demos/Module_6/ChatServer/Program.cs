using ChatServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddCors(options => options.AddDefaultPolicy(p =>
    p.AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()
     .SetIsOriginAllowed(_ => true)));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<IRCIIHub>("/hub");
app.Run();
