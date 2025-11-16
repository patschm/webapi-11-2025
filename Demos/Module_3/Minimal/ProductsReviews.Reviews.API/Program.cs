using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.DAL.Repositories;
using ProductsReviews.Reviews.API;
using ProductsReviews.Reviews.API.DTO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ReviewsProfile>());
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

app.MapGet("/reviews", async (IMapper mapper, IReviewRepository repo, [AsParameters]ReviewParams request) =>
{
    var query = await repo.GetAsync(request.page, request.count);
    return mapper.ProjectTo<ReviewDTO>(query.AsQueryable());
})
.WithName("GetReviews")
.WithOpenApi();

app.MapGet("/reviews/{id}", async (IMapper mapper, IReviewRepository repo, int id) =>
{
    var review = await repo.GetByIdAsync(id);
    if (review == null) return Results.NotFound(new { Error = $"Not Review with {id} exists" });
    var dto = mapper.Map<ReviewDTO>(review);
    return Results.Ok(dto);
})
.WithName("GetReview")
.WithOpenApi();

app.MapPost("/reviews", async (IMapper mapper, IReviewRepository repo, IValidator<ReviewDTO> validator, ReviewDTO review) =>
{
    var valResult = validator.Validate(review);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbReview = mapper.Map<Review>(review);
    dbReview = await repo.AddAsync(dbReview);
    review = mapper.Map<ReviewDTO>(dbReview);
    return Results.CreatedAtRoute("GetReview", new {id = dbReview.Id }, review);
})
.WithName("PostReview")
.WithOpenApi();

app.MapPut("/reviews/{id}", async (IMapper mapper, IReviewRepository repo, IValidator<ReviewDTO> validator, int id, ReviewDTO review) =>
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

app.MapDelete("/reviews/{id}", async (IMapper mapper, IReviewRepository repo, int id) =>
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

app.Run();
