using AutoMapper;
using ControllerWeb.DTO;
using Microsoft.AspNetCore.Mvc;
using ProductsReviews.DAL.Entities;
using ProductsReviews.DAL.Interfaces;

namespace ControllerWeb.Controllers;

[ApiController]
[Route("reviews")]
public class ReviewController : ControllerBase
{
    private readonly ILogger<ReviewController> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _uow;

    public ReviewController(IMapper mapper, IUnitOfWork uow, ILogger<ReviewController> logger)
    {
        _logger = logger;
        _mapper = mapper;
        _uow = uow;
    }

    [HttpGet]
    public async Task<IEnumerable<ReviewDTO>> GetReviews([FromQuery]int page = 1, [FromQuery]int count = 10)
    {
        var query = await _uow.ReviewRepository.GetAsync(page, count);
        return _mapper.ProjectTo<ReviewDTO>(query.AsQueryable());
    }

    [HttpGet]
    [Route("{id:int}")]
    [ProducesResponseType<ReviewDTO>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetReview([FromRoute]int id)
    {
        var review = await _uow.ReviewRepository.GetByIdAsync(id);
        if (review == null) return NotFound(new { Error = $"Not Review with {id} exists" });
        var dto = _mapper.Map<ReviewDTO>(review);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateReview(ReviewDTO review)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var dbReview = _mapper.Map<Review>(review);
        dbReview = await _uow.ReviewRepository.AddAsync(dbReview);
        review = _mapper.Map<ReviewDTO>(dbReview);
        return CreatedAtAction(nameof(GetReview), new { id=dbReview.Id }, review);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ModifyReview([FromRoute]int id, [FromBody]ReviewDTO review)
    {
        if (!ModelState.IsValid) 
        { 
            return BadRequest(ModelState);
        }
        var dbReview = await _uow.ReviewRepository.GetByIdAsync(id);
        if (dbReview == null)
        {
            return NotFound();
        }
        _mapper.Map(review, dbReview);
        await _uow.ReviewRepository.UpdateAsync(dbReview);
        return Accepted();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteReview(int id)
    {
        var review = await _uow.ReviewRepository.GetByIdAsync(id);
        if (review == null)
        {
            return NotFound();
        }
        await _uow.ReviewRepository.DeleteAsync(id);
        return NoContent();
    }
}
