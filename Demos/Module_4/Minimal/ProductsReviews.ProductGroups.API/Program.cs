using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.EntityFramework;
using ProductsReviews.DAL.Interfaces;
using ProductsReviews.DAL.Repositories;
using ProductsReviews.ProductGroups.API.Arguments;
using ProductsReviews.ProductGroups.API.DTO;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductReviewsContext>(opts => opts.UseInMemoryDatabase("productsDb"));
builder.Services.AddTransient<IProductGroupRepository, ProductGroupRepository>();
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<ProductGroupsProfile>());
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

app.MapGet("/productgroups", async (IMapper mapper, IProductGroupRepository repo, [AsParameters]ProductGroupParams request) =>
{
    var query = await repo.GetAsync(request.page, request.count);
    return mapper.ProjectTo<ProductGroupDTO>(query.AsQueryable());
})
.WithName("GetProductGroups")
.WithOpenApi();

app.MapGet("/productgroups/{id}", async (IMapper mapper, IProductGroupRepository repo, int id) =>
{
    var productGroup = await repo.GetByIdAsync(id);
    if (productGroup == null) return Results.NotFound(new { Error = $"Not ProductGroup with {id} exists" });
    var dto = mapper.Map<ProductGroupDTO>(productGroup);
    return Results.Ok(dto);
})
.WithName("GetProductGroup")
.WithOpenApi();

app.MapPost("/productgroups", async (IMapper mapper, IProductGroupRepository repo, IValidator<ProductGroupDTO> validator, ProductGroupDTO productGroup) =>
{
    var valResult = validator.Validate(productGroup);
    if (!valResult.IsValid) return Results.ValidationProblem(valResult.ToDictionary());
    var dbProductGroup = mapper.Map<ProductGroup>(productGroup);
    dbProductGroup = await repo.AddAsync(dbProductGroup);
    productGroup = mapper.Map<ProductGroupDTO>(dbProductGroup);
    return Results.CreatedAtRoute("GetProductGroup", new {id = dbProductGroup.Id }, productGroup);
})
.WithName("PostProductGroup")
.WithOpenApi();

app.MapPut("/productgroups/{id}", async (IMapper mapper, IProductGroupRepository repo, IValidator<ProductGroupDTO> validator, int id, ProductGroupDTO productGroup) =>
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

app.MapDelete("/productgroups/{id}", async (IMapper mapper, IProductGroupRepository repo, int id) =>
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

app.Run();
