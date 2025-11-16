using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;
using Newtonsoft.Json;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class BrandController : ControllerBase
{
    private readonly ILogger<BrandController> _logger;
    private readonly IBrandRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public BrandController(IUnitOfWork unitOfWork, ILogger<BrandController> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.BrandRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ICollection<Brand>> Get(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Start {nameof(BrandController)}/{nameof(Get)}?page={page}&count={count}");
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<Brand> Get(int id)
    {
        _logger.LogInformation($"Start {nameof(BrandController)}/{nameof(Get)}/{id}");
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Brand brand)
    {
        _logger.LogInformation($"Start {nameof(BrandController)}/{nameof(Post)}");
        _logger.LogTrace(JsonConvert.SerializeObject(brand));
        var result  = await _repository.AddAsync(brand);
        return CreatedAtAction(nameof(Get), new { id= result.Id});
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]Brand brand)
    {
        _logger.LogInformation($"Start {nameof(BrandController)}/{nameof(Put)}/{id}");
        brand.Id = id;
        var result = await _repository.UpdateAsync(brand);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Start {nameof(BrandController)}/{nameof(Delete)}/{id}");
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}