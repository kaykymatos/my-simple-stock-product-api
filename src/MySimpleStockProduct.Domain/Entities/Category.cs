namespace MySimpleStockProduct.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
        }
        public Category(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Category description is required.", nameof(description));
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
