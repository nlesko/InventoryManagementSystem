using FluentValidation;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Contracts.InventoryItems;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public static class UpdateInventoryItem
{
    public record Command(int Id, InventoryItemRequest Data) : IRequest<Unit>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Data).NotNull();
            RuleFor(x => x.Data.ProductId).GreaterThan(0);
            RuleFor(x => x.Data.WarehouseId).GreaterThan(0).MustAsync(BeUniqueProduct).WithMessage("The specified product already exists.");
        }

        private Task<bool> BeUniqueProduct(Command model, int productId, CancellationToken cancellationToken)
        {
            return _context.InventoryItems
                .Where(x => x.Id != model.Id && x.WarehouseId == model.Data.WarehouseId)
                .AllAsync(x => x.ProductId != productId, cancellationToken);
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

            var entity = await _context.InventoryItems
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(InventoryItem), request.Id);
            }

            entity.ProductId = request.Data.ProductId;
            entity.WarehouseId = request.Data.WarehouseId;
            entity.Quantity = request.Data.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}