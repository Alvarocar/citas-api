using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Citas.Domain.Entities;
using Citas.Infrastructure.Persistence.Converters;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class EmployeeScheduleExceptionConfiguration : IEntityTypeConfiguration<EmployeeScheduleException>
{
    public void Configure(EntityTypeBuilder<EmployeeScheduleException> builder)
    {
        builder.ToTable("employee_schedule_exception");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");
        
        builder.Property(x => x.RangeTime)
            .HasColumnName("range_time")
            .HasColumnType("tstzrange")
            .HasConversion<DateTimeRangeConverter>()
            .IsRequired();

        builder.Property(x => x.Comment)
            .HasColumnName("comment")
            .HasMaxLength(1000)
            .IsRequired();

        // Employee relation (required) -> shadow fk employee_id
        builder.HasOne(x => x.Employee)
               .WithMany()
               .HasForeignKey("employee_id")
               .IsRequired();

        builder.HasIndex("employee_id").HasDatabaseName("fk__employee_schedule_exception_employee");
    }
}