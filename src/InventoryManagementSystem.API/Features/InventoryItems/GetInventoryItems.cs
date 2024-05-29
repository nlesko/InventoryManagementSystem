using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Contracts.InventoryItems;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public static class GetInventoryItems
{
    public record Query : IRequest<List<InventoryItemsResult>>;

    internal sealed class Handler : IRequestHandler<Query, List<InventoryItemsResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<InventoryItemsResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entities = await _context.InventoryItems
                .ProjectTo<InventoryItemsResult>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return entities;
        }
    }
}