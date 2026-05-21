using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    //Arquivos de configuração só serão implementados no sistema quando configurado no AppDbContext
    public class OrderStatusHistoryConfiguration : BaseEntityConfiguration<OrderStatusHistory>
    {
        public override void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
        {
            builder.ToTable("order_status_history");

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

            builder.HasIndex(os => os.OrderId);

            builder.Property(os => os.FromStatus)
                .HasMaxLength(30)
                .HasConversion<string>() //Este campo é um enum, é necessário informar qual o tipo de dado contido no enum;
                .HasColumnType("VARCHAR(30)");

            builder.Property(os => os.ToStatus)
                .IsRequired()
                .HasMaxLength(30)
                .HasConversion<string>() //Este campo é um enum, é necessário informar qual o tipo de dado contido no enum;
                .HasColumnType("VARCHAR(30)");

            builder.Property(os => os.Notes)
                .HasColumnType("text");

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

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;

        }
    }
}
