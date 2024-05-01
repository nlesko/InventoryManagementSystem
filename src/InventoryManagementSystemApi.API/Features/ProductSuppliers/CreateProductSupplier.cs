using FluentValidation;

using InventoryManagementSystemApi.API.Contracts.ProductSuppliers;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.ProductSuppliers;

public static class CreateProductSupplier
{
    public record Command(ProductSupplierRequest Data) : IRequest<int>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Data.Name).NotNull().NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
            RuleFor(x => x.Data.Address).MaximumLength(512);
            RuleFor(x => x.Data.ContactPerson).MaximumLength(128);
            RuleFor(x => x.Data.EmailAddress).MaximumLength(128);
            RuleFor(x => x.Data.PhoneNumber).MaximumLength(32);
            RuleFor(x => x.Data.Website).MaximumLength(512);
        }
        private Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return _context.ProductSuppliers
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

            var entity = new ProductSupplier
            {
                Name = request.Data.Name,
                Address = request.Data.Address,
                ContactPerson = request.Data.ContactPerson,
                EmailAddress = request.Data.EmailAddress,
                PhoneNumber = request.Data.PhoneNumber,
                Website = request.Data.Website
            };

            _context.ProductSuppliers.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}