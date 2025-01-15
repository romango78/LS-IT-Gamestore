using EnsureThat;

namespace Gamestore.Domain.Entities;

public class CustomerEntity
{
    public string Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public CustomerEntity(string id, string firstName, string lastName, string email)
    {
        EnsureArg.IsNotEmptyOrWhiteSpace(id, nameof(id));
        EnsureArg.IsNotEmptyOrWhiteSpace(firstName, nameof(firstName));
        EnsureArg.IsNotEmptyOrWhiteSpace(lastName, nameof(lastName));
        EnsureArg.IsNotEmptyOrWhiteSpace(email, nameof(email));

        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
}