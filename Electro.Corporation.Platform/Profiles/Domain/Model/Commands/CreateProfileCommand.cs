namespace Electro.Corporation.Platform.Profiles.Domain.Model.Commands;

public record CreateProfileCommand(
    int UserId,
    string FirstName,
    string LastName,
    string Email,
    string Street,
    string Number,
    string City,
    string PostalCode,
    string Country);
