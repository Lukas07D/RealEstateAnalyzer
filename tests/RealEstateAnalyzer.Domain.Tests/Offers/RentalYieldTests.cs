using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Domain.Tests.Offers;

public class RentalYieldTests
{
    [Fact]
    public void CalculateRentalYield_WithoutAssumptions_ReturnsNull()
    {
        var offer = OfferTestData.CreateOffer();

        Assert.Null(offer.CalculateRentalYield());
    }

    [Fact]
    public void CalculateRentalYield_Gross_IsAnnualRentDividedByPrice()
    {
        // 2 500 * 12 / 500 000 * 100 = 6.00
        var offer = OfferTestData.CreateOffer(askingPrice: 500_000m);
        offer.SetRentalAssumptions(RentalAssumptions.Create(Money.Create(2_500m)));

        Assert.Equal(6.00m, offer.CalculateRentalYield()!.GrossPercent);
    }

    [Fact]
    public void CalculateRentalYield_Gross_IgnoresVacancy()
    {
        var offer = OfferTestData.CreateOffer(askingPrice: 500_000m);
        offer.SetRentalAssumptions(RentalAssumptions.Create(Money.Create(2_500m), vacancyMonthsPerYear: 2));

        Assert.Equal(6.00m, offer.CalculateRentalYield()!.GrossPercent);
    }

    [Fact]
    public void CalculateRentalYield_Net_SubtractsVacancyAndCosts()
    {
        // przychód: 2 500 * (12 - 1) = 27 500; koszty: (650 + 100) * 12 = 9 000
        // (27 500 - 9 000) / 500 000 * 100 = 3.70
        var offer = OfferTestData.CreateOffer(askingPrice: 500_000m, adminFee: 650m);
        offer.SetRentalAssumptions(
            RentalAssumptions.Create(Money.Create(2_500m), vacancyMonthsPerYear: 1, otherMonthlyCosts: Money.Create(100m)));

        Assert.Equal(3.70m, offer.CalculateRentalYield()!.NetPercent);
    }

    [Fact]
    public void CalculateRentalYield_Net_AdminFeeHigherThanRent_IsNegative()
    {
        // przychód: 1 000 * 12 = 12 000; koszty: 1 500 * 12 = 18 000
        // (12 000 - 18 000) / 500 000 * 100 = -1.20
        var offer = OfferTestData.CreateOffer(askingPrice: 500_000m, adminFee: 1_500m);
        offer.SetRentalAssumptions(RentalAssumptions.Create(Money.Create(1_000m)));

        Assert.Equal(-1.20m, offer.CalculateRentalYield()!.NetPercent);
    }

    [Fact]
    public void CalculateRentalYield_RoundsToTwoDecimals()
    {
        // 2 000 * 12 / 333 333 * 100 = 7.2000072... -> 7.20
        var offer = OfferTestData.CreateOffer(askingPrice: 333_333m, adminFee: 0m);
        offer.SetRentalAssumptions(RentalAssumptions.Create(Money.Create(2_000m)));

        var yield = offer.CalculateRentalYield()!;

        Assert.Equal(7.20m, yield.GrossPercent);
        Assert.Equal(7.20m, yield.NetPercent);
    }
}
