using FluentValidation;

using InventoryManagementSystemApi.API.Common.Exceptions;
using InventoryManagementSystemApi.API.Contracts.Warehouses;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.Warehouses;

public static class UpdateWarehouse
{
    public record Command(int Id, WarehouseRequest Data) : IRequest<int> { }

    public sealed class Validator : AbstractValidator<Command>
    {
        private readonly ApplicationDbContext _context;
        public Validator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Data.Name).NotNull().NotEmpty().MustAsync(BeUniqueName).WithMessage("The specified name already exists.");
            RuleFor(x => x.Data.Location).NotNull().NotEmpty();
            RuleFor(x => x.Data.EmailAddress).NotNull().NotEmpty();
            RuleFor(x => x.Data.PhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.Data.Capacity).GreaterThan(0);
            RuleFor(x => x.Data.ManagerId).GreaterThan(0);
        }
        private Task<bool> BeUniqueName(Command model, string name, CancellationToken cancellationToken)
        {
            return _context.Warehouses
                .Where(x => x.Id != model.Id)
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

            var entity = await _context.Warehouses
                                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Warehouse), request.Id);
            }

            entity.Name = request.Data.Name;
            entity.Location = request.Data.Location;
            entity.EmailAddress = request.Data.EmailAddress;
            entity.PhoneNumber = request.Data.PhoneNumber;
            entity.Capacity = request.Data.Capacity;
            entity.ManagerId = request.Data.ManagerId;

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}