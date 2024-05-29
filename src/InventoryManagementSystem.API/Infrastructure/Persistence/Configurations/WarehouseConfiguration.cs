using InventoryManagementSystem.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystem.API.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.ToTable("Warehouses");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Location)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(e => e.Capacity)
            .IsRequired();

        builder.Property(e => e.ManagerId)      
            .IsRequired();
        
        builder.Property(e => e.EmailAddress)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PhoneNumber)    
            .IsRequired()
            .HasMaxLength(20);
    }
}