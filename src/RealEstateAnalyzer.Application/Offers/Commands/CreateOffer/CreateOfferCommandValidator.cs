using FluentValidation;
using RealEstateAnalyzer.Domain.Offers.ValueObjects;

namespace RealEstateAnalyzer.Application.Offers.Commands.CreateOffer;

public sealed class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferCommandValidator(TimeProvider timeProvider)
    {
        RuleFor(x => x.Street).MaximumLength(Address.StreetMaxLength);
        RuleFor(x => x.City).NotEmpty().MaximumLength(Address.CityMaxLength);
        RuleFor(x => x.District).NotEmpty().MaximumLength(Address.DistrictMaxLength);
        RuleFor(x => x.AreaSquareMeters).GreaterThan(0);
        RuleFor(x => x.AskingPrice).GreaterThan(0);
        RuleFor(x => x.Rooms).GreaterThanOrEqualTo(1);
        RuleFor(x => x.Floor).InclusiveBetween(Floor.Min, Floor.Max);
        RuleFor(x => x.YearBuilt)
            .InclusiveBetween(YearBuilt.Min, timeProvider.GetUtcNow().Year + YearBuilt.MaxYearsAhead);
        RuleFor(x => x.AdminFee).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ListingUrl)
            .Must(BeAbsoluteHttpUrl!)
            .When(x => !string.IsNullOrWhiteSpace(x.ListingUrl))
            .WithMessage("'{PropertyName}' musi być absolutnym adresem http/https.");
    }

    private static bool BeAbsoluteHttpUrl(string url) =>
        Uri.TryCreate(url.Trim(), UriKind.Absolute, out var uri)
        && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
}
