using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ICollection<Product>> Get(int page = 1, int count = 10)
    {
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<Product> Get(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Product product)
    {
        var result  = await _repository.AddAsync(product);
        return CreatedAtAction(nameof(Get), new { id= result.Id});
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]Product product)
    {
        product.Id = id;
        var result = await _repository.UpdateAsync(product);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}