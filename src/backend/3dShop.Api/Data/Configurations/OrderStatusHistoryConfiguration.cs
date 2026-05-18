using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
    {
        public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.HasKey(os => os.Id);

            builder.Property(os => os.FromStatus)
                .HasMaxLength(30)
                .HasColumnType("VARCHAR(30)");

            builder.HasOne(os => os.Order)
                .WithMany(o => o.OrderStatusHistory)
                .HasForeignKey(os => os.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_OrderStatusHistory_Order_OrderId")
        
                    
        }

    }
}
