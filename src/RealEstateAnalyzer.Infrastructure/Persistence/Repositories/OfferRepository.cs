using Microsoft.EntityFrameworkCore;
using RealEstateAnalyzer.Application.Common.Abstractions;
using RealEstateAnalyzer.Domain.Offers;

namespace RealEstateAnalyzer.Infrastructure.Persistence.Repositories;

internal sealed class OfferRepository(AppDbContext dbContext) : IOfferRepository
{
    public void Add(Offer offer) => dbContext.Offers.Add(offer);

    public Task<Offer?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        dbContext.Offers.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
}
