using FluentValidation;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Contracts.Products;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.Products;

public static class UpdateProduct
{
    public record Command(int Id, ProductRequest Data) : IRequest<Unit>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Data).NotNull();
            RuleFor(x => x.Data.Name).NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");;
            RuleFor(x => x.Data.SKU).MustAsync(BeUniqueSKU).WithMessage("The specified SKU already exists.");
            RuleFor(x => x.Data.Description).MaximumLength(2000);
            RuleFor(x => x.Data.ProductSupplierId).GreaterThan(0);
        }

        private Task<bool> BeUniqueName(Command model, string name, CancellationToken cancellationToken)
        {
            return _context.Products
                .Where(x => x.Id != model.Id)
                .AllAsync(x => x.Name != name, cancellationToken);
        }

        private Task<bool> BeUniqueSKU(Command model, string? sku, CancellationToken cancellationToken)
        {
            return _context.Products
                .Where(x => x.Id != model.Id)
                .AllAsync(x => string.IsNullOrEmpty(sku) || x.SKU != sku, cancellationToken);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Unit>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.Products
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            entity.Name = request.Data.Name;
            entity.Description = request.Data.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}