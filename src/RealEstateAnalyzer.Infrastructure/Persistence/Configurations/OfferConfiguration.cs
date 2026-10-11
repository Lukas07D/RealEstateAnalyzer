using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstateAnalyzer.Domain.Offers;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Infrastructure.Persistence.Configurations;

internal sealed class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers");
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.Rooms).IsRequired();

        builder.ComplexProperty(o => o.Address, address =>
        {
            address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(Address.StreetMaxLength);
            address.Property(a => a.City).HasColumnName("City").HasMaxLength(Address.CityMaxLength).IsRequired();
            address.Property(a => a.District).HasColumnName("District").HasMaxLength(Address.DistrictMaxLength).IsRequired();
        });

        builder.Property(o => o.Area)
            .HasColumnName("AreaSquareMeters")
            .HasConversion(a => a.SquareMeters, v => Area.Create(v))
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.AskingPrice)
            .HasConversion(m => m.Amount, v => Money.Create(v))
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.AdminFee)
            .HasConversion(m => m.Amount, v => Money.Create(v))
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(o => o.Floor)
            .HasConversion(f => f.Value, v => Floor.Create(v))
            .IsRequired();

        // ComplexProperty zamiast konwertera: YearBuilt.Create wymaga bieżącego roku,
        // a EF materializuje VO przez prywatny konstruktor bez ponownej walidacji.
        builder.ComplexProperty(o => o.YearBuilt, year =>
            year.Property(y => y.Value).HasColumnName("YearBuilt").IsRequired());

        builder.Property(o => o.ListingUrl)
            .HasConversion(u => u!.Value.ToString(), v => ListingUrl.Create(v))
            .HasMaxLength(2048);

        // Opcjonalny VO: wszystkie kolumny NULL = brak założeń najmu.
        builder.ComplexProperty(o => o.RentalAssumptions, rental =>
        {
            rental.Property(r => r.MonthlyRent)
                .HasColumnName("RentalMonthlyRent")
                .HasConversion(m => m.Amount, v => Money.Create(v))
                .HasPrecision(18, 2);
            rental.Property(r => r.VacancyMonthsPerYear)
                .HasColumnName("RentalVacancyMonthsPerYear");
            rental.Property(r => r.OtherMonthlyCosts)
                .HasColumnName("RentalOtherMonthlyCosts")
                .HasConversion(m => m.Amount, v => Money.Create(v))
                .HasPrecision(18, 2);
        });
    }
}
