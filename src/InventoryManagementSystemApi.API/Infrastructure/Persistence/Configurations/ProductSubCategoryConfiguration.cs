using InventoryManagementSystemApi.API.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagementSystemApi.API.Infrastructure.Persistence.Configurations;

public class ProductSubCategoryConfiguration : IEntityTypeConfiguration<ProductSubCategory>
{
    public void Configure(EntityTypeBuilder<ProductSubCategory> builder)
    {
        builder.ToTable("ProductSubCategories");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Description)
            .HasMaxLength(1024);

        builder.Property(e => e.ProductCategoryId)
            .IsRequired();

        builder.HasOne(e => e.ProductCategory)
            .WithMany(e => e.ProductSubCategories)
            .HasForeignKey(e => e.ProductCategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}