using InventoryManagementSystem.API.Common;
using InventoryManagementSystem.Shared.Contracts.ProductSuppliers;

using MediatR;

namespace InventoryManagementSystem.API.Features.ProductSuppliers;

public class ProductSupplierModule : BaseEndpointModule
{
    public ProductSupplierModule() : base("product-suppliers") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ISender sender, CancellationToken cancellationToken = new()) =>
        {
            return await sender.Send(new GetProductSuppliers.Query(), cancellationToken);
        })
        .WithName(nameof(GetProductSuppliers))
        .WithTags(nameof(ProductSuppliers))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapGet("{id}", async (ISender sender, int id, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new GetProductSupplier.Query(id), cancellationToken);
        })
        .WithName(nameof(GetProductSupplier))
        .WithTags(nameof(ProductSuppliers))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("", async (ISender sender, ProductSupplierRequest data, CancellationToken cancellationToken = new()) =>
        {
            var id = await sender.Send(new CreateProductSupplier.Command(data), cancellationToken);
            return Results.Created($"product-categories/{id}", id);
        })
        .WithName(nameof(CreateProductSupplier))
        .WithTags(nameof(ProductSuppliers))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPut("{id}", async (ISender sender, ProductSupplierRequest data, int id, CancellationToken cancellationToken = new()) =>
        {
            await sender.Send(new UpdateProductSupplier.Command(id, data), cancellationToken);

            return Results.NoContent();
        })
        .WithName(nameof(UpdateProductSupplier))
        .WithTags(nameof(ProductSuppliers))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapDelete("{id}", async (ISender sender, int id, CancellationToken cancellationToken = new()) =>
        {
            var result = await sender.Send(new DeleteProductSupplier.Command(id), cancellationToken);
            return Results.Ok(result);
        })
        .WithName(nameof(DeleteProductSupplier))
        .WithTags(nameof(ProductSuppliers))
        .WithOpenApi()
        .RequireAuthorization();
    }
}