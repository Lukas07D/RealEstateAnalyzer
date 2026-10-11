using MediatR;
using RealEstateAnalyzer.Application.Offers.Dtos;

namespace RealEstateAnalyzer.Application.Offers.Queries.GetOfferById;

public sealed record GetOfferByIdQuery(Guid Id) : IRequest<OfferDto?>;
