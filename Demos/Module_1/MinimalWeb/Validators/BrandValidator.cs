using FluentValidation;
using MinimalWeb.DTO;

namespace MinimalWeb.Validators;

public class BrandValidator : AbstractValidator<BrandDTO>
{
    public BrandValidator()
    {
        RuleFor(b=>b.Name).NotEmpty().MaximumLength(255);
    }
}
