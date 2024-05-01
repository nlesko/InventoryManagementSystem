using InventoryManagementSystemApi.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystemApi.API.Infrastructure.Persistence.Configurations;

public class ProductSupplierConfiguration : IEntityTypeConfiguration<ProductSupplier>
{
    public void Configure(EntityTypeBuilder<ProductSupplier> builder)
    {
        builder.ToTable("ProductSuppliers");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.EmailAddress)
            .HasMaxLength(128);
        
        builder.Property(e => e.PhoneNumber)
            .HasMaxLength(32);
        
        builder.Property(e => e.Address)
            .HasMaxLength(512);
        
        builder.Property(e => e.ContactPerson)
            .HasMaxLength(128);
        
        builder.Property(e => e.Website)
            .HasMaxLength(512);
    }
}