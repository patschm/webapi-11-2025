using AutoMapper;
using ProductsReviews.DAL.Entities;

namespace ProductsReviews.Reviews.API.DTO;

public class ReviewsProfile: Profile
{
    public ReviewsProfile()
    {
        CreateMap<Review, ReviewDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
    }
}
