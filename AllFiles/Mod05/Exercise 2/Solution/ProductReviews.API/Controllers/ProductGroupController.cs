
using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductGroupController : ControllerBase
{
    private readonly ILogger<ProductGroupController> _logger;
    private readonly IProductGroupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductGroupController(IUnitOfWork unitOfWork, ILogger<ProductGroupController> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.ProductGroupRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ICollection<ProductGroup>> Get(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Start {nameof(ProductGroupController)}/{nameof(Get)}?page={page}&count={count}");
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<ProductGroup> Get(int id)
    {
        _logger.LogInformation($"Start {nameof(ProductGroupController)}/{nameof(Get)}/{id}");
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProductGroup productGroup)
    {
        _logger.LogInformation($"Start {nameof(ProductGroupController)}/{nameof(Post)}");
        var result = await _repository.AddAsync(productGroup); 
        return CreatedAtAction(nameof(Get), new { id=result.Id }, result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]ProductGroup productGroup)
    {
        _logger.LogInformation($"Start {nameof(ProductGroupController)}/{nameof(Put)}/{id}");
        productGroup.Id = id;
        var result = await _repository.UpdateAsync(productGroup);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Start {nameof(ProductGroupController)}/{nameof(Delete)}/{id}");
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}