using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;

namespace ProductReviews.API.Controllers;

[Authorize]
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

    [RequiredScope("Data.Read")]
    [HttpGet]
    public async Task<ICollection<ProductGroup>> Get(int page = 1, int count = 10)
    {
        return await _repository.GetAsync(page, count);
    }
    [RequiredScope("Data.Read")]
    [HttpGet("{id}")]
    public async Task<ProductGroup> Get(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    [Authorize(Roles = "Writers")]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProductGroup productGroup)
    {
        var result = await _repository.AddAsync(productGroup); 
        return CreatedAtAction(nameof(Get), new { id=result.Id }, result);
    }
    [Authorize(Roles = "Writers")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]ProductGroup productGroup)
    {
        productGroup.Id = id;
        var result = await _repository.UpdateAsync(productGroup);
        return Ok(result);
    }
    [Authorize(Roles = "Writers")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}