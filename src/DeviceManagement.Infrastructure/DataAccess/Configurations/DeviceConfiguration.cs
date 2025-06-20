using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Infrastructure.DataAccess.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("Devices");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Serial)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("NVARCHAR(100)");

        builder.Property(d => d.IMEI)
            .IsRequired()
            .HasMaxLength(15)
            .HasColumnType("NVARCHAR(15)");

        builder.Property(d => d.ActivationDate)
            .HasColumnType("datetime2");

        builder.Property(d => d.CustomerId)
            .IsRequired();

        builder.Property(d => d.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnType("datetime2");

        builder.Property(d => d.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnType("datetime2");

        builder.HasIndex(d => d.Serial)
            .IsUnique()
            .HasDatabaseName("IX_Devices_Serial");

        builder.HasIndex(d => d.IMEI)
            .IsUnique()
            .HasDatabaseName("IX_Devices_IMEI");

        builder.HasIndex(d => d.CustomerId)
            .HasDatabaseName("IX_Devices_CustomerId");

        builder.HasIndex(d => d.ActivationDate)
            .HasDatabaseName("IX_Devices_ActivationDate");

        builder.HasOne(d => d.Customer)
            .WithMany(c => c.Devices)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Devices_Customers");

        builder.HasMany(d => d.Events)
            .WithOne(e => e.Device)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Events_Devices");

        builder.Navigation(d => d.Customer)
            .EnableLazyLoading(false);

        builder.Navigation(d => d.Events)
            .EnableLazyLoading(false);
    }
}
