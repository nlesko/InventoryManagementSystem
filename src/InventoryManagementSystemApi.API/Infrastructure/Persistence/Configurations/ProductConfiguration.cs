using InventoryManagementSystemApi.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystemApi.API.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(124);
        
        builder.Property(e => e.Description)
            .HasMaxLength(2000);

        builder.Property(e => e.SKU)
            .HasMaxLength(50);

        builder.Property(e => e.QuantityInStock)
            .IsRequired();

        builder.Property(e => e.ProductCategoryId)
            .IsRequired();

        builder.Property(e => e.ProductSupplierId)
            .IsRequired();

        builder.Property(e => e.CreatedById)
            .IsRequired();

        builder.Property(e => e.DateCreatedUtc)
            .IsRequired();

        builder.Property(e => e.LastModifiedById)
            .IsRequired();

        builder.Property(e => e.DateLastModifiedUtc)
            .IsRequired();

        builder.HasOne(e => e.ProductCategory)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.ProductCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.ProductSubCategory)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.ProductSubCategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(e => e.ProductSupplier)
            .WithMany(e => e.Products)
            .HasForeignKey(e => e.ProductSupplierId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}