using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.ProductSuppliers;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductSuppliers;

public static class GetProductSuppliers
{

    public class GetProductSuppliersMapping : IMapFrom<ProductSupplier>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductSupplier, ProductSuppliersResult>();
        }
    }

    public record Query : IRequest<List<ProductSuppliersResult>>;

    internal sealed class Handler : IRequestHandler<Query, List<ProductSuppliersResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductSuppliersResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entities = await _context.ProductSuppliers
                .ProjectTo<ProductSuppliersResult>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return entities;
        }
    }
}