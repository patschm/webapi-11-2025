using Microsoft.AspNetCore.Mvc;
using ProductReviews.Interfaces;
using ProductReviews.DAL.EntityFramework.Entities;
using Microsoft.AspNetCore.SignalR;
using ProductReviews.API.Hubs;

namespace ProductReviews.API.Controllers;

// [RequiredScope("Data.Read")]
[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<ReviewHub> _hub;

    public ReviewController(IUnitOfWork unitOfWork, IHubContext<ReviewHub> hub)
    {
        _unitOfWork = unitOfWork;
        _repository = _unitOfWork.ReviewRepository;
        _hub = hub;
    }
    [HttpGet]
    public async Task<ICollection<Review>> Get(int page = 1, int count = 10)
    {
        return await _repository.GetAsync(page, count);
    }
    [HttpGet("{id}")]
    public async Task<Review> Get(int id)
    {
        return await _repository.GetByIdAsync(id);
    }
    [HttpPost]
   // [Authorize(Roles = "Writers")]
    public async Task<IActionResult> Post([FromBody]Review review)
    {
        var result  = await _repository.AddAsync(review);
        await _hub.Clients.All.SendAsync("ReviewAdded", review);
        return CreatedAtAction(nameof(Get), new { id= result.Id});
    }
    [HttpPut("{id}")]
   // [Authorize(Roles = "Writers")]
    public async Task<IActionResult> Put(int id, [FromBody]Review review)
    {
        review.Id = id;
        var result = await _repository.UpdateAsync(review);
        await _hub.Clients.All.SendAsync("ReviewModified", review);
        return Ok(result);
    }
    [HttpDelete("{id}")]
   // [Authorize(Roles = "Writers")]
    public async Task<IActionResult> Delete(int id)
    {
        await _repository.DeleteAsync(id);
        return Accepted();
    }
}