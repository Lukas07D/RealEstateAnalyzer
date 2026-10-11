using RealEstateAnalyzer.Domain.Common;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Domain.Tests.Offers;

public class RentalAssumptionsTests
{
    [Fact]
    public void Create_WithZeroRent_Throws()
    {
        var ex = Assert.Throws<DomainException>(() => RentalAssumptions.Create(Money.Zero));

        Assert.Contains("Czynsz najmu", ex.Message);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(13)]
    public void Create_WithVacancyOutOfRange_Throws(int vacancyMonths)
    {
        var ex = Assert.Throws<DomainException>(
            () => RentalAssumptions.Create(Money.Create(2_500m), vacancyMonths));

        Assert.Contains("Pustostan", ex.Message);
    }

    [Fact]
    public void Create_WithDefaults_HasNoVacancyAndZeroCosts()
    {
        var assumptions = RentalAssumptions.Create(Money.Create(2_500m));

        Assert.Equal(0, assumptions.VacancyMonthsPerYear);
        Assert.Equal(Money.Zero, assumptions.OtherMonthlyCosts);
    }

    [Fact]
    public void Equality_IsByValue()
    {
        var a = RentalAssumptions.Create(Money.Create(2_500m), 1, Money.Create(100m));
        var b = RentalAssumptions.Create(Money.Create(2_500m), 1, Money.Create(100m));

        Assert.Equal(a, b);
    }
}
