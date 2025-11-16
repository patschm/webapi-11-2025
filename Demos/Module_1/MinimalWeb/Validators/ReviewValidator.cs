using FluentValidation;
using MinimalWeb.DTO;

namespace MinimalWeb.Validators;

public class ReviewValidator : AbstractValidator<ReviewDTO>
{
    public ReviewValidator()
    {
        RuleFor(b=>b.Author).NotEmpty().MaximumLength(255);
        RuleFor(b => b.Email).NotEmpty().MaximumLength(255).Matches("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
        RuleFor(b => b.Text).NotEmpty().MaximumLength(1024);
        RuleFor(b => b.Score).NotEmpty().InclusiveBetween(1, 5);
        RuleFor(b => b.ProductId).NotEmpty();
    }
}
