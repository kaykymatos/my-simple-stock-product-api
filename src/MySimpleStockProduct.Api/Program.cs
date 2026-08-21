using MySimpleStockProduct.Api.Controllers;
using MySimpleStockProduct.Api.Endpoints;
using MySimpleStockProduct.IoC;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 1. Map the native OpenAPI JSON endpoint (/openapi/v1.json)
    app.MapOpenApi();

    // 2. Enable Swagger UI and point it to the native JSON endpoint
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });
}

CategoryEndpoints.MapCategoryEndpoints(app);
ProdctEndpoints.MapProductEndpoints(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
