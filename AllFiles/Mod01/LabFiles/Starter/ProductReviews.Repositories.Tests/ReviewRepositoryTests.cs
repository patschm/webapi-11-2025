using ProductReviews.Repositories.EntityFramework;
using ProductReviews.DAL.EntityFramework.Entities;


namespace ProductReviews.Repositories.Tests;

public class ReviewRepositoryTests : TestBase
{   
    [Fact]
    public async Task TestPagingAsync()
    {
        var repo = new ReviewRepository(CreateContext());

        var result = await repo.GetAsync(1, 5);
        Assert.NotNull(result);
        Assert.True(result.Count == 5);
    }
    [Fact]
    public async Task TestGetByIdAsync()
    {
        var repo = new ReviewRepository(CreateContext());
        var result = await repo.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.IsType<Review>(result);
    }
    [Fact]
    public async Task TestInsertAsync()
    {
        var repo = new ReviewRepository(CreateContext());
        var tmp = new Review { Text = "Test", ProductId = 1};
        var result = await repo.AddAsync(tmp);
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
    }
    [Fact]
    public async Task TestDeleteAsync()
    {
        var repo = new ReviewRepository(CreateContext());
        
        await repo.DeleteAsync(1);
        var result = await repo.GetByIdAsync(1);

        Assert.Null(result);
    }
    [Fact]
    public async Task TestUpdateAsync()
    {
        var repo = new ReviewRepository(CreateContext());
       
        var tmp = await repo.GetByIdAsync(1);
        tmp.Text = "Test";
        var result = await repo.UpdateAsync(tmp);

        Assert.NotNull(result);
        Assert.True(result.Text == "Test");
    }
}