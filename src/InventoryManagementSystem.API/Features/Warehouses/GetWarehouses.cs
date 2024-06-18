using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common;
using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.Warehouses;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.Warehouses;

public static class GetWarehouses
{
    public class GetWarehousesMapping : IMapFrom<Warehouse>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Warehouse, WarehousesResult>();
        }
    }

    public record Query : IRequest<List<WarehousesResult>> { }

    internal sealed class Handler : IRequestHandler<Query, List<WarehousesResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<WarehousesResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var warehouses = await _context.Warehouses
                .ProjectTo<WarehousesResult>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return warehouses;
        }
    }
}