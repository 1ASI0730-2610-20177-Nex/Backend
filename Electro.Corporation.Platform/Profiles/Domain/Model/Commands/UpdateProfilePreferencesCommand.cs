namespace Electro.Corporation.Platform.Profiles.Domain.Model.Commands;

public record UpdateProfilePreferencesCommand(
    int ProfileId,
    string Language,
    string Theme,
    bool NotificationsEnabled);
