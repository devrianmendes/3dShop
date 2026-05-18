using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    /// <summary>
    /// Configuração de mapeamento da entidade User para o banco de dados.
    /// Define relacionamentos com Orders, OrderStatusHistory e Cart.
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.UserRole)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.UpdatedAt)
                .IsRequired();

            builder.HasMany(u => u.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.OrderStatusHistory)
                .WithOne(o => o.User)
                .HasForeignKey(u => u.ChangedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Cart)
                .WithOne(c => c.Customer)
                .HasForeignKey<Cart>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(p => p.Cart).AutoInclude(false);
        }
    }
}
