using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    public class PaymentConfiguration : BaseEntityConfiguration<Payment>
    {
        public override void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

            builder.HasIndex(p => p.OrderId)
                .IsUnique();
            builder.HasIndex(p => p.GatewayPaymentId);
            builder.HasIndex(p => p.Status);

            builder.Property(p => p.Method)
                .IsRequired()
                .HasConversion<string>() //Este campo é um enum, é necessário informar qual o tipo de dado contido no enum;
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(p => p.Status)
                .IsRequired()
                .HasConversion<string>() //Este campo é um enum, é necessário informar qual o tipo de dado contido no enum;
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

            builder.HasOne(p => p.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(p => p.OrderId) //Em relações 1:1, é necessário que informe via angle brackets de qual entity vem a FK 
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Payment_Order_OrderId");

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;
        }
    }
}
