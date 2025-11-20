var builder = WebApplication.CreateBuilder();

//var ctrl = new HomeController();
builder.Services.AddControllers();

var app = builder.Build();

//app.MapGet("/", ctrl.Index);
app.MapControllers();

app.Run();