namespace Domain.Common;
public record Address(
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string Code
);