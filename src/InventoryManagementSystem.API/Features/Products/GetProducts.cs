using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.Products;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.Products;

public static class GetProducts
{
    public class GetProductsMapping : IMapFrom<Domain.Entities.Product>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Product, ProductsResult>()
                .ForMember(dest => dest.ProductCategory, opt => opt.MapFrom(src => src.ProductCategory.Name))
                .ForMember(dest => dest.ProductSubCategory, opt => opt.MapFrom(src => src.ProductSubCategory != null ? src.ProductSubCategory.Name : null))
                .ForMember(dest => dest.ProductSupplier, opt => opt.MapFrom(src => src.ProductSupplier.Name));
        }
    }

    public record Query : IRequest<List<ProductsResult>>;

    internal sealed class Handler : IRequestHandler<Query, List<ProductsResult>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ProductsResult>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entities = await _context.Products
                .ProjectTo<ProductsResult>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return entities;
        }
    }
}