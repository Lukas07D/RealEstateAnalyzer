using MediatR;
using RealEstateAnalyzer.Application.Common.Abstractions;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Application.Offers.Commands.SetRentalAssumptions;

public sealed class SetRentalAssumptionsCommandHandler(
    IOfferRepository offers,
    IUnitOfWork unitOfWork) : IRequestHandler<SetRentalAssumptionsCommand, bool>
{
    public async Task<bool> Handle(SetRentalAssumptionsCommand request, CancellationToken cancellationToken)
    {
        var offer = await offers.GetByIdAsync(request.OfferId, cancellationToken);
        if (offer is null)
            return false;

        offer.SetRentalAssumptions(RentalAssumptions.Create(
            Money.Create(request.MonthlyRent),
            request.VacancyMonthsPerYear,
            Money.Create(request.OtherMonthlyCosts)));

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
