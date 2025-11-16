// See https://aka.ms/new-console-template for more information
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProductReviews.DAL.EntityFramework.Database;

var options = new DbContextOptionsBuilder<ProductReviewsContext>();
options.UseSqlServer(SqlConnectionBuilder.ExpressConnectionString("Mod1DB"));
var context = new ProductReviewsContext(options.Options);
var query = context.ProductGroups!
    .Include(pg=>pg.Products)
        .ThenInclude(p=>p.Brand);
foreach(var productGroup in query)
{
    Console.WriteLine($"{productGroup.Name}");
    foreach(var product in productGroup.Products)
    {
        Console.WriteLine($"\t - {product.Brand!.Name} {product.Name}" );
    }
}

Console.WriteLine("Done");
Console.ReadLine();