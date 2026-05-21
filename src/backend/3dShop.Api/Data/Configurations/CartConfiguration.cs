using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Herda BaseEntityConfiguration para herdar id, createdAt e updatedAt.
    //Lá, foi configurada a herança de IEntityTypeConfiguration<T>
    //Arquivos de configuração só serão implementados no sistema quando configurado no AppDbContext
    public class CartConfiguration : BaseEntityConfiguration<Cart>
    {
        public override void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.ToTable("carts"); 

            base.Configure(builder); //Chama id, createdAt e updatedAt do baseEntity

            builder.HasIndex(c => c.CustomerId)
                .IsUnique();

            builder.HasOne(c => c.Customer)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.CustomerId) //Em relações 1:1, é necessário que informe via angle brackets de qual entity vem a FK 
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Cart_User_UserId");

            //builder.Navigation(c => c.CartItem).AutoInclude(false) foi removido porque, por padrão, o autoinclude já é false;
        }
    }
}
