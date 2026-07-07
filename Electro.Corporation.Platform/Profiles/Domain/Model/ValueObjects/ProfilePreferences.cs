namespace Electro.Corporation.Platform.Profiles.Domain.Model.ValueObjects;

public record ProfilePreferences(string Language, string Theme, bool NotificationsEnabled)
{
    public ProfilePreferences() : this("es", "light", true)
    {
    }
}
