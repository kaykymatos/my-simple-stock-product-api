using System;

namespace MySimpleStockProduct.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        protected Product()
        {
        }

        public Product(string name, string description, decimal price, Guid categoryId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.", nameof(name));

            if (price < 0m)
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be non-negative.");

            if (categoryId == Guid.Empty)
                throw new ArgumentException("Either categoryId must be provided or category must be supplied.");

            Id = Guid.NewGuid();
            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? string.Empty : description.Trim();
            Price = price;
            CategoryId = categoryId;
        }
    }
}
