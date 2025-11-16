using AutoMapper;
using ControllerWeb.Controllers;
using ControllerWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Products.DAL.Database;
using Products.DAL.Entities;

namespace ControllerWeb.Tests;

public class BrandControllerTests
{
    private readonly ProductReviewsContext _context;
    private readonly ILogger<BrandController> _logger;
    private readonly IMapper _mapper;

    public BrandControllerTests()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ProductReviewsContext>();
        optionsBuilder.UseInMemoryDatabase("brandtests");
        _context = new ProductReviewsContext(optionsBuilder.Options);
        _logger = NullLogger<BrandController>.Instance;
        var mapconfig = new MapperConfiguration(cfg => cfg.AddProfile<ProductsProfile>(), NullLoggerFactory.Instance);
        _mapper = mapconfig.CreateMapper();
    }

    [Fact]
    public void GetBrands_Return_ListOfBrandDTOs()
    {
        var controller = new BrandController(_mapper, _context, _logger);

        var result = controller.GetBrands(0, 5);

        Assert.NotEmpty(result);
        Assert.True(result.Count() == 5);
        Assert.All(result, item => Assert.IsType<BrandDTO>(item));
    }

    [Fact]
    public async Task GetBrand_Return_SingleAsync()
    {
        var controller = new BrandController(_mapper, _context, _logger);

        var result = await controller.GetBrand(1);

        Assert.IsType<OkObjectResult>(result);
        Assert.IsType<BrandDTO>((result as OkObjectResult)?.Value);      
    }

    [Fact]
    public async Task GetBrand_Return_NotFoundAsync()
    {
        var controller = new BrandController(_mapper, _context, _logger);

        var result = await controller.GetBrand(10000);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task PostBrand_Return_Created()
    {
        var controller = new BrandController(_mapper, _context, _logger);
        var dto = new BrandDTO { Name = "Test" };

        var result = await controller.CreateBrand(dto);

        Assert.IsType<CreatedAtActionResult>(result);
        var ar = (result as CreatedAtActionResult);
        Assert.IsType<BrandDTO>(ar?.Value);
        Assert.True(ar?.RouteValues?.Count  > 0);          
    }

    [Fact]
    public async Task PutBrand_Return_Accepted()
    {
        var controller = new BrandController(_mapper, _context, _logger);
        var entity = new Brand { Name = "Test" };
        _context.Brands.Add(entity);
        await _context.SaveChangesAsync();
        var dto = new BrandDTO { Name = "Test 2" };

        var result = await controller.ModifyBrand(entity.Id, dto);

        Assert.IsType<AcceptedResult>(result);
    }
    [Fact]
    public async Task PutBrand_Return_NotFound()
    {
        var controller = new BrandController(_mapper, _context, _logger);
        var dto = new BrandDTO { Name = "Test 2" };

        var result = await controller.ModifyBrand(100, dto);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task DeleteBrand_Return_NoContent()
    {
        var controller = new BrandController(_mapper, _context, _logger);
        var result = await controller.DeleteBrand(10);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteBrand_Return_NotFound()
    {
        var controller = new BrandController(_mapper, _context, _logger);
        var result = await controller.DeleteBrand(100);

        Assert.IsType<NotFoundResult>(result);
    }
}