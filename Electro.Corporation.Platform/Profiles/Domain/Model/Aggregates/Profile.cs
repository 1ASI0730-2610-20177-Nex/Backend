using Electro.Corporation.Platform.Profiles.Domain.Model.Commands;
using Electro.Corporation.Platform.Profiles.Domain.Model.ValueObjects;

namespace Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;

public partial class Profile
{
    public Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
        Address = new StreetAddress();
        Preferences = new ProfilePreferences();
    }

    public Profile(CreateProfileCommand command)
    {
        UserId = command.UserId;
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new EmailAddress(command.Email);
        Address = new StreetAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
        Preferences = new ProfilePreferences();
    }

    public int Id { get; private set; }
    public int UserId { get; private set; }
    public PersonName Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public StreetAddress Address { get; private set; }
    public ProfilePreferences Preferences { get; private set; }

    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;
    public string StreetAddress => Address.FullAddress;

    public void Update(UpdateProfileCommand command)
    {
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new EmailAddress(command.Email);
        Address = new StreetAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
    }

    public void UpdatePreferences(UpdateProfilePreferencesCommand command)
    {
        Preferences = new ProfilePreferences(command.Language, command.Theme, command.NotificationsEnabled);
    }
}
