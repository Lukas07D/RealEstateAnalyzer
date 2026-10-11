using Microsoft.EntityFrameworkCore;
using RealEstateAnalyzer.Application.Common.Abstractions;
using RealEstateAnalyzer.Domain.Offers;

namespace RealEstateAnalyzer.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Offer> Offers => Set<Offer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
}
