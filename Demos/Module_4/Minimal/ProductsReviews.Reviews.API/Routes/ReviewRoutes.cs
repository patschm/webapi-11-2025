using AutoMapper;
using FluentValidation;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.Reviews.API.DTO;

namespace ProductsReviews.Reviews.API.Routes;

public static class ReviewRoutes
{
    public static RouteGroupBuilder MapReviewsApi(this RouteGroupBuilder routes)
    {
        routes.MapGet("/", async (IMapper mapper, IReviewRepository repo, [AsParameters] ReviewParams request) =>
        {
            var query = await repo.GetAsync(request.page, request.count);
            return mapper.ProjectTo<ReviewDTO>(query.AsQueryable());
        })
        .WithName("GetReviews")
        .WithOpenApi();

        routes.MapGet("/{id}", async (IMapper mapper, IReviewRepository repo, int id) =>
        {
            var review = await repo.GetByIdAsync(id);
            if (review == null) return Results.NotFound(new { Error = $"Not Review with {id} exists" });
            var dto = mapper.Map<ReviewDTO>(review);
            return Results.Ok(dto);
        })
        .WithName("GetReview")
        .WithOpenApi();

        routes.MapPost("/", async (IMapper mapper, IReviewRepository repo, IValidator<ReviewDTO> validator, ReviewDTO review) =>
        {
            var valResult = validator.Validate(review);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbReview = mapper.Map<Review>(review);
            dbReview = await repo.AddAsync(dbReview);
            review = mapper.Map<ReviewDTO>(dbReview);
            return Results.CreatedAtRoute("GetReview", new { id = dbReview.Id }, review);
        })
        .WithName("PostReview")
        .WithOpenApi();

        routes.MapPut("/{id}", async (IMapper mapper, IReviewRepository repo, IValidator<ReviewDTO> validator, int id, ReviewDTO review) =>
        {
            var valResult = validator.Validate(review);
            if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
            var dbReview = await repo.GetByIdAsync(id);
            if (dbReview == null)
            {
                return Results.NotFound();
            }
            mapper.Map(review, dbReview);
            await repo.UpdateAsync(dbReview);
            return Results.Accepted();
        })
        .WithName("PutReview")
        .WithOpenApi();

        routes.MapDelete("/{id}", async (IMapper mapper, IReviewRepository repo, int id) =>
        {
            var dbReview = await repo.GetByIdAsync(id);
            if (dbReview == null)
            {
                return Results.NotFound();
            }
            await repo.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteReview")
        .WithOpenApi();


        return routes;
    }
}
