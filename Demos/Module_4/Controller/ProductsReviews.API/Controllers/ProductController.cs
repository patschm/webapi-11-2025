using AutoMapper;
using ControllerWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ControllerWeb.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public ProductController(IMapper mapper, IUnitOfWork uow, ILogger<ProductController> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _uow = uow;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductDTO>> GetProducts(int page = 1, int count = 10)
    {
        var query = await _uow.ProductRepository.GetAsync(page, count);
        return _mapper.ProjectTo<ProductDTO>(query.AsQueryable());
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType<ProductDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetProduct([FromRoute]int id)
    {
        var product = await _uow.ProductRepository.GetByIdAsync(id);
        if (product == null) return NotFound(new { Error = $"No product with {id} exists" });
        var dto = _mapper.Map<ProductDTO>(product);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProduct(ProductDTO product)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dbProduct = _mapper.Map<Product>(product);
        dbProduct = await _uow.ProductRepository.AddAsync(dbProduct);
        product = _mapper.Map<ProductDTO>(dbProduct);
        return CreatedAtAction(nameof(GetProduct), new { id=dbProduct.Id }, product);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ModifyProduct([FromRoute]int id, [FromBody]ProductDTO product)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState);
        }
        var dbProduct = await _uow.ProductRepository.GetByIdAsync(id);
        if (dbProduct == null)
        {
            return NotFound();
        }
        _mapper.Map(product, dbProduct);
        await _uow.ProductRepository.UpdateAsync(dbProduct);
        return Accepted();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await _uow.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        await _uow.ProductRepository.DeleteAsync(id);
        return NoContent();
    }
}
