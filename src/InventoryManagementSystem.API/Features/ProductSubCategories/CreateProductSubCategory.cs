using FluentValidation;

using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.ProductSubCategories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public static class CreateProductSubCategory
{
    public record Command(ProductSubCategoryRequest Data) : IRequest<int>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Data.Name).NotNull().NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
            RuleFor(x => x.Data.ProductCategoryId).GreaterThan(0);
        }
        private Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return _context.ProductSubCategories
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

            var entity = new ProductSubCategory
            {
                Name = request.Data.Name,
                Description = request.Data.Description,
                ProductCategoryId = request.Data.ProductCategoryId
            };

            _context.ProductSubCategories.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}