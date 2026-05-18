using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasColumnType("VARCHAR(500)")
                .HasMaxLength(500);

            builder.Property(pi => pi.IsMain)
                .IsRequired()
                .HasColumnType("bool")
                .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImageList)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(pi => pi.Product).AutoInclude(false);
        }
    }
}
