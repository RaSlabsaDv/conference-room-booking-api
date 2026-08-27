using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class RoomConfiguration : IEntityTypeConfiguration<Room>
{
    public void Configure(EntityTypeBuilder<Room> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(r => r.Capacity)
            .IsRequired();

        builder.Property(r => r.RoomStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.ComplexProperty(r => r.BaseHourlyRate, price =>
        {
            price.Property(m => m.Amount)
                .HasColumnName("Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            price.Property(m => m.Currency)
                .HasColumnName("Currency")
                .HasMaxLength(3)
                .IsRequired();
        });

        builder.HasMany<Service>("_services")
            .WithOne()
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.Services)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_services");

        builder.HasQueryFilter(r => r.RoomStatus != RoomStatus.Deleted);
    }
}