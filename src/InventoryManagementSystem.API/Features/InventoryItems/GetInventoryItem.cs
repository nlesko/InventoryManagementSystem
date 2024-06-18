using AutoMapper;
using AutoMapper.QueryableExtensions;

using InventoryManagementSystem.API.Common.Exceptions;
using InventoryManagementSystem.API.Common.Mappings;
using InventoryManagementSystem.API.Domain.Entities;
using InventoryManagementSystem.API.Infrastructure.Persistence;
using InventoryManagementSystem.Shared.Contracts.InventoryItems;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace InventoryManagementSystem.API.Features.InventoryItems;

public static class GetInventoryItem
{

    public class GetInventoyrItemMapping : IMapFrom<InventoryItem>
    {
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<InventoryItem, InventoryItemResult>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
                .ForMember(dest => dest.WarehouseLocation, opt => opt.MapFrom(src => src.Warehouse.Location))
                .ForMember(dest => dest.WarehouseManager, opt => opt.MapFrom(src => $"{src.Warehouse.Manager.FirstName} {src.Warehouse.Manager.LastName}"))
                .ForMember(dest => dest.WarehouseEmailAddress, opt => opt.MapFrom(src => src.Warehouse.EmailAddress))
                .ForMember(dest => dest.WarehousePhoneNumber, opt => opt.MapFrom(src => src.Warehouse.PhoneNumber));
        }
    }

    public record Query(int Id) : IRequest<InventoryItemResult>;

    internal sealed class Handler : IRequestHandler<Query, InventoryItemResult>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public Handler(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InventoryItemResult> Handle(Query request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            var entity = await _context.InventoryItems
                .ProjectTo<InventoryItemResult>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(InventoryItem), request.Id);
            }

            return entity;
        }
    }
}