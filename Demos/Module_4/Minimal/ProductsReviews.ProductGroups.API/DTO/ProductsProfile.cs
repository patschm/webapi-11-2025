using AutoMapper;
using ProductsReviews.DAL.Entities;

namespace ProductsReviews.ProductGroups.API.DTO;

public class ProductGroupsProfile: Profile
{
    public ProductGroupsProfile()
    {
        CreateMap<ProductGroup, ProductGroupDTO>()
            .ReverseMap()
            .ForMember(dest => dest.Id, src => src.Ignore());
    }
}
