namespace Domain.Common;
public abstract record PersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirth,
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string Code
);