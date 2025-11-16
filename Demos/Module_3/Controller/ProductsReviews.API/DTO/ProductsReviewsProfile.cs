using AutoMapper;
using ProductsReviews.DAL.Entities;

namespace ControllerWeb.DTO;

public class ProductsReviewsProfile: Profile
{
    public ProductsReviewsProfile()
    {
        CreateMap<Brand, BrandDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
        CreateMap<Product, ProductDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
        CreateMap<ProductGroup, ProductGroupDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
        CreateMap<Review, ReviewDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
    }
}
