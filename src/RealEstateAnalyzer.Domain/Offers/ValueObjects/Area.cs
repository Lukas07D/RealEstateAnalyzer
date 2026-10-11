using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

public sealed record Area
{
    public decimal SquareMeters { get; }

    private Area(decimal squareMeters) => SquareMeters = squareMeters;

    public static Area Create(decimal squareMeters)
    {
        if (squareMeters <= 0)
            throw new DomainException("Metraż musi być większy od 0.");

        return new Area(squareMeters);
    }
}
