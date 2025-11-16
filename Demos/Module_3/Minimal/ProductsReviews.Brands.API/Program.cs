using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsReviews.Brands.API.Arguments;
using ProductsReviews.Brands.API.DTO;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.DAL.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<BrandsProfile>());
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

app.MapGet("/brands", async (IMapper mapper, IBrandRepository repo, [AsParameters]BrandParams request) =>
{
    var query = await repo.GetAsync(request.page, request.count);
    return mapper.ProjectTo<BrandDTO>(query.AsQueryable());
})
.WithName("GetBrands")
.WithOpenApi();

app.MapGet("/brands/{id}", async (IMapper mapper, IBrandRepository repo, int id) =>
{
    var brand = await repo.GetByIdAsync(id);
    if (brand == null) return Results.NotFound(new { Error = $"Not brand with {id} exists" });
    var dto = mapper.Map<BrandDTO>(brand);
    return Results.Ok(dto);
})
.WithName("GetBrand")
.WithOpenApi();

app.MapPost("/brands", async (IMapper mapper, IBrandRepository repo, IValidator<BrandDTO> validator, BrandDTO brand) =>
{
    var valResult = validator.Validate(brand);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbBrand = mapper.Map<Brand>(brand);
    dbBrand = await repo.AddAsync(dbBrand);
    brand = mapper.Map<BrandDTO>(dbBrand);
    return Results.CreatedAtRoute("GetBrand", new {id = dbBrand.Id }, brand);
})
.WithName("PostBrand")
.WithOpenApi();

app.MapPut("/brands/{id}", async (IMapper mapper, IBrandRepository repo, IValidator<BrandDTO> validator, int id, BrandDTO brand) =>
{
    var valResult = validator.Validate(brand);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbBrand = await repo.GetByIdAsync(id);
    if (dbBrand == null)
    {
        return Results.NotFound();
    }
    mapper.Map(brand, dbBrand);
    await repo.UpdateAsync(dbBrand);
    return Results.Accepted();
})
.WithName("PutBrand")
.WithOpenApi();

app.MapDelete("/brands/{id}", async (IMapper mapper, IBrandRepository repo, int id) =>
{
    var dbBrand = await repo.GetByIdAsync(id);
    if (dbBrand == null)
    {
        return Results.NotFound();
    }
    await repo.DeleteAsync(id);
    return Results.NoContent();
})
.WithName("DeleteBrand")
.WithOpenApi();

app.Run();
