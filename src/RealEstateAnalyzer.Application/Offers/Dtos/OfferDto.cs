using RealEstateAnalyzer.Domain.Offers;

namespace RealEstateAnalyzer.Application.Offers.Dtos;

public sealed record OfferDto(
    Guid Id,
    string? Street,
    string City,
    string District,
    decimal AreaSquareMeters,
    decimal AskingPrice,
    decimal PricePerSquareMeter,
    int Rooms,
    int Floor,
    int YearBuilt,
    decimal AdminFee,
    string? ListingUrl,
    DateTimeOffset CreatedAt,
    RentalAssumptionsDto? RentalAssumptions,
    RentalYieldDto? RentalYield)
{
    public static OfferDto FromDomain(Offer offer)
    {
        var assumptions = offer.RentalAssumptions;
        var rentalYield = offer.CalculateRentalYield();

        return new(
            offer.Id,
            offer.Address.Street,
            offer.Address.City,
            offer.Address.District,
            offer.Area.SquareMeters,
            offer.AskingPrice.Amount,
            offer.PricePerSquareMeter().Amount,
            offer.Rooms,
            offer.Floor.Value,
            offer.YearBuilt.Value,
            offer.AdminFee.Amount,
            offer.ListingUrl?.ToString(),
            offer.CreatedAt,
            assumptions is null
                ? null
                : new RentalAssumptionsDto(
                    assumptions.MonthlyRent.Amount,
                    assumptions.VacancyMonthsPerYear,
                    assumptions.OtherMonthlyCosts.Amount),
            rentalYield is null
                ? null
                : new RentalYieldDto(rentalYield.GrossPercent, rentalYield.NetPercent));
    }
}

public sealed record RentalAssumptionsDto(decimal MonthlyRent, int VacancyMonthsPerYear, decimal OtherMonthlyCosts);

public sealed record RentalYieldDto(decimal GrossPercent, decimal NetPercent);
