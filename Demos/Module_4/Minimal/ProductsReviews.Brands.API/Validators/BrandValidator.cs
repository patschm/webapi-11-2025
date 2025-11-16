using FluentValidation;
using ProductsReviews.Brands.API.DTO;

namespace ProductsReviews.Brands.API.Validators;

public class BrandValidator : AbstractValidator<BrandDTO>
{
    public BrandValidator()
    {
        RuleFor(b=>b.Name).NotEmpty().MaximumLength(255);
    }
}
