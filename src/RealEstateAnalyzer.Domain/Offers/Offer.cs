using RealEstateAnalyzer.Domain.Common;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Domain.Offers;

public sealed class Offer
{
    public Guid Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Address Address { get; private set; } = null!;
    public Area Area { get; private set; } = null!;
    public Money AskingPrice { get; private set; } = null!;
    public int Rooms { get; private set; }
    public Floor Floor { get; private set; } = null!;
    public YearBuilt YearBuilt { get; private set; } = null!;
    public Money AdminFee { get; private set; } = null!;
    public ListingUrl? ListingUrl { get; private set; }
    public RentalAssumptions? RentalAssumptions { get; private set; }

    // Dla EF Core.
    private Offer() { }

    public static Offer Create(
        Address address,
        Area area,
        Money askingPrice,
        int rooms,
        Floor floor,
        YearBuilt yearBuilt,
        Money adminFee,
        ListingUrl? listingUrl,
        DateTimeOffset createdAt)
    {
        ArgumentNullException.ThrowIfNull(address);
        ArgumentNullException.ThrowIfNull(area);
        ArgumentNullException.ThrowIfNull(askingPrice);
        ArgumentNullException.ThrowIfNull(floor);
        ArgumentNullException.ThrowIfNull(yearBuilt);
        ArgumentNullException.ThrowIfNull(adminFee);

        if (askingPrice.Amount <= 0)
            throw new DomainException("Cena musi być większa od 0.");
        if (rooms < 1)
            throw new DomainException("Liczba pokoi musi wynosić co najmniej 1.");

        return new Offer
        {
            Id = Guid.CreateVersion7(createdAt),
            CreatedAt = createdAt,
            Address = address,
            Area = area,
            AskingPrice = askingPrice,
            Rooms = rooms,
            Floor = floor,
            YearBuilt = yearBuilt,
            AdminFee = adminFee,
            ListingUrl = listingUrl,
        };
    }

    public Money PricePerSquareMeter() =>
        Money.Create(Math.Round(AskingPrice.Amount / Area.SquareMeters, 2, MidpointRounding.AwayFromZero));

    public void SetRentalAssumptions(RentalAssumptions rentalAssumptions)
    {
        ArgumentNullException.ThrowIfNull(rentalAssumptions);
        RentalAssumptions = rentalAssumptions;
    }

    /// <summary>
    /// Brutto celowo bez pustostanów (porównywalne z ofertami rynkowymi).
    /// Netto: (czynsz * (12 - pustostan) - (AdminFee + OtherMonthlyCosts) * 12) / cena; może być ujemne.
    /// Zwraca null, gdy brak założeń najmu.
    /// </summary>
    public RentalYield? CalculateRentalYield()
    {
        if (RentalAssumptions is null)
            return null;

        const int monthsPerYear = 12;
        var monthlyRent = RentalAssumptions.MonthlyRent.Amount;
        var annualRent = monthlyRent * (monthsPerYear - RentalAssumptions.VacancyMonthsPerYear);
        var annualCosts = (AdminFee.Amount + RentalAssumptions.OtherMonthlyCosts.Amount) * monthsPerYear;

        var gross = monthlyRent * monthsPerYear / AskingPrice.Amount * 100;
        var net = (annualRent - annualCosts) / AskingPrice.Amount * 100;

        return new RentalYield(
            Math.Round(gross, 2, MidpointRounding.AwayFromZero),
            Math.Round(net, 2, MidpointRounding.AwayFromZero));
    }
}
