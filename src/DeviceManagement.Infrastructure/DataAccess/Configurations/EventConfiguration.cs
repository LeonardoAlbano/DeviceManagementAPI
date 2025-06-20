using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeviceManagement.Domain.Entities;

namespace DeviceManagement.Infrastructure.DataAccess.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasDefaultValueSql("NEWID()")
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<int>()
            .HasComment("0=TurnedOn, 1=TurnedOff, 2=Movement, 3=SignalLoss");

        builder.Property(e => e.Observations)
            .HasMaxLength(500)
            .HasColumnType("NVARCHAR(500)");

        builder.Property(e => e.EventDateTime)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnType("datetime2");

        builder.Property(e => e.DeviceId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()")
            .HasColumnType("datetime2");

        builder.HasIndex(e => e.EventDateTime)
            .HasDatabaseName("IX_Events_EventDateTime");

        builder.HasIndex(e => e.DeviceId)
            .HasDatabaseName("IX_Events_DeviceId");

        builder.HasIndex(e => e.Type)
            .HasDatabaseName("IX_Events_Type");

        builder.HasIndex(e => new { e.DeviceId, e.EventDateTime })
            .HasDatabaseName("IX_Events_DeviceId_EventDateTime");

        builder.HasIndex(e => new { e.Type, e.EventDateTime })
            .HasDatabaseName("IX_Events_Type_EventDateTime");

        builder.HasOne(e => e.Device)
            .WithMany(d => d.Events)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Events_Devices");

        builder.Navigation(e => e.Device)
            .EnableLazyLoading(false);
    }
}
