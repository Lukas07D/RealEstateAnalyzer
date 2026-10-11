namespace RealEstateAnalyzer.Domain.Tests.Offers;

public class PricePerSquareMeterTests
{
    [Fact]
    public void PricePerSquareMeter_DivisibleValues_ReturnsExactPrice()
    {
        var offer = OfferTestData.CreateOffer(askingPrice: 500_000m, area: 50m);

        Assert.Equal(10_000m, offer.PricePerSquareMeter().Amount);
    }

    // TheoryData zamiast InlineData: atrybuty nie przyjmują decimal, a double gubi precyzję.
    public static TheoryData<decimal, decimal, decimal> NonDivisibleCases => new()
    {
        { 500_000m, 47.3m, 10_570.82m },   // 10570.8245... -> w dół
        { 100_000m, 3m, 33_333.33m },      // 33333.333... -> w dół
        { 200_000m, 3m, 66_666.67m },      // 66666.666... -> w górę
        { 1_000.05m, 2m, 500.03m },        // 500.025 -> AwayFromZero
    };

    [Theory]
    [MemberData(nameof(NonDivisibleCases))]
    public void PricePerSquareMeter_NonDivisibleValues_RoundsToTwoDecimals(
        decimal askingPrice, decimal area, decimal expected)
    {
        var offer = OfferTestData.CreateOffer(askingPrice: askingPrice, area: area);

        Assert.Equal(expected, offer.PricePerSquareMeter().Amount);
    }
}
