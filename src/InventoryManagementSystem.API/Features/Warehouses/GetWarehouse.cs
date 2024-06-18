using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.Warehouses;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.Warehouses;

public static class GetWarehouse
{

    public class GetWarehouseMapping : IMapFrom<Warehouse>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Warehouse, WarehouseResult>();
        }
    }

    public record Query(int Id) : IRequest<WarehouseResult> { }

    internal sealed class Handler : IRequestHandler<Query, WarehouseResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<WarehouseResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.Warehouses
                .Where(x => x.Id == request.Id)
                .ProjectTo<WarehouseResult>(_mapper.ConfigurationProvider, new { id = request.Id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(GetWarehouse), request.Id);
            }

            return entity;
        }
    }
}