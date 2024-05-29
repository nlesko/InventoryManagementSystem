using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Contracts.ProductCategories;
using InventoryManagementSystem.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public static class GetProductCategories
{
    public record Query : IRequest<List<ProductCategoriesResult>>;

    internal sealed class Handler : IRequestHandler<Query, List<ProductCategoriesResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductCategoriesResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entities = await _context.ProductCategories
                .ProjectTo<ProductCategoriesResult>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return entities;
        }
    }
}