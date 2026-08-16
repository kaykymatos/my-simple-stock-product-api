using MySimpleStockProduct.Api.Controllers;
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
    app.MapOpenApi();
}

app.MapProductEndpoints();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
