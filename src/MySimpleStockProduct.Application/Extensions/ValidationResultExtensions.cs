using FluentValidation.Results;

namespace MySimpleStockProduct.Application.Extensions
{
    public static class ValidationResultExtensions
    {
        public static Dictionary<string, string[]> ToDictionary(this ValidationResult result)
        {
            return result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(e => e.ErrorMessage).ToArray()
                );
        }
    }
}
