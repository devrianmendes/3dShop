using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("products");

            builder.HasKey(p => p.Id);

            builder.HasIndex(p => p.Category);

            builder.Property(p => p.NamePt)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)");

            builder.Property(p => p.NameEn)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("VARCHAR(200)");

            builder.Property(p => p.DescriptionPt)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("text");

            builder.Property(p => p.DescriptionEn)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("text");

            builder.Property(p => p.Price)
                .IsRequired()
                .HasPrecision(10, 2)
                .HasColumnType("numeric(10,2)");

            builder.Property(p => p.IsCustom)
                .IsRequired();

            builder.Property(p => p.IsActive)
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.Property(p => p.UpdatedAt)
                .IsRequired()
                .HasColumnType("timestamp");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.ProductList)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Products_Categories_CategoryId");

            builder.Navigation(p => p.Category)
                .AutoInclude(false);
        }
    }
}
