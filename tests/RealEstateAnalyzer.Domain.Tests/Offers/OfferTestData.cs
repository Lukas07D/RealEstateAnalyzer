using RealEstateAnalyzer.Domain.Offers;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Domain.Tests.Offers;

internal static class OfferTestData
{
    public const int CurrentYear = 2026;
    public static readonly DateTimeOffset Now = new(2026, 9, 30, 12, 0, 0, TimeSpan.Zero);

    public static Offer CreateOffer(
        decimal askingPrice = 500_000m,
        decimal area = 50m,
        int rooms = 2,
        int yearBuilt = 1965,
        decimal adminFee = 650m) =>
        Offer.Create(
            Address.Create("ul. Marszałkowska 10", "Warszawa", "Śródmieście"),
            Area.Create(area),
            Money.Create(askingPrice),
            rooms,
            Floor.Create(3),
            YearBuilt.Create(yearBuilt, CurrentYear),
            Money.Create(adminFee),
            ListingUrl.Create("https://example.com/oferta/123"),
            Now);
}
