using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Citas.Domain.Entities;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("service");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(x => x.SuggestedPrice)
            .HasColumnName("suggested_price")
            .IsRequired();

        builder.Property(x => x.IsUnavailable)
            .HasColumnName("is_unavailable")
            .IsRequired();

        // Company relation (shadow FK: company_id)
        builder.HasOne(x => x.Company)
               .WithMany()
               .HasForeignKey("company_id")
               .IsRequired();

        // Indexes

        builder.HasIndex("company_id").HasDatabaseName("fk__service_company");
    }
}