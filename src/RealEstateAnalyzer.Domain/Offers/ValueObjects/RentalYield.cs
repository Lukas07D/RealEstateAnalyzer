namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

/// <summary>Wynik obliczenia rentowności (w procentach); nie jest zapisywany.</summary>
public sealed record RentalYield(decimal GrossPercent, decimal NetPercent);
