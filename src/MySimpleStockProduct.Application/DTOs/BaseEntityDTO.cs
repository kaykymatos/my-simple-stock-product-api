namespace MySimpleStockProduct.Application.DTOs
{
    public record BaseEntityDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
