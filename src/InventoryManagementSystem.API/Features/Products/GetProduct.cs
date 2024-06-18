using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.Products;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.Products;

public static class GetProduct
{

    public class GetProductMapping : IMapFrom<Product>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Product, ProductResult>()
                .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory))
                .ForMember(dest => dest.ProductSubCategory, opt => opt.MapFrom(src => src.ProductSubCategory))
                .ForMember(dest => dest.ProductSupplier, opt => opt.MapFrom(src => src.ProductSupplier));
        }
    }

    public record Query(int Id) : IRequest<ProductResult>;

    internal sealed class Handler : IRequestHandler<Query, ProductResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.Products
                .Where(x => x.Id == request.Id)
                .ProjectTo<ProductResult>(_mapper.ConfigurationProvider, new
                {
                    id = request.Id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            return entity;
        }
    }
}