using AutoMapper;
using ControllerWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using Products.DAL.Database;
using Products.DAL.Entities;

namespace ControllerWeb.Controllers;

[ApiController]
[Route("brands")]
public class BrandController : ControllerBase
{
    private readonly ILogger<BrandController> _logger;
    private readonly IMapper _mapper;
    private readonly ProductReviewsContext _dbContext;

    public BrandController(IMapper mapper, ProductReviewsContext dbContext, ILogger<BrandController> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IEnumerable<BrandDTO> GetBrands([FromQuery]int page = 1, [FromQuery]int count = 10)
    {
        var query = _dbContext.Brands.Skip((page - 1) * count).Take(count);
        return _mapper.ProjectTo<BrandDTO>(query);
    }

    // Alternatively: [HttpGet("{id}")]
    [HttpGet]
    [Route("{id:int}")]
    // Merely for documentation. Swagger uses them to specify the return values
    // since it cannot determine the response type (ActionResult is generic)
    [ProducesResponseType<BrandDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBrand([FromRoute]int id)
    {
        var brand = await _dbContext.Brands.FindAsync(id);
        if (brand == null) return NotFound(new { Error = $"Not brand with {id} exists" });
        var dto = _mapper.Map<BrandDTO>(brand);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateBrand(BrandDTO brand)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dbBrand = _mapper.Map<Brand>(brand);
        _dbContext.Brands.Add(dbBrand);
        int result = await _dbContext.SaveChangesAsync();
        if (result == 0)
        {
            return BadRequest(brand);
        }
        brand.Id = dbBrand.Id;
        return CreatedAtAction(nameof(GetBrand), new { id=dbBrand.Id }, brand);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ModifyBrand([FromRoute]int id, [FromBody]BrandDTO brand)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState);
        }
        var dbBrand = await _dbContext.Brands.FindAsync(id);
        if (dbBrand == null)
        {
            return NotFound();
        }
        _mapper.Map(brand, dbBrand);
        var result = await _dbContext.SaveChangesAsync();
        if (result == 0)
        {
            return BadRequest();
        }
        return Accepted();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBrand(int id)
    {
        var brand = await _dbContext.Brands.FindAsync(id);
        if (brand == null)
        {
            return NotFound();
        }
        _dbContext.Brands.Remove(brand);
        var result = await _dbContext.SaveChangesAsync();
        if (result == 0)
        {
            return BadRequest();
        }
        return NoContent();
    }
}
