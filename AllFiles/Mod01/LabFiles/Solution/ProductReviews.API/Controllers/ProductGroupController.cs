using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductGroupController : ControllerBase
{
    private readonly IProductGroupRepository _repository;

    public ProductGroupController(IProductGroupRepository repository)
    {
        _repository = repository;
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
    public async Task<ProductGroup> Post([FromBody]ProductGroup productGroup)
    {
        return await _repository.AddAsync(productGroup);
    }
    [HttpPut("{id}")]
    public async Task<ProductGroup> Put(int id, [FromBody]ProductGroup productGroup)
    {
        productGroup.Id = id;
        return await _repository.UpdateAsync(productGroup);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Ok();
    }
}