using FluentValidation;

using InventoryManagementSystemApi.API.Contracts.Products;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.Products;

public static class CreateProduct
{
    public record Command(ProductRequest Data) : IRequest<int>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Data.Name).NotNull().NotEmpty().MaximumLength(124).MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
            RuleFor(x => x.Data.SKU).MustAsync(BeUniqueSKU).WithMessage("The specified SKU already exists.");
            RuleFor(x => x.Data.Description).MaximumLength(2000);
            RuleFor(x => x.Data.ProductSupplierId).GreaterThan(0);
        }
        private Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return _context.Products
                .AllAsync(x => x.Name != name, cancellationToken);
        }

        private Task<bool> BeUniqueSKU(string? sku, CancellationToken cancellationToken)
        {
            return _context.Products
                .AllAsync(x => string.IsNullOrEmpty(sku) || x.SKU != sku, cancellationToken);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, int>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = new Product
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                SKU = request.Data.SKU,
                ProductSupplierId = request.Data.ProductSupplierId,
                ProductCategoryId = request.Data.ProductCategoryId,
                ProductSubCategoryId = request.Data.ProductSubCategoryId,
                Price = request.Data.Price,
                QuantityInStock = request.Data.QuantityInStock,
                ImageUrl = request.Data.ImageUrl
            };

            _context.Products.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}