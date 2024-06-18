using FluentValidation;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.ProductSubCategories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public static class UpdateProductSubCategory
{
    public record Command(int Id, ProductSubCategoryRequest Data) : IRequest<Unit>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Data).NotNull();
            RuleFor(x => x.Data.Name).NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");;
            RuleFor(x => x.Data.ProductCategoryId).GreaterThan(0);
        }

        private Task<bool> BeUniqueName(Command model, string name, CancellationToken cancellationToken)
        {
            return _context.ProductSubCategories
                .Where(x => x.Id != model.Id)
                .AllAsync(x => x.Name != name, cancellationToken);
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

            var entity = await _context.ProductSubCategories
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductSubCategory), request.Id);
            }

            entity.Name = request.Data.Name;
            entity.Description = request.Data.Description;
            entity.ProductCategoryId = request.Data.ProductCategoryId;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}