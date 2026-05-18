using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("order_item");
            builder.HasKey(oi => oi.Id);

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
                .HasColumnType("numeric(10,2)");

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItemList)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItem_Order_OrderId");

            builder.HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItemList)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderItem_Product_ProductItem");

            builder.Navigation(oi => oi.Order).AutoInclude(false);
            builder.Navigation(oi => oi.Product).AutoInclude(false);
        }
    }
}
