using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Citas.Domain.Entities;
using Citas.Domain.Enums;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class EmployeeScheduleConfiguration : IEntityTypeConfiguration<EmployeeSchedule>
{
    public void Configure(EntityTypeBuilder<EmployeeSchedule> builder)
    {
        builder.ToTable("employee_schedule");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .HasColumnType("integer");

        // Company relation
        builder.HasOne(x => x.Company)
               .WithMany()
               .HasForeignKey("company_id")
               .IsRequired();

        builder.HasIndex("company_id").HasDatabaseName("fk__employee_schedule_company");
    }
}