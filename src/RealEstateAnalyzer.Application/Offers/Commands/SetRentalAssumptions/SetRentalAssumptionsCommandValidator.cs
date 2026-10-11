using FluentValidation;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Application.Offers.Commands.SetRentalAssumptions;

public sealed class SetRentalAssumptionsCommandValidator : AbstractValidator<SetRentalAssumptionsCommand>
{
    public SetRentalAssumptionsCommandValidator()
    {
        RuleFor(x => x.MonthlyRent).GreaterThan(0);
        RuleFor(x => x.VacancyMonthsPerYear).InclusiveBetween(0, RentalAssumptions.MaxVacancyMonthsPerYear);
        RuleFor(x => x.OtherMonthlyCosts).GreaterThanOrEqualTo(0);
    }
}
