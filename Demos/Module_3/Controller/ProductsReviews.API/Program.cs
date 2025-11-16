using ControllerWeb.DTO;
using Microsoft.EntityFrameworkCore;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.DAL.Repositories;

namespace ControllerWeb;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
        builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
        builder.Services.AddAutoMapper(cfg=>cfg.AddProfile<ProductsReviewsProfile>());
        
        builder.Services.AddControllers().AddXmlSerializerFormatters();
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
    }
}
