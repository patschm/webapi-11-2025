using AutoMapper;
using FluentValidation;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.Products.API.Arguments;
using ProductsReviews.Products.API.DTO;

namespace ProductsReviews.Products.API.Routes;

public static class ProductRoutes
{
    public static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder routes)
    {
        routes.MapGet("/", async (IMapper mapper, IProductRepository repo, [AsParameters] ProductParams request) =>
        {
            var query = await repo.GetAsync(request.page, request.count);
            return mapper.ProjectTo<ProductDTO>(query.AsQueryable());
        })
        .WithName("GetProducts")
        .WithOpenApi();

        routes.MapGet("/{id}", async (IMapper mapper, IProductRepository repo, int id) =>
        {
            var product = await repo.GetByIdAsync(id);
            if (product == null) return Results.NotFound(new { Error = $"Not Product with {id} exists" });
            var dto = mapper.Map<ProductDTO>(product);
            return Results.Ok(dto);
        })
        .WithName("GetProduct")
        .WithOpenApi();

        routes.MapPost("/", async (IMapper mapper, IProductRepository repo, IValidator<ProductDTO> validator, ProductDTO product) =>
        {
            var valResult = validator.Validate(product);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbProduct = mapper.Map<Product>(product);
            dbProduct = await repo.AddAsync(dbProduct);
            product = mapper.Map<ProductDTO>(dbProduct);
            return Results.CreatedAtRoute("GetProduct", new { id = dbProduct.Id }, product);
        })
        .WithName("PostProduct")
        .WithOpenApi();

        routes.MapPut("/{id}", async (IMapper mapper, IProductRepository repo, IValidator<ProductDTO> validator, int id, ProductDTO product) =>
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

        routes.MapDelete("/{id}", async (IMapper mapper, IProductRepository repo, int id) =>
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

        return routes;
    }
}
