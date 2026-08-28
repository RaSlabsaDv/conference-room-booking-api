using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookingStatus) 
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(b => b.StartTime).IsRequired();
        builder.Property(b => b.EndTime).IsRequired();

        builder.ComplexProperty(b => b.TotalPrice, price =>
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

        builder.HasOne<Room>()
            .WithMany()
            .HasForeignKey(b => b.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsMany(b => b.SelectedServices, sb =>
        {
            sb.ToTable("booking_services");

            sb.WithOwner().HasForeignKey("BookingId");

            sb.HasKey("BookingId", nameof(BookedService.ServiceId)); // композитний ключ

            sb.Property(s => s.ServiceId).IsRequired();
            sb.Property(s => s.Name).IsRequired().HasMaxLength(200);

            sb.OwnsOne(s => s.Price, price =>
            {
                price.Property(m => m.Amount)
                    .HasColumnName("PriceAmount")
                    .HasColumnType("decimal(18,2)")
                    .IsRequired();

                price.Property(m => m.Currency)
                    .HasColumnName("PriceCurrency")
                    .HasMaxLength(3)
                    .IsRequired();
            });
        });

        builder.Navigation(b => b.SelectedServices)
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasField("_selectedServices");
    }
}