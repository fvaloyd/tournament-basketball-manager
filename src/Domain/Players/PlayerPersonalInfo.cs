using Domain.Common;

namespace Domain.Players;
public sealed record PlayerPersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirht,
    Address Address,
    float Height,
    float Weight
) : PersonalInfo(FirstName: FirstName, LastName: LastName, Email: Email, DateOfBirht: DateOfBirht, Address: Address);