using AutoMapper;
using ControllerWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ControllerWeb.Controllers;

[ApiController]
[Route("productgroups")]
public class ProductGroupController : ControllerBase
{
    private readonly ILogger<ProductGroupController> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public ProductGroupController(IMapper mapper, IUnitOfWork uow, ILogger<ProductGroupController> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _uow = uow;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductGroupDTO>> GetProducts(int page = 1, int count = 10)
    {
        var query = await _uow.ProductGroupRepository.GetAsync(page, count);
        return _mapper.ProjectTo<ProductGroupDTO>(query.AsQueryable());
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType<ProductGroupDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetProductGroup([FromRoute]int id)
    {
        var productgroup = await _uow.ProductGroupRepository.GetByIdAsync(id);
        if (productgroup == null) return NotFound(new { Error = $"No product with {id} exists" });
        var dto = _mapper.Map<ProductGroupDTO>(productgroup);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProductGroup(ProductGroupDTO productGroup)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dbProductGroup = _mapper.Map<ProductGroup>(productGroup);
        dbProductGroup = await _uow.ProductGroupRepository.AddAsync(dbProductGroup);
        productGroup = _mapper.Map<ProductGroupDTO>(dbProductGroup);
        return CreatedAtAction(nameof(GetProductGroup), new { id=dbProductGroup.Id }, productGroup);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ModifyProductGroup([FromRoute]int id, [FromBody]ProductGroupDTO productGroup)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState);
        }
        var dbProductGroup = await _uow.ProductGroupRepository.GetByIdAsync(id);
        if (dbProductGroup == null)
        {
            return NotFound();
        }
        _mapper.Map(productGroup, dbProductGroup);
        await _uow.ProductGroupRepository.UpdateAsync(dbProductGroup);
        return Accepted();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteProductGroup(int id)
    {
        var productGroup = await _uow.ProductGroupRepository.GetByIdAsync(id);
        if (productGroup == null)
        {
            return NotFound();
        }
        await _uow.ProductGroupRepository.DeleteAsync(id);
        return NoContent();
    }
}
