namespace Domain.Common;
public abstract record PersonalInfo(
    string FirstName,
    string LastName,
    string Email,
    DateTime DateOfBirht,
    Address Address
);