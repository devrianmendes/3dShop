using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    public class OrderItemConfiguration : BaseEntityConfiguration<OrderItem>
    {
        public override void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_items");

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

            builder.HasIndex(oi => oi.OrderId);
            builder.HasIndex(oi => oi.ProductId);

            builder.Property(oi => oi.ProductNameSnapshot)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)");

            builder.Property(oi => oi.UnitPrice)
                .IsRequired()
                .HasPrecision(10,2)
                .HasColumnType("numeric(10,2)");

            builder.Property(oi => oi.Quantity)
                .IsRequired()
                .HasColumnType("int");

            builder.Property(oi => oi.ItemTotal)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasColumnType("numeric(10,2)")
                .HasComputedColumnSql("unit_price * quantity", stored: true);

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItemList)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItem_Order_OrderId");

            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItemList)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItem_Product_ProductId");

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;

        }
    }
}
