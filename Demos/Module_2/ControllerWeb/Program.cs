
using ControllerWeb.Controllers;
using ControllerWeb.DTO;
using ControllerWeb.Filters;
using ControllerWeb.Formatters;
using ControllerWeb.Middleware;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Products.DAL.Database;
using System.Data.Common;

namespace ControllerWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddSingleton<DbConnection, SqliteConnection>(serviceProvider =>
            //{
            //    var connection = new SqliteConnection("Data Source=:memory:");
            //    connection.Open();
            //    return connection;
            //});

            //builder.Services.AddDbContext<ProductReviewsContext>((sp, opts)=>
            //{
            //    var connection = sp.GetRequiredService<DbConnection>();
            //    opts.UseSqlite(connection);
            //});

            builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
            builder.Services.AddAutoMapper(cfg=>cfg.AddProfile<ProductsProfile>());
            builder.Services.AddScoped<CustomFilterAttribute>();

           // builder.Services.AddControllers().AddXmlSerializerFormatters();
            builder.Services.AddControllers(options =>
            {
                options.OutputFormatters.Add(new CsvFormatter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.Map("/ping", (HttpContext context) => {
                context.Response.WriteAsync("PONG!");
            });
            app.UseWhen(ctx => ctx.Request.Query.ContainsKey("ping"), bld =>
            {
             
                bld.Run(async ctx =>
                {
                    await ctx.Response.WriteAsync("PONG!!!");
                });
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseExceptionHandler("error");
            }

            // Custom Middleware 1
            app.Use(async (context, next) => {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogInformation("Custom1 Incoming...");
                //context.Request.Headers
                await next(context);
                logger.LogInformation("Custom1 Outgoing...");
            });
            // Custom Middleware 2
            //app.UseMiddleware<Custom2>();
            // Or
            app.UseCustom2();


            app.UseHttpsRedirection();

            app.UseAuthorization();
            //app.UseAuthentication();

            // Route Tables
            // Conventional Routing. Not recommended. Most commonly used in MVC
            // app.MapControllerRoute("default", "{controller=Brand}/{action=GetBrands}/{id?}", defaults: new {id=1});
            
            // Attribute Routing. Recommended
            app.MapControllers();

            app.Run();
        }
    }
}
