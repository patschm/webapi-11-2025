
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsReviews.Identity.Database;

namespace ProductsReviews.Identity
{
    public class Program
    {
        // dotnet ef migrations add InitialCreate
        // dotnet ef database update
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ProductReviewsContext>(opts => {
                opts.UseSqlite("Data Source=aspnetauth.db");
            });
            builder.Services.AddIdentityApiEndpoints<IdentityUser>(opts => {
                opts.Password.RequiredLength = 5;
                opts.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ProductReviewsContext>();

            builder.Services.AddAuthorization();

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
            app.MapIdentityApi<IdentityUser>();

            app.Run();
        }
    }
}
