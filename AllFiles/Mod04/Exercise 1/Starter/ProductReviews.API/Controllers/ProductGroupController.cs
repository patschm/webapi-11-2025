using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductGroupController : ControllerBase
{
    private readonly IProductGroupRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductGroupController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.ProductGroupRepository;
    }

    [HttpGet]
    public async Task<ICollection<ProductGroup>> Get(int page = 1, int count = 10)
    {
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<ProductGroup> Get(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProductGroup productGroup)
    {
        var result = await _repository.AddAsync(productGroup); 
        return CreatedAtAction(nameof(Get), new { id=result.Id }, result);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]ProductGroup productGroup)
    {
        productGroup.Id = id;
        var result = await _repository.UpdateAsync(productGroup);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}