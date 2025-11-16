using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProductReviews.DAL.EntityFramework.Database;

namespace ProductReviews.Repositories.Tests
{
    public class TestBase
    {
    private static SqliteConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        return connection;
    }
    protected ProductReviewsContext CreateContext()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductReviewsContext>();
        optionsBuilder.UseSqlite<ProductReviewsContext>(CreateInMemoryDatabase());
        return new ProductReviewsContext(optionsBuilder.Options);
    }

    }
}