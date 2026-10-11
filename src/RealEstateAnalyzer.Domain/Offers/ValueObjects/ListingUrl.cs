using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

public sealed record ListingUrl
{
    public Uri Value { get; }

    private ListingUrl(Uri value) => Value = value;

    public static ListingUrl Create(string url)
    {
        if (!Uri.TryCreate(url?.Trim(), UriKind.Absolute, out var uri)
            || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new DomainException("Link do ogłoszenia musi być absolutnym adresem http/https.");

        return new ListingUrl(uri);
    }

    public override string ToString() => Value.ToString();
}
