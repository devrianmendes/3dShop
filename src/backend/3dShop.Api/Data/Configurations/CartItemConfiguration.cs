using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    //Arquivos de configuração só serão implementados no sistema quando configurado no AppDbContext
    public class CartItemConfiguration : BaseEntityConfiguration<CartItem>
    {
        public override void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("cart_items");

            base.Configure(builder);//Chama id, createdAt e updatedAt

            builder.HasIndex(ci => ci.CartId);
            builder.HasIndex(ci => ci.ProductId);

            builder.Property(ci => ci.ProductNameSnapshot)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)");

            builder.Property(ci => ci.UnitPrice)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasColumnType("numeric(10,2)");

            builder.Property(ci => ci.Quantity)
                .IsRequired()
                .HasColumnType("int");

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItemList)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CartItem_Cart_CartId");

            builder.HasOne(ci => ci.Product)
                .WithMany(c => c.CartItemList)
                .HasForeignKey(ci => ci.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CartItem_Product_ProductId");

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;

        }
    }
}
