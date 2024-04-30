using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystemApi.API.Common.Exceptions;
using InventoryManagementSystemApi.API.Contracts.ProductCategories;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystemApi.API.Features.ProductCategories;

public static class GetProductCategory
{
    public record Query(int Id) : IRequest<ProductCategoryResult>;

    internal sealed class Handler : IRequestHandler<Query, ProductCategoryResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductCategoryResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.ProductCategories
                .Where(x => x.Id == request.Id)
                .ProjectTo<ProductCategoryResult>(_mapper.ConfigurationProvider, new { id = request.Id
                })
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(ProductCategory), request.Id);
            }

            return entity;
        }
    }
}