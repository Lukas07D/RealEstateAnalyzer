using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

/// <summary>Założenia najmu wpisane przez użytkownika; wymieniane w całości.</summary>
public sealed record RentalAssumptions
{
    public const int MaxVacancyMonthsPerYear = 12;

    public Money MonthlyRent { get; }
    public int VacancyMonthsPerYear { get; }
    public Money OtherMonthlyCosts { get; }

    private RentalAssumptions(Money monthlyRent, int vacancyMonthsPerYear, Money otherMonthlyCosts)
    {
        MonthlyRent = monthlyRent;
        VacancyMonthsPerYear = vacancyMonthsPerYear;
        OtherMonthlyCosts = otherMonthlyCosts;
    }

    public static RentalAssumptions Create(Money monthlyRent, int vacancyMonthsPerYear = 0, Money? otherMonthlyCosts = null)
    {
        ArgumentNullException.ThrowIfNull(monthlyRent);

        if (monthlyRent.Amount <= 0)
            throw new DomainException("Czynsz najmu musi być większy od 0.");
        if (vacancyMonthsPerYear is < 0 or > MaxVacancyMonthsPerYear)
            throw new DomainException($"Pustostan musi być w zakresie 0..{MaxVacancyMonthsPerYear} miesięcy.");

        return new RentalAssumptions(monthlyRent, vacancyMonthsPerYear, otherMonthlyCosts ?? Money.Zero);
    }
}
