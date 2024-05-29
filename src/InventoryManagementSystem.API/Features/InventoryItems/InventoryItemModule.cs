using InventoryManagementSystem.API.Common;
using InventoryManagementSystem.API.Contracts.InventoryItems;

using MediatR;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public class InventoryItemModule : BaseEndpointModule
{
    public InventoryItemModule() : base("inventory-items") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ISender sender, CancellationToken cancellationToken = new()) =>
        {
            return await sender.Send(new GetInventoryItems.Query(), cancellationToken);
        })
        .WithName(nameof(GetInventoryItems))
        .WithTags(nameof(InventoryItems))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapGet("{id}", async (ISender sender, int id, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new GetInventoryItem.Query(id), cancellationToken);
        })
        .WithName(nameof(GetInventoryItem))
        .WithTags(nameof(InventoryItems))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("", async (ISender sender, InventoryItemRequest data, CancellationToken cancellationToken = new()) =>
        {
            var id = await sender.Send(new CreateInventoryItem.Command(data), cancellationToken);
            return Results.Created($"inventoryItems/{id}", id);
        })
        .WithName(nameof(CreateInventoryItem))
        .WithTags(nameof(InventoryItems))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPut("{id}", async (ISender sender, InventoryItemRequest data, int id, CancellationToken cancellationToken = new()) =>
        {
            await sender.Send(new UpdateInventoryItem.Command(id, data), cancellationToken);

            return Results.NoContent();
        })
        .WithName(nameof(UpdateInventoryItem))
        .WithTags(nameof(InventoryItems))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapDelete("{id}", async (ISender sender, int id, CancellationToken cancellationToken = new()) =>
        {
            var result = await sender.Send(new DeleteInventoryItem.Command(id), cancellationToken);
            return Results.Ok(result);
        })
        .WithName(nameof(DeleteInventoryItem))
        .WithTags(nameof(InventoryItems))
        .WithOpenApi()
        .RequireAuthorization();
    }
}