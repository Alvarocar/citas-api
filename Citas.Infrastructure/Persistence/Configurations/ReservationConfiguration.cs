using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Citas.Domain.Entities;
using Citas.Infrastructure.Persistence.Converters;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("reservation");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        // Client relation
        builder.HasOne(x => x.Client)
               .WithMany()
               .HasForeignKey("client_id")
               .IsRequired();

        // Employee relation
        builder.HasOne(x => x.Employee)
               .WithMany()
               .HasForeignKey("employee_id")
               .IsRequired();

        builder.Property(x => x.Price)
            .HasColumnName("price");

        builder.Property(x => x.ClientComment)
            .HasColumnName("client_comment")
            .HasMaxLength(1000);

        builder.Property(x => x.RatingFromClient)
            .HasColumnName("rating_from_client");

        builder.Property(x => x.EmployeeComment)
            .HasColumnName("employee_comment")
            .HasMaxLength(1000);

        builder.Property(x => x.RatingFromEmployee)
            .HasColumnName("rating_from_employee");

        builder.Property(x => x.State)
            .HasColumnName("state")
            .HasColumnType("enum__reservation_state")
            .IsRequired();

        builder.Property(x => x.RangeTime)
            .HasColumnName("range_time")
            .HasColumnType("tstzrange")
            .HasConversion<DateTimeRangeConverter>()
            .IsRequired();

        builder.HasIndex("client_id").HasDatabaseName("fk__reservation_client");
        builder.HasIndex("employee_id").HasDatabaseName("fk__reservation_employee");
    }
}