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
                .HasMaxLength(150)
                .HasColumnType("VARCHAR(150)");

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("VARCHAR(255)");
            
            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnType("VARCHAR(255)");
            ;

            builder.Property(u => u.UserRole)
                .IsRequired()
                .HasConversion<string>()
                .HasColumnType("VARCHAR(20)")
                .HasMaxLength(20);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");
           
            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp");
        }
    }
}
