using InventoryManagementSystemApi.API.Common;
using InventoryManagementSystemApi.API.Contracts.Warehouses;

using MediatR;

namespace InventoryManagementSystemApi.API.Features.Warehouses;

public class WarehouseModule : BaseEndpointModule
{
    public WarehouseModule() : base("warehouses") { }
    
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ISender sender, CancellationToken cancellationToken = new()) =>
        {
            return await sender.Send(new GetWarehouses.Query(), cancellationToken);
        })
        .WithName(nameof(GetWarehouses))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapGet("{id}", async (ISender sender, int id, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new GetWarehouse.Query(id), cancellationToken);
        })
        .WithName(nameof(GetWarehouse))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("", async (ISender sender, WarehouseRequest data, CancellationToken cancellationToken = new()) =>
        {
            var id = await sender.Send(new CreateWarehouse.Command(data), cancellationToken);
            return Results.Created($"warehouses/{id}", id);
        })
        .WithName(nameof(CreateWarehouse))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPut("{id}", async (ISender sender, WarehouseRequest data, int id, CancellationToken cancellationToken = new()) =>
        {
            await sender.Send(new UpdateWarehouse.Command(id, data), cancellationToken);

            return Results.NoContent();
        })
        .WithName(nameof(UpdateWarehouse))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapDelete("{id}", async (ISender sender, int id, CancellationToken cancellationToken = new()) =>
        {
            var result = await sender.Send(new DeleteWarehouse.Command(id), cancellationToken);
            return Results.Ok(result);
        })
        .WithName(nameof(DeleteWarehouse))
        .WithOpenApi()
        .RequireAuthorization();
    }
}