using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            builder.HasKey(o => o.Id);

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(30)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(30)");

            builder.Property(o => o.ShippingStreet)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)");

            builder.Property(o => o.ShippingNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(o => o.ShippingComplement)
                .HasMaxLength(100)
                .HasColumnType("VARCHAR(100)");

            builder.Property(o => o.ShippingNeighborhood)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("VARCHAR(100)");

            builder.Property(o => o.ShippingCity)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("VARCHAR(100)");

            builder.Property(o => o.ShippingState)
               .IsRequired()
               .HasMaxLength(2)
               .HasColumnType("VARCHAR(2)");

            builder.Property(o => o.ShippingZipCode)
               .IsRequired()
               .HasMaxLength(9)
               .HasColumnType("VARCHAR(9)");

            builder.Property(o => o.ShippingMethod)
               .IsRequired()
               .HasMaxLength(50)
               .HasColumnType("VARCHAR(50)");

            builder.Property(o => o.ShippingCost)
               .IsRequired()
               .HasPrecision(10, 2)
               .HasColumnType("numeric(10,2)");

            builder.Property(o => o.ShippingEstimatedDays)
              .IsRequired()
              .HasColumnType("int");

            builder.Property(o => o.Subtotal)
               .IsRequired()
               .HasPrecision(10, 2)
               .HasColumnType("numeric(10,2)");

            builder.Property(o => o.DiscountTotal)
               .IsRequired()
               .HasPrecision(10, 2)
               .HasColumnType("numeric(10,2)");

            builder.Property(o => o.Total)
               .IsRequired()
               .HasPrecision(10, 2)
               .HasColumnType("numeric(10,2)");

            builder.Property(o => o.Notes)
                .HasColumnType("text");

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.HasOne(o => o.Customer)
                .WithMany(u => u.OrderList)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Navigation(o => o.Customer)
                .AutoInclude(false);
        }
    }
}
