namespace Ordering.Domain.ValueObjects;

public record OrderId
{
    public Guid Value { get; init;  }

    private OrderId(Guid value) => Value = value;
    
    public static OrderId Of(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw  new DomainException("OrderId cannot be an empty guid.");
        }

        return new OrderId(value);
    }
}