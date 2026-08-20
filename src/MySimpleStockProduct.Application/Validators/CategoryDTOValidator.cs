using FluentValidation;
using MySimpleStockProduct.Application.DTOs;

namespace MySimpleStockProduct.Application.Validators
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(x => x.Name)
                 .NotEmpty().WithMessage("Category name is required.")
                 .Length(3, 100).WithMessage("Category name must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}
