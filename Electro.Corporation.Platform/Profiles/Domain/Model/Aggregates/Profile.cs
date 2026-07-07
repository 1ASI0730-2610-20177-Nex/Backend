using Electro.Corporation.Platform.Profiles.Domain.Model.Commands;
using Electro.Corporation.Platform.Profiles.Domain.Model.ValueObjects;

namespace Electro.Corporation.Platform.Profiles.Domain.Model.Aggregates;

/// <summary>
///     Profile aggregate root. References IAM User by scalar UserId only (no nested aggregate).
/// </summary>
public partial class Profile
{
    public Profile()
    {
        Name = new PersonName();
        Email = new EmailAddress();
        Address = new StreetAddress();
    }

    public Profile(CreateProfileCommand command)
    {
        UserId = command.UserId;
        Name = new PersonName(command.FirstName, command.LastName);
        Email = new EmailAddress(command.Email);
        Address = new StreetAddress(command.Street, command.Number, command.City, command.PostalCode, command.Country);
    }

    public int Id { get; }
    public int UserId { get; }
    public PersonName Name { get; }
    public EmailAddress Email { get; }
    public StreetAddress Address { get; }

    public string FullName => Name.FullName;
    public string EmailAddress => Email.Address;
    public string StreetAddress => Address.FullAddress;
}
