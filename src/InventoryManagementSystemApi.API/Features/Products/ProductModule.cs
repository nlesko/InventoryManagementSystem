using InventoryManagementSystemApi.API.Common;
using InventoryManagementSystemApi.API.Contracts.Products;

using MediatR;

namespace InventoryManagementSystemApi.API.Features.Products;

public class ProductModule : BaseEndpointModule
{
    public ProductModule() : base("products") { }

     public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ISender sender, CancellationToken cancellationToken = new()) =>
        {
            return await sender.Send(new GetProducts.Query(), cancellationToken);
        })
        .WithName(nameof(GetProducts))
        .WithTags(nameof(Products))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapGet("{id}", async (ISender sender, int id, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new GetProduct.Query(id), cancellationToken);
        })
        .WithName(nameof(GetProduct))
        .WithTags(nameof(Products))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("", async (ISender sender, ProductRequest data, CancellationToken cancellationToken = new()) =>
        {
            var id = await sender.Send(new CreateProduct.Command(data), cancellationToken);
            return Results.Created($"product-categories/{id}", id);
        })
        .WithName(nameof(CreateProduct))
        .WithTags(nameof(Products))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPut("{id}", async (ISender sender, ProductRequest data, int id, CancellationToken cancellationToken = new()) =>
        {
            await sender.Send(new UpdateProduct.Command(id, data), cancellationToken);

            return Results.NoContent();
        })
        .WithName(nameof(UpdateProduct))
        .WithTags(nameof(Products))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapDelete("{id}", async (ISender sender, int id, CancellationToken cancellationToken = new()) =>
        {
            var result = await sender.Send(new DeleteProduct.Command(id), cancellationToken);
            return Results.Ok(result);
        })
        .WithName(nameof(DeleteProduct))
        .WithTags(nameof(Products))
        .WithOpenApi()
        .RequireAuthorization();
    }
}