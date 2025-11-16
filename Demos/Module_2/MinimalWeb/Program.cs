using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MinimalWeb.Arguments;
using MinimalWeb.DTO;
using Products.DAL.Database;
using Products.DAL.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
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

app.MapGet("/brands", (IMapper mapper, ProductReviewsContext context, [AsParameters]BrandParams request) =>
{
    var query = context.Brands.Skip((request.page - 1) * request.count).Take(request.count);
    return mapper.ProjectTo<BrandDTO>(query);
})
.WithName("GetBrands")
.WithOpenApi();

app.MapGet("/brands/{id}", async (IMapper mapper, ProductReviewsContext context, int id) =>
{
    var brand = await context.Brands.FindAsync(id);
    if (brand == null) return Results.NotFound(new { Error = $"Not brand with {id} exists" });
    var dto = mapper.Map<BrandDTO>(brand);
    return Results.Ok(dto);
})
.WithName("GetBrand")
.WithOpenApi();

app.MapPost("/brands", async (IMapper mapper, ProductReviewsContext context, IValidator<BrandDTO> validator, BrandDTO brand) =>
{
    var valResult = validator.Validate(brand);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbBrand = mapper.Map<Brand>(brand);
    context.Brands.Add(dbBrand);
    int result = await context.SaveChangesAsync();
    if (result == 0)
    {
        return Results.BadRequest();
    }
    brand.Id = dbBrand.Id;
    return Results.CreatedAtRoute("GetBrand", new {id = dbBrand.Id }, brand);
})
.WithName("PostBrand")
.WithOpenApi();

app.MapPut("/brands/{id}", async (IMapper mapper, ProductReviewsContext context, IValidator<BrandDTO> validator, int id, BrandDTO brand) =>
{
    var valResult = validator.Validate(brand);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbBrand = await context.Brands.FindAsync(id);
    if (dbBrand == null)
    {
        return Results.NotFound();
    }
    mapper.Map(brand, dbBrand);
    var result = await context.SaveChangesAsync();
    if (result == 0)
    {
        return Results.BadRequest();
    }
    return Results.Accepted();
})
.WithName("PutBrand")
.WithOpenApi();

app.MapDelete("/brands/{id}", async (IMapper mapper, ProductReviewsContext context, int id) =>
{
    var dbBrand = await context.Brands.FindAsync(id);
    if (dbBrand == null)
    {
        return Results.NotFound();
    }
    context.Brands.Remove(dbBrand);
    var result = await context.SaveChangesAsync();
    if (result == 0)
    {
        return Results.BadRequest();
    }
    return Results.NoContent();
})
.WithName("DeleteBrand")
.WithOpenApi();

app.Run();
