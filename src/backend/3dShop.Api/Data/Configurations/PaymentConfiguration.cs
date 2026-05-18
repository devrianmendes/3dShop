using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");
            builder.HasKey(p => p.Id);
            builder.HasIndex(p => p.OrderId)
                .IsUnique();

            builder.Property(p => p.Method)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(p => p.GatewayPaymentId)
                .HasMaxLength(100)
                .HasColumnType("VARCHAR(100)");

            builder.Property(p => p.GatewayPreferenceId)
               .HasMaxLength(100)
               .HasColumnType("VARCHAR(100)");

            builder.Property(p => p.Amount)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasColumnType("numeric(10,2)");

            builder.Property(p => p.PaidAt)
                .HasColumnType("timestamp");

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Payment_Order_OrderId");

            builder.Navigation(p => p.Order).AutoInclude(false);
        }
    }
}
