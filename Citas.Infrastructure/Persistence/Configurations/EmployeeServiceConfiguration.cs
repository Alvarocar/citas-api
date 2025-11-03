using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Citas.Domain.Entities;

namespace Citas.Infrastructure.Persistence.Configurations;

internal class EmployeeServiceConfiguration : IEntityTypeConfiguration<EmployeeService>
{
    public void Configure(EntityTypeBuilder<EmployeeService> builder)
    {
        builder.ToTable("employee_service");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasColumnType("integer")
            .HasColumnName("id");

        builder.Property(x => x.Rating)
            .HasColumnType("real")
            .HasColumnName("rating");

        builder.HasOne(x => x.Employee)
               .WithMany()
               .HasForeignKey("employee_id")
               .IsRequired();

        builder.HasOne(x => x.Service)
               .WithMany()
               .HasForeignKey("service_id")
               .IsRequired();

        builder.HasIndex("service_id").HasDatabaseName("fk__employee_service_service");
        builder.HasIndex("employee_id").HasDatabaseName("fk__employee_service_employee");
    }
}