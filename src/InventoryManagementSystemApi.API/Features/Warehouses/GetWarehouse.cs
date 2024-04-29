using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystemApi.API.Common.Exceptions;
using InventoryManagementSystemApi.API.Contracts.Warehouses;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.Warehouses;

public static class GetWarehouse
{
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
                }).FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(GetWarehouse), request.Id);
            }

            return entity;
        }
    }
}