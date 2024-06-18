using FluentValidation;

using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.ProductCategories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public static class CreateProductCategory
{
    public record Command(ProductCategoryRequest Data) : IRequest<int>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Data.Name).NotNull().NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
        }
        private Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return _context.ProductCategories
                .AllAsync(x => x.Name != name, cancellationToken);
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

            var entity = new ProductCategory
            {
                Name = request.Data.Name,
                Description = request.Data.Description
            };

            _context.ProductCategories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}