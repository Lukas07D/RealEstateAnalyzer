using MediatR;

namespace RealEstateAnalyzer.Application.Offers.Commands.SetRentalAssumptions;

/// <returns>false, gdy oferta nie istnieje.</returns>
public sealed record SetRentalAssumptionsCommand(
    Guid OfferId,
    decimal MonthlyRent,
    int VacancyMonthsPerYear,
    decimal OtherMonthlyCosts) : IRequest<bool>;
