using Xunit;
using ProductReviews.Repositories.EntityFramework;
using System.Threading.Tasks;
using ProductReviews.DAL.EntityFramework.Entities;


namespace ProductReviews.Repositories.Tests;

public class ProductRepositoryTests : TestBase
{   
    [Fact]
    public async Task TestPagingAsync()
    {
        var repo = new ProductRepository(CreateContext());

        var result = await repo.GetAsync(1, 10);
        Assert.NotNull(result);
        Assert.True(result.Count == 10);
    }
    [Fact]
    public async Task TestGetByIdAsync()
    {
        var repo = new ProductRepository(CreateContext());
        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.IsType<Product>(result);
    }
    [Fact]
    public async Task TestInsertAsync()
    {
        var repo = new ProductRepository(CreateContext());
        var tmp = new Product { Name = "Test", BrandId = 1, ProductGroupId=1};
        var result = await repo.AddAsync(tmp);
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
    }
    [Fact]
    public async Task TestUpdateAsync()
    {
        var repo = new ProductRepository(CreateContext());
        var tmp = await repo.GetByIdAsync(1);
        tmp.Name = "Test";
        var result = await repo.UpdateAsync(tmp);
        Assert.NotNull(result);
        Assert.True(result.Name == "Test");
    }
    [Fact]
    public async Task TestDeleteAsync()
    {
        var repo = new ProductRepository(CreateContext());
        await repo.DeleteAsync(1);
        var result = await repo.GetByIdAsync(1);

        Assert.Null(result);
    }
    [Fact]
    public async Task TestReviewsAsync()
    {
        var repo = new ProductRepository(CreateContext());
        var tmp = await repo.GetByIdAsync(1);
        var result = await repo.GetReviewsAsync(tmp.Id);

        Assert.NotNull(result);
        Assert.True(result.Count == 10);
    }

}