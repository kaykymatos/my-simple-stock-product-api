using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;

namespace MySimpleStockProduct.Api.Endpoints
{
    using MySimpleStockProduct.Logging;

    public class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/api/categories").WithTags("Categories");

            group.MapGet("/{page:int}/{pageSize:int}", async (
                int page,
                int pageSize,
                ICategoryService service,
                ICustomLogger<CategoryEndpoints> logger,
                CancellationToken cancellationToken) =>
            {
                logger.LogInfo("Fetching categories page {0} with size {1}", page, pageSize);

                ResponseDTO<IEnumerable<CategoryDTO>> categories = await service.GetAsync(page, pageSize);

                logger.LogInfo("Successfully retrieved categories for page {0}", page);
                return Results.Ok(categories);
            }).WithName("GetCategories")
              .WithSummary("List all categories with a filter")
              .WithDescription("Returns all categories available in the system with a filter.");

            group.MapGet("/{id:Guid}", async (
                Guid id,
                ICategoryService service,
                ICustomLogger<CategoryEndpoints> logger,
                CancellationToken cancellationToken) =>
            {
                logger.LogInfo("Fetching category with ID: {0}", id);

                ResponseDTO<CategoryDTO>? category = await service.GetByIdAsync(id, cancellationToken);

                if (!category.Success)
                {
                    logger.LogWarning("Category with ID {0} was not found", id);
                    return Results.NotFound();
                }

                logger.LogInfo("Successfully fetched category with ID: {0}", id);
                return Results.Ok(category);
            }).WithName("GetCategoryById")
              .WithSummary("Get a category by id")
              .WithDescription("Returns a single category by its id.");

            group.MapPost("/", async (
                CategoryDTO categoryDto,
                ICategoryService service,
                ICustomLogger<CategoryEndpoints> logger,
                CancellationToken cancellationToken) =>
            {
                logger.LogInfo("Initiating creation for category: {0}", categoryDto.Name);

                ResponseDTO<CategoryDTO> created = await service.CreateAsync(categoryDto, cancellationToken);

                if (!created.Success)
                {
                    logger.LogWarning("Failed to create category: {0}", categoryDto.Name);
                    return Results.BadRequest(created);
                }

                logger.LogInfo("Category created successfully with ID: {0}", created.Data!.Id);
                return Results.CreatedAtRoute("GetCategoryById", new { id = created.Data!.Id }, created);
            }).WithName("CreateCategory")
              .WithSummary("Create a new category")
              .WithDescription("Creates a new category and returns it.");

            group.MapPut("/{id:Guid}", async (
                Guid id,
                CategoryDTO categoryDto,
                ICategoryService service,
                ICustomLogger<CategoryEndpoints> logger,
                CancellationToken cancellationToken) =>
            {
                logger.LogInfo("Updating category with ID: {0}", id);

                ResponseDTO<CategoryDTO> updated = await service.UpdateAsync(id, categoryDto, cancellationToken);

                if (!updated.Success)
                {
                    logger.LogWarning("Failed to update category with ID: {0}", id);
                    return Results.BadRequest(updated);
                }

                logger.LogInfo("Category with ID {0} updated successfully", id);
                return Results.NoContent();
            }).WithName("UpdateCategory")
              .WithSummary("Update an existing category")
              .WithDescription("Updates a category by id.");

            group.MapDelete("/{id:Guid}", async (
                Guid id,
                ICategoryService service,
                ICustomLogger<CategoryEndpoints> logger,
                CancellationToken cancellationToken) =>
            {
                logger.LogInfo("Request received to delete category with ID: {0}", id);

                ResponseDTO<bool> deleted = await service.DeleteAsync(id, cancellationToken);

                if (!deleted.Success)
                {
                    logger.LogWarning("Category with ID {0} could not be deleted or was not found", id);
                    return Results.NotFound();
                }

                logger.LogInfo("Category with ID {0} deleted successfully", id);
                return Results.NoContent();
            }).WithName("DeleteCategory")
              .WithSummary("Delete a category")
              .WithDescription("Deletes a category by id.");
        }
    }
}
