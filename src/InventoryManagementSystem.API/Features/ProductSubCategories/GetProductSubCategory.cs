using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.ProductSubCategories;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.ProductCategories;

public static class GetProductSubCategory
{

    public class GetProductSubCategoryMapping : IMapFrom<ProductSubCategory>
    {
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<ProductSubCategory, ProductSubCategoryResult>();
        }
    }

    public record Query(int Id) : IRequest<ProductSubCategoryResult>;

    internal sealed class Handler : IRequestHandler<Query, ProductSubCategoryResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductSubCategoryResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.ProductSubCategories
                .Where(x => x.Id == request.Id)
                .ProjectTo<ProductSubCategoryResult>(_mapper.ConfigurationProvider, new { id = request.Id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductSubCategory), request.Id);
            }

            return entity;
        }
    }
}