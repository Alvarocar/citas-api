using Citas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class RolConfiguration : IEntityTypeConfiguration<Rol>
{
  public void Configure(EntityTypeBuilder<Rol> builder)
  {
    builder.ToTable("rol");

    builder.HasKey(x => x.Id);
    builder.Property(x => x.Id)
        .ValueGeneratedOnAdd()
        .HasColumnType("integer")
        .HasColumnName("id");

    builder.Property(x => x.Name)
        .HasColumnName("name")
        .HasColumnType("varchar(100)")
        .HasMaxLength(100);
  }
}