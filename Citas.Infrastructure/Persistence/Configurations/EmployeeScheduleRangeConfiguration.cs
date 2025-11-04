using Citas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Citas.Infrastructure.Persistence.Configurations;
internal class EmployeeScheduleRangeConfiguration : IEntityTypeConfiguration<EmployeeScheduleRange>
{
    public void Configure(EntityTypeBuilder<EmployeeScheduleRange> builder)
    {
        builder.ToTable("employee_schedule_range");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnType("integer")
            .HasColumnName("id");

        builder.Property(x => x.Day)
            .HasColumnName("day")
            .HasColumnType("enum__day")
            .IsRequired();

        builder.Property(x => x.StartTime)
            .HasColumnName("start_time")
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.EndTime)
            .HasColumnName("end_time")
            .HasColumnType("time")
            .IsRequired();

        // EmployeeSchedule relation
        builder.HasOne(x => x.Schedule)
               .WithMany()
               .HasForeignKey("employee_schedule_id")
               .IsRequired();

        builder.HasIndex("employee_schedule_id").HasDatabaseName("fk__employee_schedule_range_employee_schedule");
    }
}

