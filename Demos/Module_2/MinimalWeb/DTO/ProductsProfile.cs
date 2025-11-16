using AutoMapper;
using Products.DAL.Entities;

namespace MinimalWeb.DTO;

public class ProductsProfile: Profile
{
    public ProductsProfile()
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
