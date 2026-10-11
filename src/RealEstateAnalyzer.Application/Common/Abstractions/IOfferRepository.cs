using RealEstateAnalyzer.Domain.Offers;

namespace RealEstateAnalyzer.Application.Common.Abstractions;

public interface IOfferRepository
{
    void Add(Offer offer);
    Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
