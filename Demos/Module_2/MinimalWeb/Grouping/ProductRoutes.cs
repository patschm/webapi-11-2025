using AutoMapper;
using FluentValidation;
using MinimalWeb.Arguments;
using MinimalWeb.DTO;
using Products.DAL.Database;
using Products.DAL.Entities;

namespace MinimalWeb.Grouping;

public static class ProductRoutes
{
    public static RouteGroupBuilder MapProductApi(this RouteGroupBuilder group)
    {
        group.MapGet("/", (IMapper mapper, ProductReviewsContext context, [AsParameters] ProductParams request) =>
        {
            var query = context.Products.Skip((request.page - 1) * request.count).Take(request.count);
            return mapper.ProjectTo<ProductDTO>(query);
        })
        .WithName("GetProducts")
        .WithOpenApi();

        group.MapGet("/{id}", async (IMapper mapper, ProductReviewsContext context, int id) =>
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return Results.NotFound(new { Error = $"No product with {id} exists" });
            var dto = mapper.Map<ProductDTO>(product);
            return Results.Ok(dto);
        })
        .WithName("GetProduct")
        .WithOpenApi();

        group.MapPost("/", async (IMapper mapper, ProductReviewsContext context, IValidator<ProductDTO> validator, ProductDTO product) =>
        {
            var valResult = validator.Validate(product);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbProduct = mapper.Map<Product>(product);
            context.Products.Add(dbProduct);
            int result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return Results.BadRequest();
            }
            product.Id = dbProduct.Id;
            return Results.CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        })
        .WithName("PostProduct")
        .WithOpenApi();

        group.MapPut("/{id}", async (IMapper mapper, ProductReviewsContext context, IValidator<ProductDTO> validator, int id, ProductDTO product) =>
        {
            var valResult = validator.Validate(product);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbProduct = await context.Products.FindAsync(id);
            if (dbProduct == null)
            {
                return Results.NotFound();
            }
            mapper.Map(product, dbProduct);
            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return Results.BadRequest();
            }
            return Results.Accepted();
        })
        .WithName("PutProduct")
        .WithOpenApi();

        group.MapDelete("/{id}", async (IMapper mapper, ProductReviewsContext context, int id) =>
        {
            var dbProduct = await context.Products.FindAsync(id);
            if (dbProduct == null)
            {
                return Results.NotFound();
            }
            context.Products.Remove(dbProduct);
            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return Results.BadRequest();
            }
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .WithOpenApi();

        return group;
    }
}
