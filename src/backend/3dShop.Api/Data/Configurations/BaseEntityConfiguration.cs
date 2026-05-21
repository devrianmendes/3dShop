using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    //Recebe genérico (todas as outras entidades) e diz que todos os recebidos irão herdar baseEntity
    //Arquivos de configuração só serão implementados no sistema quando configurado no AppDbContext
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        { 
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamptz");

            builder.Property(e => e.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamptz");
        }
    }
}
