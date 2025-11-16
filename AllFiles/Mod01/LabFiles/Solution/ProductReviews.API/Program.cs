using Microsoft.EntityFrameworkCore;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.Interfaces;
using ProductReviews.Repositories.EntityFramework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductReviewsContext>(opt=>{
    opt.UseSqlServer(SqlConnectionBuilder.ExpressConnectionString("Mod1DB"));
});
builder.Services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
