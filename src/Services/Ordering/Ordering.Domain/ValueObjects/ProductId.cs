namespace Ordering.Domain.ValueObjects;

public record ProductId
{
    public Guid Value { get; init; }

    private ProductId(Guid value) => Value = value;
    
    public static ProductId Of(Guid value)
    {
        return value == Guid.Empty 
                   ? throw new DomainException("ProductId cannot be an empty guid.")
                   : new ProductId(value);
    }
}