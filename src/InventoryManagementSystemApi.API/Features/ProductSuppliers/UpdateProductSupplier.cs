using FluentValidation;

using InventoryManagementSystemApi.API.Common.Exceptions;
using InventoryManagementSystemApi.API.Contracts.ProductSuppliers;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.ProductSuppliers;

public static class UpdateProductSupplier
{
    public record Command(int Id, ProductSupplierRequest Data) : IRequest<Unit>;

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Data).NotNull();
            RuleFor(x => x.Data.Name).NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");;
            RuleFor(x => x.Data.Address).MaximumLength(512);
            RuleFor(x => x.Data.ContactPerson).MaximumLength(128);
            RuleFor(x => x.Data.EmailAddress).MaximumLength(128);
            RuleFor(x => x.Data.PhoneNumber).MaximumLength(32);
            RuleFor(x => x.Data.Website).MaximumLength(512);
        }

        private Task<bool> BeUniqueName(Command model, string name, CancellationToken cancellationToken)
        {
            return _context.ProductSuppliers
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

            var entity = await _context.ProductSuppliers
                .Where(x => x.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductSupplier), request.Id);
            }

            entity.Name = request.Data.Name;
            entity.Address = request.Data.Address;
            entity.ContactPerson = request.Data.ContactPerson;
            entity.EmailAddress = request.Data.EmailAddress;
            entity.PhoneNumber = request.Data.PhoneNumber;
            entity.Website = request.Data.Website;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}