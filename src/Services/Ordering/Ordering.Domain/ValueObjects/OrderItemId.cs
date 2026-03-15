namespace Ordering.Domain.ValueObjects;

public record OrderItemId
{
    public Guid Value { get; init; }

    private OrderItemId(Guid value) => Value = value;

    public static OrderItemId Of(Guid value)
    {
        return value == Guid.Empty 
                   ? throw new DomainException("OrderId cannot be an empty guid.")
                   : new OrderItemId(value);
    }
}