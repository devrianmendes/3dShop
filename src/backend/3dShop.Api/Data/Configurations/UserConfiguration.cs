using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    public class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

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

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasColumnType("boolean")
                .HasDefaultValue(true);

            builder.Property(u => u.UserRole)
                .IsRequired()
                .HasConversion<string>() //Este campo é um enum, é necessário informar qual o tipo de dado contido no enum;
                .HasColumnType("VARCHAR(20)")
                .HasMaxLength(20);

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;
        }
    }
}
