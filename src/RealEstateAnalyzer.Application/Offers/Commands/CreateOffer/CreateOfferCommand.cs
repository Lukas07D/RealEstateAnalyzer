using MediatR;

namespace RealEstateAnalyzer.Application.Offers.Commands.CreateOffer;

public sealed record CreateOfferCommand(
    string? Street,
    string City,
    string District,
    decimal AreaSquareMeters,
    decimal AskingPrice,
    int Rooms,
    int Floor,
    int YearBuilt,
    decimal AdminFee,
    string? ListingUrl) : IRequest<Guid>;
