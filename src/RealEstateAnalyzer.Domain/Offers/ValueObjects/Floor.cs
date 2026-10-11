using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

/// <summary>Piętro: -1 = suterena, 0 = parter.</summary>
public sealed record Floor
{
    public const int Min = -1;
    public const int Max = 100;

    public int Value { get; }

    private Floor(int value) => Value = value;

    public static Floor Create(int value)
    {
        if (value is < Min or > Max)
            throw new DomainException($"Piętro musi być w zakresie {Min}..{Max}.");

        return new Floor(value);
    }
}
