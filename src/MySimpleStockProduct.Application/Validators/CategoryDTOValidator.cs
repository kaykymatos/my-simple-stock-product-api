using FluentValidation;
using MySimpleStockProduct.Application.DTOs;

namespace MySimpleStockProduct.Application.Validators
{
    public class CategoryDTOValidator : AbstractValidator<CategoryDTO>
    {
        public CategoryDTOValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Category name is required.")
                .Length(3, 100).WithMessage("Category name must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                 .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Category description is required.")
                .Length(3, 100).WithMessage("Category description must be between 3 and 100 characters.");
        }
    }
}
