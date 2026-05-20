using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("order_status_history");
            builder.HasKey(os => os.Id);

            builder.Property(os => os.FromStatus)
                .HasMaxLength(30)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(30)");

            builder.Property(os => os.ToStatus)
                .IsRequired()
                .HasMaxLength(30)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(30)");

            builder.Property(os => os.Notes)
                .HasColumnType("text");

            builder.Property(os => os.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.HasOne(os => os.Order)
                .WithMany(o => o.OrderStatusHistory)
                .HasForeignKey(os => os.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderStatusHistory_Order_OrderId");

            builder.HasOne(os => os.User)
                .WithMany(u => u.OrderStatusHistory)
                .HasForeignKey(os => os.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderStatusHistory_User_UserId");

            builder.Navigation(os => os.User).AutoInclude(false);
            builder.Navigation(os => os.Order).AutoInclude(false);
        }
    }
}
