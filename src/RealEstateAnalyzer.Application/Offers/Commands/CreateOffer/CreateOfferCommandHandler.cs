using MediatR;
using RealEstateAnalyzer.Application.Common.Abstractions;
using RealEstateAnalyzer.Domain.Offers;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Application.Offers.Commands.CreateOffer;

public sealed class CreateOfferCommandHandler(
    IOfferRepository offers,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IRequestHandler<CreateOfferCommand, Guid>
{
    public async Task<Guid> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
    {
        var now = timeProvider.GetUtcNow();

        var offer = Offer.Create(
            Address.Create(request.Street, request.City, request.District),
            Area.Create(request.AreaSquareMeters),
            Money.Create(request.AskingPrice),
            request.Rooms,
            Floor.Create(request.Floor),
            YearBuilt.Create(request.YearBuilt, now.Year),
            Money.Create(request.AdminFee),
            string.IsNullOrWhiteSpace(request.ListingUrl) ? null : ListingUrl.Create(request.ListingUrl),
            now);

        offers.Add(offer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return offer.Id;
    }
}
