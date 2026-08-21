using FluentValidation;
using MySimpleStockProduct.Application.DTOs;

namespace MySimpleStockProduct.Application.Validators
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(x => x.Name)
               .Cascade(CascadeMode.Stop)
               .NotEmpty().WithMessage("Product name is required.")
               .Length(3, 100).WithMessage("Product name must be between 3 and 100 characters.");

            RuleFor(x => x.Description)
                 .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Product description is required.")
                .Length(3, 100).WithMessage("Product description must be between 3 and 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
