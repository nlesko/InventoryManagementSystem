using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystemApi.API.Common;
using InventoryManagementSystemApi.API.Contracts.Warehouses;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.Warehouses;

public static class GetWarehouses
{
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