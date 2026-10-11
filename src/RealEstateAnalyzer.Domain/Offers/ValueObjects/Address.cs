using RealEstateAnalyzer.Domain.Common;

namespace RealEstateAnalyzer.Domain.Offers.ValueObjects;

public sealed record Address
{
    public const int StreetMaxLength = 200;
    public const int CityMaxLength = 100;
    public const int DistrictMaxLength = 100;

    public string? Street { get; }
    public string City { get; }
    public string District { get; }

    private Address(string? street, string city, string district)
    {
        Street = street;
        City = city;
        District = district;
    }

    public static Address Create(string? street, string city, string district)
    {
        var trimmedStreet = string.IsNullOrWhiteSpace(street) ? null : street.Trim();
        var trimmedCity = city?.Trim() ?? string.Empty;
        var trimmedDistrict = district?.Trim() ?? string.Empty;

        if (trimmedCity.Length == 0)
            throw new DomainException("Miasto jest wymagane.");
        if (trimmedDistrict.Length == 0)
            throw new DomainException("Dzielnica jest wymagana.");
        if (trimmedStreet?.Length > StreetMaxLength)
            throw new DomainException($"Ulica może mieć maksymalnie {StreetMaxLength} znaków.");
        if (trimmedCity.Length > CityMaxLength)
            throw new DomainException($"Miasto może mieć maksymalnie {CityMaxLength} znaków.");
        if (trimmedDistrict.Length > DistrictMaxLength)
            throw new DomainException($"Dzielnica może mieć maksymalnie {DistrictMaxLength} znaków.");

        return new Address(trimmedStreet, trimmedCity, trimmedDistrict);
    }
}
