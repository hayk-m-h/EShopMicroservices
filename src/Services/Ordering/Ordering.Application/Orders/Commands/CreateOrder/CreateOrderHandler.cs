namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = createNewOrder(command.Order);
        
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new CreateOrderResult(order.Id!.Value);
    }

    private Order createNewOrder(OrderDto commandOrder)
    {
        var shippingAddress = Address.Of(
            commandOrder.ShippingAddress.FirstName, 
            commandOrder.ShippingAddress.LastName, 
            commandOrder.ShippingAddress.EmailAddress, 
            commandOrder.ShippingAddress.AddressLine, 
            commandOrder.ShippingAddress.State, 
            commandOrder.ShippingAddress.Country,
            commandOrder.ShippingAddress.ZipCode);

        var billingAddress = Address.Of(
            commandOrder.BillingAddress.FirstName, 
            commandOrder.BillingAddress.LastName, 
            commandOrder.BillingAddress.EmailAddress, 
            commandOrder.BillingAddress.AddressLine, 
            commandOrder.BillingAddress.State, 
            commandOrder.BillingAddress.Country,
            commandOrder.BillingAddress.ZipCode);
        
        var payment = Payment.Of(
            commandOrder.Payment.CardName, 
            commandOrder.Payment.CardNumber, 
            commandOrder.Payment.Expiration, 
            commandOrder.Payment.Cvv,
            commandOrder.Payment.PaymentMethod);
        
        var order = Order.Create(
            OrderId.Of(Guid.NewGuid()), 
            CustomerId.Of(commandOrder.CustomerId), 
            OrderName.Of(commandOrder.OrderName), 
            shippingAddress, 
            billingAddress, 
            payment);

        foreach (var orderItemDto in commandOrder.OrderItems)
        {
            order.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }

        return order;
    }
}