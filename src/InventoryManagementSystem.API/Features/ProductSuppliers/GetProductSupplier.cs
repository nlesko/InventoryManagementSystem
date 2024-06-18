using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.ProductSuppliers;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductSuppliers;

public static class GetProductSupplier
{

    public class GetProductSupplierMapping : IMapFrom<ProductSupplier>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductSupplier, ProductSupplierResult>();
        }
    }

    public record Query(int Id) : IRequest<ProductSupplierResult>;

    internal sealed class Handler : IRequestHandler<Query, ProductSupplierResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductSupplierResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.ProductSuppliers
                .Where(x => x.Id == request.Id)
                .ProjectTo<ProductSupplierResult>(_mapper.ConfigurationProvider, new { id = request.Id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductSupplier), request.Id);
            }

            return entity;
        }
    }
}