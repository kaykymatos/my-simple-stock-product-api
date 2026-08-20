using MySimpleStockProduct.Application.DTOs;
using MySimpleStockProduct.Application.Interfaces;

namespace MySimpleStockProduct.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
        {
            RouteGroupBuilder group = endpoints.MapGroup("/api/categories").WithTags("Categories");

            group.MapGet("/{page:int}/{pgeSize:int}", async (int page, int pgeSize, ICategoryService service, CancellationToken ct) =>
            {
                ResponseDTO<IEnumerable<CategoryDTO>> categories = await service.GetAsync(page, pgeSize);
                return Results.Ok(categories);
            }).WithName("GetCategories")
              .WithSummary("List all categories with a filter")
              .WithDescription("Returns all categories available in the system with a filter.");

            group.MapGet("/{id:Guid}", async (Guid id, ICategoryService service, CancellationToken ct) =>
            {
                ResponseDTO<CategoryDTO>? category = await service.GetByIdAsync(id, ct);
                return category is null ? Results.NotFound() : Results.Ok(category);
            }).WithName("GetCategoryById")
              .WithSummary("Get a category by id")
              .WithDescription("Returns a single category by its id.");

            group.MapPost("/", async (CategoryDTO categoryDto, ICategoryService service, CancellationToken ct) =>
            {
                ResponseDTO<CategoryDTO> created = await service.CreateAsync(categoryDto, ct);
                return created.Success ? Results.CreatedAtRoute("GetCategoryById", new { id = created.Data.Id }, created) : Results.BadRequest(created);
            }).WithName("CreateCategory")
              .WithSummary("Create a new category")
              .WithDescription("Creates a new category and returns it.");

            group.MapPut("/{id:Guid}", async (Guid id, CategoryDTO categoryDto, ICategoryService service, CancellationToken ct) =>
            {
                ResponseDTO<CategoryDTO> updated = await service.UpdateAsync(id, categoryDto, ct);
                return updated.Success ? Results.NoContent() : Results.BadRequest(updated);
            }).WithName("UpdateCategory")
              .WithSummary("Update an existing category")
              .WithDescription("Updates a category by id.");

            group.MapDelete("/{id:Guid}", async (Guid id, ICategoryService service, CancellationToken ct) =>
            {
                ResponseDTO<bool> deleted = await service.DeleteAsync(id, ct);
                return deleted.Success ? Results.NoContent() : Results.NotFound();
            }).WithName("DeleteCategory")
              .WithSummary("Delete a category")
              .WithDescription("Deletes a category by id.");

            return endpoints;
        }
    }
}
