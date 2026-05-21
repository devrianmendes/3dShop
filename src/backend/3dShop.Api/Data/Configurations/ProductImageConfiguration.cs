using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    public class ProductImageConfiguration : BaseEntityConfiguration<ProductImage>
    {
        public override void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("product_images");

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

            builder.Property(pi => pi.Url)
                .IsRequired()
                .HasColumnType("VARCHAR(500)")
                .HasMaxLength(500);

            builder.Property(pi => pi.IsMain)
                .IsRequired()
                .HasColumnType("boolean")
                .HasDefaultValue(false);

            builder.Property(pi => pi.DisplayOrder)
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImageList)
                .HasForeignKey(pi => pi.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_ProductImage_Product_ProductId");

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;
        }
    }
}
