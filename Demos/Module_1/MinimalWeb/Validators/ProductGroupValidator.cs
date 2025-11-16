using FluentValidation;
using MinimalWeb.DTO;

namespace MinimalWeb.Validators;

public class ProductGroupValidator : AbstractValidator<ProductGroupDTO>
{
    public ProductGroupValidator()
    {
        RuleFor(b=>b.Name).NotEmpty().MaximumLength(255);
        RuleFor(b => b.Image).Matches("[^\\s]+(\\.(?i)(jpe?g|png|gif|bmp))$");
    }
}
