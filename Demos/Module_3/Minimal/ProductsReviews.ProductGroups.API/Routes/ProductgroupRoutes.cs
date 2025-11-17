using AutoMapper;
using FluentValidation;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.ProductGroups.API.Arguments;
using ProductsReviews.ProductGroups.API.DTO;

namespace ProductsReviews.ProductGroups.API.Routes;

public static class ProductgroupRoutes
{
    public static RouteGroupBuilder MapProductgroupsApi(this RouteGroupBuilder routes)
    {
        routes.MapGet("/", async (IMapper mapper, IProductGroupRepository repo, [AsParameters] ProductGroupParams request) =>
        {
            var query = await repo.GetAsync(request.page, request.count);
            return mapper.ProjectTo<ProductGroupDTO>(query.AsQueryable());
        })
        .WithName("GetProductGroups")
        .WithOpenApi();

        routes.MapGet("/{id}", async (IMapper mapper, IProductGroupRepository repo, int id) =>
        {
            var productGroup = await repo.GetByIdAsync(id);
            if (productGroup == null) return Results.NotFound(new { Error = $"Not ProductGroup with {id} exists" });
            var dto = mapper.Map<ProductGroupDTO>(productGroup);
            return Results.Ok(dto);
        })
        .WithName("GetProductGroup")
        .WithOpenApi();

        routes.MapPost("/", async (IMapper mapper, IProductGroupRepository repo, IValidator<ProductGroupDTO> validator, ProductGroupDTO productGroup) =>
        {
            var valResult = validator.Validate(productGroup);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbProductGroup = mapper.Map<ProductGroup>(productGroup);
            dbProductGroup = await repo.AddAsync(dbProductGroup);
            productGroup = mapper.Map<ProductGroupDTO>(dbProductGroup);
            return Results.CreatedAtRoute("GetProductGroup", new { id = dbProductGroup.Id }, productGroup);
        })
        .WithName("PostProductGroup")
        .WithOpenApi();

        routes.MapPut("/{id}", async (IMapper mapper, IProductGroupRepository repo, IValidator<ProductGroupDTO> validator, int id, ProductGroupDTO productGroup) =>
        {
            var valResult = validator.Validate(productGroup);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbProductGroup = await repo.GetByIdAsync(id);
            if (dbProductGroup == null)
            {
                return Results.NotFound();
            }
            mapper.Map(productGroup, dbProductGroup);
            await repo.UpdateAsync(dbProductGroup);
            return Results.Accepted();
        })
        .WithName("PutProductGroup")
        .WithOpenApi();

        routes.MapDelete("/{id}", async (IMapper mapper, IProductGroupRepository repo, int id) =>
        {
            var dbProductGroup = await repo.GetByIdAsync(id);
            if (dbProductGroup == null)
            {
                return Results.NotFound();
            }
            await repo.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteProductGroup")
        .WithOpenApi();

        return routes;
    }
}
