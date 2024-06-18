using FluentValidation;

using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.InventoryItems;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public static class CreateInventoryItem
{
    public record Command(InventoryItemRequest Data) : IRequest<int>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Data.ProductId).GreaterThan(0).MustAsync(BeUniqueProduct).WithMessage("The specified product already exists.");
            RuleFor(x => x.Data.WarehouseId).GreaterThan(0);
            RuleFor(x => x.Data.Quantity).GreaterThan(0);
        }

        private Task<bool> BeUniqueProduct(Command model, int productId, CancellationToken cancellationToken)
        {
            return _context.InventoryItems
                .Where(x => x.WarehouseId == model.Data.WarehouseId)
                .AllAsync(x => x.ProductId != productId, cancellationToken);
        }
    }

    public class Handler : IRequestHandler<Command, int>
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
            
            var entity = new Domain.Entities.InventoryItem
            {
                ProductId = request.Data.ProductId,
                WarehouseId = request.Data.WarehouseId,
                Quantity = request.Data.Quantity
            };

            _context.InventoryItems.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}