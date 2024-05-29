using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public static class DeleteInventoryItem
{
    public record Command(int Id) : IRequest<int>;

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

            var entity = await _context.InventoryItems
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(InventoryItem), request.Id);
            }

            _context.InventoryItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}