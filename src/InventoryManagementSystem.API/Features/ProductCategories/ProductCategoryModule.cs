using InventoryManagementSystem.API.Common;
using InventoryManagementSystem.Shared.Contracts.ProductCategories;

using MediatR;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public class ProductCategoryModule : BaseEndpointModule
{
    public ProductCategoryModule() : base("product-categories") { }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (ISender sender, CancellationToken cancellationToken = new()) =>
        {
            return await sender.Send(new GetProductCategories.Query(), cancellationToken);
        })
        .WithName(nameof(GetProductCategories))
        .WithTags(nameof(ProductCategories))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapGet("{id}", async (ISender sender, int id, CancellationToken cancellationToken) =>
        {
            return await sender.Send(new GetProductCategory.Query(id), cancellationToken);
        })
        .WithName(nameof(GetProductCategory))
        .WithTags(nameof(ProductCategories))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPost("", async (ISender sender, ProductCategoryRequest data, CancellationToken cancellationToken = new()) =>
        {
            var id = await sender.Send(new CreateProductCategory.Command(data), cancellationToken);
            return Results.Created($"product-categories/{id}", id);
        })
        .WithName(nameof(CreateProductCategory))
        .WithTags(nameof(ProductCategories))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapPut("{id}", async (ISender sender, ProductCategoryRequest data, int id, CancellationToken cancellationToken = new()) =>
        {
            await sender.Send(new UpdateProductCategory.Command(id, data), cancellationToken);

            return Results.NoContent();
        })
        .WithName(nameof(UpdateProductCategory))
        .WithTags(nameof(ProductCategories))
        .WithOpenApi()
        .RequireAuthorization();

        app.MapDelete("{id}", async (ISender sender, int id, CancellationToken cancellationToken = new()) =>
        {
            var result = await sender.Send(new DeleteProductCategory.Command(id), cancellationToken);
            return Results.Ok(result);
        })
        .WithName(nameof(DeleteProductCategory))
        .WithTags(nameof(ProductCategories))
        .WithOpenApi()
        .RequireAuthorization();
    }
}