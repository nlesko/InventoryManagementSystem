using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Contracts.ProductSubCategories;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductSubCategories;

public static class GetProductSubCategories
{
    public record Query : IRequest<List<ProductSubCategoriesResult>>;

    internal sealed class Handler : IRequestHandler<Query, List<ProductSubCategoriesResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductSubCategoriesResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entities = await _context.ProductSubCategories
                .ProjectTo<ProductSubCategoriesResult>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return entities;
        }
    }
}