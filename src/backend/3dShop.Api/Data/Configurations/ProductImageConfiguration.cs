using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("productImages");
            builder.HasKey(pi => pi.Id);

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(pi => pi.IsMain)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                .IsRequired();

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImageList)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(pi => pi.Product).AutoInclude(false);
        }
    }
}
