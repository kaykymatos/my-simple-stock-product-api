namespace MySimpleStockProduct.Api.Controllers
{
    public static class ProdctEndpoints
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var group = endpoints.MapGroup("/api/products").WithTags("Products");

            group.MapGet("/", () =>
                    {

                    }).WithName("GetProducts")
                      .WithSummary("List all products")
                      .WithDescription("Returns all products available in the system.");


            return endpoints;
        }
    }
}
