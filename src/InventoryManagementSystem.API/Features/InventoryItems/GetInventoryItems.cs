using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.InventoryItems;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public static class GetInventoryItems
{

    public class GetInventoryItemsMapping : IMapFrom<InventoryItem>
    {
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Domain.Entities.InventoryItem, InventoryItemsResult>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
                .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.Warehouse.Location))
                .ForMember(dest => dest.WarehouseManager, opt => opt.MapFrom(src => $"{src.Warehouse.Manager.FirstName} {src.Warehouse.Manager.LastName}"))
                .ForMember(dest => dest.WarehouseEmailAddress, opt => opt.MapFrom(src => src.Warehouse.EmailAddress))
                .ForMember(dest => dest.WarehousePhoneNumber, opt => opt.MapFrom(src => src.Warehouse.PhoneNumber));
        }
    }

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