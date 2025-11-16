using AutoMapper;
using ProductsReviews.DAL.Entities;

namespace ProductsReviews.Products.API.DTO;

public class ProductsProfile: Profile
{
    public ProductsProfile()
    {
        CreateMap<Product, ProductDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
    }
}
