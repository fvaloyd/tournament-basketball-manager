using Domain.Common;

namespace Domain.Players;
public sealed record PlayerPersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    float Height,
    float Weight,
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string Code
) : PersonalInfo(FirstName: FirstName, LastName: LastName, Email: Email, DateOfBirth: DateOfBirth, Country, City, Street, HouseNumber, Code);