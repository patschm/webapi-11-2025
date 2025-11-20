using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "Aha");
builder.Services.AddAuthorization(opts => {
    opts.AddPolicy("ok", pol => {
        pol.RequireClaim("", "");
    });
});
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("overal", conf =>
    {
        conf.AllowAnyHeader();
        conf.AllowAnyMethod();
        conf.AllowAnyOrigin();
    });

});
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("overal");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
