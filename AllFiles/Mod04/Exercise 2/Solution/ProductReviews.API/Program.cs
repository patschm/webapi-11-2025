using Microsoft.EntityFrameworkCore;
using ProductReviews.API.Middleware;
using ProductReviews.DAL.EntityFramework.Database;
using ProductReviews.Interfaces;
using ProductReviews.Repositories.EntityFramework;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options=>{
    options.AddPolicy("Allow_All", builder=>{
        builder.AllowAnyOrigin();
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
    });
});
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, subscribeToJwtBearerMiddlewareDiagnosticsEvents:true);

builder.Services.AddDbContext<ProductReviewsContext>(opt=>{
    var conStr = SqlConnectionBuilder.ExpressConnectionString();
    opt.UseSqlServer(conStr!);
});
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
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
app.UseExecutionTime();
app.UseHttpsRedirection();

app.UseCors("Allow_All");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
