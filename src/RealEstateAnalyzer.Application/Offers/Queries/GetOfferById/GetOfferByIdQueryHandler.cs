using MediatR;
using RealEstateAnalyzer.Application.Common.Abstractions;
using RealEstateAnalyzer.Application.Offers.Dtos;

namespace RealEstateAnalyzer.Application.Offers.Queries.GetOfferById;

public sealed class GetOfferByIdQueryHandler(IOfferRepository offers)
    : IRequestHandler<GetOfferByIdQuery, OfferDto?>
{
    public async Task<OfferDto?> Handle(GetOfferByIdQuery request, CancellationToken cancellationToken)
    {
        var offer = await offers.GetByIdAsync(request.Id, cancellationToken);
        return offer is null ? null : OfferDto.FromDomain(offer);
    }
}
