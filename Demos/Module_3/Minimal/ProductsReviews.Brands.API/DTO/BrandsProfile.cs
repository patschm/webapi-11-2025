using AutoMapper;
using ProductsReviews.DAL.Entities;

namespace ProductsReviews.Brands.API.DTO;

public class BrandsProfile: Profile
{
    public BrandsProfile()
    {
        CreateMap<Brand, BrandDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
    }
}
