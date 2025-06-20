using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Infrastructure.DataAccess.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("NVARCHAR(255)");

        builder.Property(c => c.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnType("NVARCHAR(255)");

        builder.Property(c => c.Phone)
            .HasMaxLength(20)
            .HasColumnType("NVARCHAR(20)");

        builder.Property(c => c.Status)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnType("datetime2");

        builder.Property(c => c.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnType("datetime2");

        builder.HasIndex(c => c.Email)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Email");

        builder.HasIndex(c => c.Status)
            .HasDatabaseName("IX_Customers_Status");

        builder.HasMany(c => c.Devices)
            .WithOne(d => d.Customer)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Devices_Customers");

        builder.Navigation(c => c.Devices)
            .EnableLazyLoading(false);
    }
}