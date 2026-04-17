namespace Ordering.Application.Extensions;

public static class OrderExtensions
{
    public static IEnumerable<OrderDto> ToOrderDtos(this IEnumerable<Order> orders)
    {
        return orders.Select(order => new OrderDto(
            order.Id!.Value, 
            order.CustomerId.Value, 
            order.OrderName.Value, 
            new AddressDto(
                order.ShippingAddress.FirstName, 
                order.ShippingAddress.LastName, 
                order.ShippingAddress.EmailAddress, 
                order.ShippingAddress.AddressLine, 
                order.ShippingAddress.State, 
                order.ShippingAddress.Country,
                order.ShippingAddress.ZipCode),
            new AddressDto(
                order.ShippingAddress.FirstName, 
                order.ShippingAddress.LastName, 
                order.ShippingAddress.EmailAddress, 
                order.ShippingAddress.AddressLine, 
                order.ShippingAddress.State, 
                order.ShippingAddress.Country,
                order.ShippingAddress.ZipCode),
            new PaymentDto(
                order.Payment.CardName, 
                order.Payment.CardNumber, 
                order.Payment.Expiration, 
                order.Payment.CVV,
                order.Payment.PaymentMethod),
            order.Status,
            order.OrderItems.Select(oi => new OrderItemDto(order.Id!.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()));
    }
}