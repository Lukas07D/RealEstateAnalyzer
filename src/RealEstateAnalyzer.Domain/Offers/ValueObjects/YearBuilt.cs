using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

public sealed record YearBuilt
{
    public const int Min = 1800;
    public const int MaxYearsAhead = 5;

    public int Value { get; }

    private YearBuilt(int value) => Value = value;

    /// <param name="currentYear">Bieżący rok (z TimeProvider w warstwie wywołującej).</param>
    public static YearBuilt Create(int value, int currentYear)
    {
        var max = currentYear + MaxYearsAhead;
        if (value < Min || value > max)
            throw new DomainException($"Rok budowy musi być w zakresie {Min}..{max}.");

        return new YearBuilt(value);
    }
}
