using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsReviews.Products.API.Arguments;
using ProductsReviews.Products.API.DTO;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.DAL.Repositories;
using System.Reflection;
using ProductsReviews.Products.API.Routes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProductsProfile>());
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
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

app.MapGroup("/products").MapProductsApi();

app.Run();
