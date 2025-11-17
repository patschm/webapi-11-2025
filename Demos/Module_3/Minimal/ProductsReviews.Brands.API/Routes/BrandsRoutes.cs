using AutoMapper;
using FluentValidation;
using ProductsReviews.Brands.API.Arguments;
using ProductsReviews.Brands.API.DTO;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ProductsReviews.Brands.API.Routes;

public static class BrandsRoutes
{
    public static RouteGroupBuilder MapBrandsApi(this RouteGroupBuilder routes)
    {
        routes.MapGet("/", async (IMapper mapper, IBrandRepository repo, [AsParameters] BrandParams request) =>
        {
            var query = await repo.GetAsync(request.page, request.count);
            return mapper.ProjectTo<BrandDTO>(query.AsQueryable());
        })
        .WithName("GetBrands")
        .WithOpenApi();

        routes.MapGet("/{id}", async (IMapper mapper, IBrandRepository repo, int id) =>
        {
            var brand = await repo.GetByIdAsync(id);
            if (brand == null) return Results.NotFound(new { Error = $"Not brand with {id} exists" });
            var dto = mapper.Map<BrandDTO>(brand);
            return Results.Ok(dto);
        })
        .WithName("GetBrand")
        .WithOpenApi();

        routes.MapPost("/", async (IMapper mapper, IBrandRepository repo, IValidator<BrandDTO> validator, BrandDTO brand) =>
        {
            var valResult = validator.Validate(brand);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbBrand = mapper.Map<Brand>(brand);
            dbBrand = await repo.AddAsync(dbBrand);
            brand = mapper.Map<BrandDTO>(dbBrand);
            return Results.CreatedAtRoute("GetBrand", new { id = dbBrand.Id }, brand);
        })
        .WithName("PostBrand")
        .WithOpenApi();

        routes.MapPut("/{id}", async (IMapper mapper, IBrandRepository repo, IValidator<BrandDTO> validator, int id, BrandDTO brand) =>
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

        routes.MapDelete("/{id}", async (IMapper mapper, IBrandRepository repo, int id) =>
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
        return routes;
    }
}
