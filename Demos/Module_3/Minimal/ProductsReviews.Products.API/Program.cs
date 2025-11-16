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

app.MapGet("/products", async (IMapper mapper, IProductRepository repo, [AsParameters]ProductParams request) =>
{
    var query = await repo.GetAsync(request.page, request.count);
    return mapper.ProjectTo<ProductDTO>(query.AsQueryable());
})
.WithName("GetProducts")
.WithOpenApi();

app.MapGet("/products/{id}", async (IMapper mapper, IProductRepository repo, int id) =>
{
    var product = await repo.GetByIdAsync(id);
    if (product == null) return Results.NotFound(new { Error = $"Not Product with {id} exists" });
    var dto = mapper.Map<ProductDTO>(product);
    return Results.Ok(dto);
})
.WithName("GetProduct")
.WithOpenApi();

app.MapPost("/products", async (IMapper mapper, IProductRepository repo, IValidator<ProductDTO> validator, ProductDTO product) =>
{
    var valResult = validator.Validate(product);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbProduct = mapper.Map<Product>(product);
    dbProduct = await repo.AddAsync(dbProduct);
    product = mapper.Map<ProductDTO>(dbProduct);
    return Results.CreatedAtRoute("GetProduct", new {id = dbProduct.Id }, product);
})
.WithName("PostProduct")
.WithOpenApi();

app.MapPut("/products/{id}", async (IMapper mapper, IProductRepository repo, IValidator<ProductDTO> validator, int id, ProductDTO product) =>
{
    var valResult = validator.Validate(product);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbProduct = await repo.GetByIdAsync(id);
    if (dbProduct == null)
    {
        return Results.NotFound();
    }
    mapper.Map(product, dbProduct);
    await repo.UpdateAsync(dbProduct);
    return Results.Accepted();
})
.WithName("PutProduct")
.WithOpenApi();

app.MapDelete("/products/{id}", async (IMapper mapper, IProductRepository repo, int id) =>
{
    var dbProduct = await repo.GetByIdAsync(id);
    if (dbProduct == null)
    {
        return Results.NotFound();
    }
    await repo.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteProduct")
.WithOpenApi();

app.Run();
