using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;
using MySimpleStockProduct.Logging;

namespace MySimpleStockProduct.Api.Controllers
{
    public class ProdctEndpoints
    {
        public static void MapProductEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/{page:int}/{pgeSize:int}", async (int page, int pgeSize, IProductService service, ICustomLogger<ProdctEndpoints> logger, CancellationToken ct) =>
            {
                logger.LogInfo("Fetching products page {0} with size {1}", page, pgeSize);

                ResponseDTO<IEnumerable<ProductDTO>> products = await service.GetAsync(page, pgeSize);

                logger.LogInfo("Successfully retrieved products for page {0}", page);
                return Results.Ok(products);
            }).WithName("GetProducts")
               .WithSummary("List all products with a filter")
               .WithDescription("Returns all products available in the system with a filter.");

            group.MapGet("/{id:Guid}", async (Guid id, IProductService service, ICustomLogger<ProdctEndpoints> logger, CancellationToken ct) =>
            {
                logger.LogInfo("Fetching product with ID: {0}", id);

                ResponseDTO<ProductDTO>? product = await service.GetByIdAsync(id, ct);

                if (product is null || !product.Success)
                {
                    logger.LogWarning("Product with ID {0} was not found", id);
                    return Results.NotFound();
                }

                logger.LogInfo("Successfully fetched product with ID: {0}", id);
                return Results.Ok(product);
            }).WithName("GetProductById")
              .WithSummary("Get a product by id")
              .WithDescription("Returns a single product by its id.");

            group.MapPost("/", async (ProductDTO productDto, IProductService service, ICustomLogger<ProdctEndpoints> logger, CancellationToken ct) =>
            {
                logger.LogInfo("Initiating creation for product: {0}", productDto.Name);

                ResponseDTO<ProductDTO> created = await service.CreateAsync(productDto, ct);

                if (!created.Success)
                {
                    logger.LogWarning("Failed to create product: {0}", productDto.Name);
                    return Results.BadRequest(created);
                }

                logger.LogInfo("Product created successfully with ID: {0}", created.Data!.Id);
                return Results.CreatedAtRoute("GetProductById", new { id = created.Data!.Id }, created);
            }).WithName("CreateProduct")
              .WithSummary("Create a new product")
              .WithDescription("Creates a new product and returns it.");

            group.MapPut("/{id:Guid}", async (Guid id, ProductDTO productDto, IProductService service, ICustomLogger<ProdctEndpoints> logger, CancellationToken ct) =>
            {
                logger.LogInfo("Updating product with ID: {0}", id);

                ResponseDTO<ProductDTO> updated = await service.UpdateAsync(id, productDto, ct);

                if (!updated.Success)
                {
                    logger.LogWarning("Failed to update product with ID: {0}", id);
                    return Results.BadRequest(updated);
                }

                logger.LogInfo("Product with ID {0} updated successfully", id);
                return Results.NoContent();
            }).WithName("UpdateProduct")
              .WithSummary("Update an existing product")
              .WithDescription("Updates a product by id.");

            group.MapDelete("/{id:Guid}", async (Guid id, IProductService service, ICustomLogger<ProdctEndpoints> logger, CancellationToken ct) =>
            {
                logger.LogInfo("Request received to delete product with ID: {0}", id);

                ResponseDTO<bool> deleted = await service.DeleteAsync(id, ct);

                if (!deleted.Success)
                {
                    logger.LogWarning("Product with ID {0} could not be deleted or was not found", id);
                    return Results.NotFound();
                }

                logger.LogInfo("Product with ID {0} deleted successfully", id);
                return Results.NoContent();
            }).WithName("DeleteProduct")
              .WithSummary("Delete a product")
              .WithDescription("Deletes a product by id.");
        }
    }
}
