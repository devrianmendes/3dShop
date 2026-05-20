using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("carts");

            builder.HasKey(c => c.Id);

            builder.HasIndex(c => c.CustomerId)
                .IsUnique();                

            builder.HasOne(c => c.Customer)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Cart_User_UserId");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp");
        }
    }
}
