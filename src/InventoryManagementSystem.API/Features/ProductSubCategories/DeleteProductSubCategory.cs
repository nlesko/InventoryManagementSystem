using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public static class DeleteProductSubCategory
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

            var entity = await _context.ProductSubCategories
                                .Where(x => x.Id == request.Id)
                                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductSubCategory), request.Id);
            }

            _context.ProductSubCategories.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}