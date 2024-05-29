using FluentValidation;

using InventoryManagementSystem.API.Common;
using InventoryManagementSystem.API.Contracts.Warehouses;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.Warehouses;

public static class CreateWarehouse
{
    public record Command : IRequest<int>
    {
        public WarehouseRequest Data { get; }

        public Command(WarehouseRequest data)
        {
            Data = data;
        }
    }

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
        private Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return _context.Warehouses
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

            var entity = new Warehouse
            {
                Name = request.Data.Name,
                Location = request.Data.Location,
                Capacity = request.Data.Capacity,
                ManagerId = request.Data.ManagerId,
                EmailAddress = request.Data.EmailAddress,
                PhoneNumber = request.Data.PhoneNumber
            };

            await _context.Warehouses.AddAsync(entity, cancellationToken);
            // entity.AddDomainEvent(new WarehouseCreatedEvent(entity));
            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}