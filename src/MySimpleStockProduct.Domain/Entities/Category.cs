namespace MySimpleStockProduct.Domain.Entities
{
    public class Category : BaseEntity
    {
        public Category(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
