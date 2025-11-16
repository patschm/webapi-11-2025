using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductController(IUnitOfWork unitOfWork, ILogger<ProductController> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.ProductRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ICollection<Product>> Get(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Start {nameof(ProductController)}/{nameof(Get)}?page={page}&count={count}");
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<Product> Get(int id)
    {
        _logger.LogInformation($"Start {nameof(ProductController)}/{nameof(Get)}/{id}");
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Product product)
    {
        _logger.LogInformation($"Start {nameof(ProductController)}/{nameof(Post)}");
        var result  = await _repository.AddAsync(product);
        return CreatedAtAction(nameof(Get), new { id= result.Id});
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]Product product)
    {
        _logger.LogInformation($"Start {nameof(ProductController)}/{nameof(Put)}/{id}");
        product.Id = id;
        var result = await _repository.UpdateAsync(product);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Start {nameof(ProductController)}/{nameof(Delete)}/{id}");
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}