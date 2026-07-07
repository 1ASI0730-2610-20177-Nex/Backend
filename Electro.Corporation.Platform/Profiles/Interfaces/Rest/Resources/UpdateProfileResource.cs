namespace Electro.Corporation.Platform.Profiles.Interfaces.Rest.Resources;

public record UpdateProfileResource(
    string FirstName,
    string LastName,
    string Email,
    string Street,
    string Number,
    string City,
    string PostalCode,
    string Country);

public record UpdateProfilePreferencesResource(
    string Language,
    string Theme,
    bool NotificationsEnabled);

public record ProfilePreferencesResource(string Language, string Theme, bool NotificationsEnabled);
