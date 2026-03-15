namespace Ordering.Domain.ValueObjects;

public record CustomerId()
{
    public Guid Value { get; init; }
    
    private CustomerId(Guid value) : this() => Value = value;
    
    public static CustomerId Of(Guid value)
    {
        return value == Guid.Empty ? throw new DomainException("CustomerId cannot be an empty guid.") : new CustomerId(value);
    }
}