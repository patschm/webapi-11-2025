using AutoMapper;
using ControllerWeb.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ControllerWeb.Controllers;

[Authorize]
[ApiController]
[Route("brands")]
public class BrandController : ControllerBase
{
    private readonly ILogger<BrandController> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public BrandController(IMapper mapper, IUnitOfWork uow, ILogger<BrandController> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _uow = uow;
    }

    [HttpGet]
    public async Task<IEnumerable<BrandDTO>> GetBrands([FromQuery]int page = 1, [FromQuery]int count = 10)
    {
        var query = await _uow.BrandRepository.GetAsync(page, count);
        return _mapper.ProjectTo<BrandDTO>(query.AsQueryable());
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType<BrandDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetBrand([FromRoute]int id)
    {
        var brand = await _uow.BrandRepository.GetByIdAsync(id);
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
        dbBrand = await _uow.BrandRepository.AddAsync(dbBrand);
        brand = _mapper.Map<BrandDTO>(dbBrand);
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
        var dbBrand = await _uow.BrandRepository.GetByIdAsync(id);
        if (dbBrand == null)
        {
            return NotFound();
        }
        _mapper.Map(brand, dbBrand);
        await _uow.BrandRepository.UpdateAsync(dbBrand);
        return Accepted();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteBrand(int id)
    {
        var brand = await _uow.BrandRepository.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound();
        }
        await _uow.BrandRepository.DeleteAsync(id);
        return NoContent();
    }
}
