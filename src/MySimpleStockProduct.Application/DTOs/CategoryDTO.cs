namespace MySimpleStockProduct.Application.DTOs
{
    public record CategoryDTO : BaseEntityDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
