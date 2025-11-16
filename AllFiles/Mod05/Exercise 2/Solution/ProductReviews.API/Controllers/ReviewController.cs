using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;

namespace ProductReviews.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly ILogger<ReviewController> _logger;
    private readonly IReviewRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReviewController(IUnitOfWork unitOfWork, ILogger<ReviewController> logger)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.ReviewRepository;
        _logger = logger;
    }
    [HttpGet]
    public async Task<ICollection<Review>> Get(int page = 1, int count = 10)
    {
        _logger.LogInformation($"Start {nameof(ReviewController)}/{nameof(Get)}?page={page}&count={count}");
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<Review> Get(int id)
    {
        _logger.LogInformation($"Start {nameof(ReviewController)}/{nameof(Get)}/{id}");
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Review review)
    {
        _logger.LogInformation($"Start {nameof(ReviewController)}/{nameof(Post)}");
        var result  = await _repository.AddAsync(review);
        return CreatedAtAction(nameof(Get), new { id= result.Id});
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]Review review)
    {
        _logger.LogInformation($"Start {nameof(ReviewController)}/{nameof(Put)}/{id}");
        review.Id = id;
        var result = await _repository.UpdateAsync(review);
        return Ok(result);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        _logger.LogInformation($"Start {nameof(ReviewController)}/{nameof(Delete)}/{id}");
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}