using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

/// <summary>Kwota w PLN (MVP: jedna waluta, bez pola Currency).</summary>
public sealed record Money
{
    public static readonly Money Zero = new(0m);

    public decimal Amount { get; }

    private Money(decimal amount) => Amount = amount;

    public static Money Create(decimal amount)
    {
        if (amount < 0)
            throw new DomainException("Kwota nie może być ujemna.");

        return new Money(Math.Round(amount, 2, MidpointRounding.AwayFromZero));
    }
}
