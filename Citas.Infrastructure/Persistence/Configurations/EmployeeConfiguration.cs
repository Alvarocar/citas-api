using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Citas.Domain.Entities;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("employee");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.LastName)
            .HasColumnName("last_name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        builder.Property(x => x.HasAccount)
            .HasColumnName("has_account")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .HasMaxLength(200);

        builder.Property(x => x.Password)
            .HasColumnName("password")
            .HasMaxLength(200);

        // Rol relation (required) -> shadow fk rol_id
        builder.HasOne(x => x.Rol)
               .WithMany()
               .HasForeignKey("rol_id")
               .IsRequired();

        // Company relation (required) -> shadow fk company_id
        builder.HasOne(x => x.Company)
               .WithMany()
               .HasForeignKey("company_id")
               .IsRequired();

        // Position optional -> shadow fk position_id
        builder.HasOne(x => x.Position)
               .WithMany()
               .HasForeignKey("position_id")
               .IsRequired(false);

        // EmployeeSchedule relation (required) -> shadow fk employee_schedule_id
        builder.HasOne(x => x.Schedule)
               .WithMany()
               .HasForeignKey("employee_schedule_id")
               .IsRequired();

        builder.HasIndex("rol_id").HasDatabaseName("fk__employee_rol");
        builder.HasIndex("company_id").HasDatabaseName("fk__employee_company");
        builder.HasIndex("position_id").HasDatabaseName("fk__employee_position");
        builder.HasIndex("employee_schedule_id").HasDatabaseName("fk__employee_employee_schedule");
    }
}