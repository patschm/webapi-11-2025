
using ControllerWeb.DTO;
using Microsoft.EntityFrameworkCore;
using Products.DAL.Database;

namespace ControllerWeb;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
        builder.Services.AddAutoMapper(cfg=>cfg.AddProfile<ProductsProfile>());

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors(opts => {
            opts.AddPolicy("cors", conf => {
                conf.AllowAnyHeader();
                conf.AllowAnyMethod();
                conf.AllowAnyOrigin();
                conf.AllowAnyMethod();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("cors");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
