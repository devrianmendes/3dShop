using _3dShop.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _3dShop.Api.Data.Configurations
{
    public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(e => e.Token)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnType("VARCHAR(150)");

            builder.Property(e => e.ExpirationDate)
                .IsRequired()
                .HasColumnType("timestamptz");

            builder.HasIndex(c => c.UserId);

            builder.HasOne(t => t.User)
                .WithMany(u => u.RefreshToken)
                .HasForeignKey(t => t.UserId)
                .HasConstraintName("FK_RefreshToken_User_UserId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
