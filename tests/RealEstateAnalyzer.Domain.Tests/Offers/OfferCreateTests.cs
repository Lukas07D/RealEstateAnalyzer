using RealEstateAnalyzer.Domain.Common;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Domain.Tests.Offers;

public class OfferCreateTests
{
    [Fact]
    public void Create_WithValidData_SetsProperties()
    {
        var offer = OfferTestData.CreateOffer();

        Assert.NotEqual(Guid.Empty, offer.Id);
        Assert.Equal(OfferTestData.Now, offer.CreatedAt);
        Assert.Equal(500_000m, offer.AskingPrice.Amount);
        Assert.Equal(50m, offer.Area.SquareMeters);
        Assert.Equal(2, offer.Rooms);
    }

    [Fact]
    public void Create_WithZeroPrice_Throws()
    {
        // Money dopuszcza 0 (np. AdminFee), więc cenę > 0 pilnuje dopiero Offer.
        var ex = Assert.Throws<DomainException>(() => OfferTestData.CreateOffer(askingPrice: 0m));

        Assert.Contains("Cena", ex.Message);
    }

    [Fact]
    public void Create_WithNegativePrice_Throws()
    {
        // Ujemną kwotę odrzuca już Money.
        var ex = Assert.Throws<DomainException>(() => OfferTestData.CreateOffer(askingPrice: -1m));

        Assert.Contains("Kwota", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_WithAreaNotGreaterThanZero_Throws(int area)
    {
        var ex = Assert.Throws<DomainException>(() => OfferTestData.CreateOffer(area: area));

        Assert.Contains("Metraż", ex.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithLessThanOneRoom_Throws(int rooms)
    {
        var ex = Assert.Throws<DomainException>(() => OfferTestData.CreateOffer(rooms: rooms));

        Assert.Contains("pokoi", ex.Message);
    }

    [Fact]
    public void Create_WithYearBuiltMoreThanFiveYearsInFuture_Throws()
    {
        var tooLate = OfferTestData.CurrentYear + YearBuilt.MaxYearsAhead + 1;

        var ex = Assert.Throws<DomainException>(() => OfferTestData.CreateOffer(yearBuilt: tooLate));

        Assert.Contains("Rok budowy", ex.Message);
    }

    [Fact]
    public void Create_WithYearBuiltExactlyFiveYearsInFuture_IsAllowed()
    {
        var maxAllowed = OfferTestData.CurrentYear + YearBuilt.MaxYearsAhead;

        var offer = OfferTestData.CreateOffer(yearBuilt: maxAllowed);

        Assert.Equal(maxAllowed, offer.YearBuilt.Value);
    }
}
