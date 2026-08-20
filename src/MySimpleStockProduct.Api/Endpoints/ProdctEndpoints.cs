using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;

namespace MySimpleStockProduct.Api.Controllers
{
    public static class ProdctEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/{page:int}/{pgeSize:int}", async (int page, int pgeSize, IProductService service, CancellationToken ct) =>
            {
                ResponseDTO<IEnumerable<ProductDTO>> products = await service.GetAsync(page, pgeSize);
                return Results.Ok(products);
            }).WithName("GetProducts")
               .WithSummary("List all products with a filter")
               .WithDescription("Returns all products available in the system with a filter.");

            group.MapGet("/{id:Guid}", async (Guid id, IProductService service, CancellationToken ct) =>
            {
                ResponseDTO<ProductDTO>? product = await service.GetByIdAsync(id, ct);
                return product is null ? Results.NotFound() : Results.Ok(product);
            }).WithName("GetProductById")
              .WithSummary("Get a product by id")
              .WithDescription("Returns a single product by its id.");

            group.MapPost("/", async (ProductDTO productDto, IProductService service, CancellationToken ct) =>
            {
                ResponseDTO<ProductDTO> created = await service.CreateAsync(productDto, ct);
                return created.Success ? Results.CreatedAtRoute("GetProductById", new { id = created.Data.Id }, created) : Results.BadRequest(created);
            }).WithName("CreateProduct")
              .WithSummary("Create a new product")
              .WithDescription("Creates a new product and returns it.");

            group.MapPut("/{id:Guid}", async (Guid id, ProductDTO productDto, IProductService service, CancellationToken ct) =>
            {
                ResponseDTO<ProductDTO> updated = await service.UpdateAsync(id, productDto, ct);
                return updated.Success ? Results.NoContent() : Results.BadRequest(updated);
            }).WithName("UpdateProduct")
              .WithSummary("Update an existing product")
              .WithDescription("Updates a product by id.");

            group.MapDelete("/{id:Guid}", async (Guid id, IProductService service, CancellationToken ct) =>
            {
                ResponseDTO<bool> deleted = await service.DeleteAsync(id, ct);
                return deleted.Success ? Results.NoContent() : Results.NotFound();
            }).WithName("DeleteProduct")
              .WithSummary("Delete a product")
              .WithDescription("Deletes a product by id.");


            return endpoints;
        }
    }
}
