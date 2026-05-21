using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    public class OrderConfiguration : BaseEntityConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

            builder.HasIndex(o => o.OrderNumber)
                .IsUnique();

            builder.HasIndex(o => o.CustomerId);
            builder.HasIndex(o => o.Status);
            builder.HasIndex(o => o.CreatedAt);

            builder.Property(o => o.OrderNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnType("VARCHAR(20)");

            builder.Property(o => o.Status)
                .IsRequired()
                .HasMaxLength(30)
                .HasConversion<string>()
                .HasColumnType("VARCHAR(30)");

            // OwnsOne instrui o EF Core que ShippingAddress não é uma tabela separada,
            // mas sim um conjunto de colunas embutidas na própria tabela "orders"
            builder.OwnsOne(o => o.Address, address =>
            {
                address.Property(a => a.ShippingStreet)
                    .IsRequired().HasMaxLength(200).HasColumnType("VARCHAR(200)");

                address.Property(a => a.ShippingNumber)
                    .IsRequired().HasMaxLength(20).HasColumnType("VARCHAR(20)");

                address.Property(a => a.ShippingComplement)
                    .HasMaxLength(100).HasColumnType("VARCHAR(100)");

                address.Property(a => a.ShippingNeighborhood)
                    .IsRequired().HasMaxLength(100).HasColumnType("VARCHAR(100)");

                address.Property(a => a.ShippingCity)
                    .IsRequired().HasMaxLength(100).HasColumnType("VARCHAR(100)");

                address.Property(a => a.ShippingState)
                    .IsRequired().HasMaxLength(2).HasColumnType("VARCHAR(2)");

                address.Property(a => a.ShippingZipCode)
                    .IsRequired().HasMaxLength(9).HasColumnType("VARCHAR(9)");
            });

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

            builder.HasOne(o => o.Customer)
                .WithMany(u => u.OrderList)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Order_Customer_CustomerId");

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;

        }
    }
}
