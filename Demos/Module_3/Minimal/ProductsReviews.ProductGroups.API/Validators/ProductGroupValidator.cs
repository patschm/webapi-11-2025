using FluentValidation;
using ProductsReviews.ProductGroups.API.DTO;

namespace ProductsReviews.ProductGroups.API.Validators;

public class ProductGroupValidator : AbstractValidator<ProductGroupDTO>
{
    public ProductGroupValidator()
    {
        RuleFor(b=>b.Name).NotEmpty().MaximumLength(255);
        RuleFor(b => b.Image).Matches("[^\\s]+(\\.(?i)(jpe?g|png|gif|bmp))$");
    }
}
