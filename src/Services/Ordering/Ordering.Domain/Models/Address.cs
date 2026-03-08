namespace Ordering.Domain.Models;

public record Address(
          string FirstName
        , string LastName
        , string? EmailAddress
        , string AddressLine
        , string State
        , string Country
        , string ZipCode);