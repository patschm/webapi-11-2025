using AutoMapper;
using FluentValidation;
using MinimalWeb.Arguments;
using MinimalWeb.DTO;
using Products.DAL.Database;
using Products.DAL.Entities;

namespace MinimalWeb.Grouping;

public static class ProductgroupEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/productgroups", (IMapper mapper, ProductReviewsContext context, [AsParameters] ProductgroupParams request) =>
        {
            var query = context.ProductGroups.Skip((request.page - 1) * request.count).Take(request.count);
            return mapper.ProjectTo<ProductGroupDTO>(query);
        })
        .WithName("GetProductgroups")
        .WithOpenApi();

        app.MapGet("/productgroups/{id}", async (IMapper mapper, ProductReviewsContext context, int id) =>
        {
            var brand = await context.ProductGroups.FindAsync(id);
            if (brand == null) return Results.NotFound(new { Error = $"No productgroup with {id} exists" });
            var dto = mapper.Map<ProductGroupDTO>(brand);
            return Results.Ok(dto);
        })
        .WithName("GetProductgroup")
        .WithOpenApi();

        app.MapPost("/productgroups", async (IMapper mapper, ProductReviewsContext context, IValidator<ProductGroupDTO> validator, ProductGroupDTO group) =>
        {
            var valResult = validator.Validate(group);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbGroup = mapper.Map<ProductGroup>(group);
            context.ProductGroups.Add(dbGroup);
            int result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return Results.BadRequest();
            }
            group.Id = dbGroup.Id;
            return Results.CreatedAtRoute("GetProductgroup", new { id = group.Id }, group);
        })
        .WithName("PostProductgroup")
        .WithOpenApi();

        app.MapPut("/productgroups/{id}", async (IMapper mapper, ProductReviewsContext context, IValidator<ProductGroupDTO> validator, int id, ProductGroupDTO group) =>
        {
            var valResult = validator.Validate(group);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbGroup = await context.ProductGroups.FindAsync(id);
            if (dbGroup == null)
            {
                return Results.NotFound();
            }
            mapper.Map(group, dbGroup);
            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return Results.BadRequest();
            }
            return Results.Accepted();
        })
        .WithName("PutProductgroup")
        .WithOpenApi();

        app.MapDelete("/productgroups/{id}", async (IMapper mapper, ProductReviewsContext context, int id) =>
        {
            var dbGroup = await context.ProductGroups.FindAsync(id);
            if (dbGroup == null)
            {
                return Results.NotFound();
            }
            context.ProductGroups.Remove(dbGroup);
            var result = await context.SaveChangesAsync();
            if (result == 0)
            {
                return Results.BadRequest();
            }
            return Results.NoContent();
        })
        .WithName("DeleteProductgroup")
        .WithOpenApi();
    }
}
