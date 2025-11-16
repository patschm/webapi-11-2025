using Microsoft.EntityFrameworkCore;
using ProductReviews.API.Middleware;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.Interfaces;
using ProductReviews.Repositories.EntityFramework;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ProductReviewsContext>(opt=>{
    //var conStr = Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE");
    var conStr = SqlConnectionBuilder.ExpressConnectionString();
    opt.UseSqlServer(conStr!);
});
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

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
app.UseExecutionTime();
app.UseHttpsRedirection();


app.MapControllers();

app.Run();
