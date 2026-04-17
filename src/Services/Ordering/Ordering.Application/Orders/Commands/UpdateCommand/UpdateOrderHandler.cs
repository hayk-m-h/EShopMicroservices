namespace Ordering.Application.Orders.Commands.UpdateCommand;

public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }

        updateOrder(order, command.Order);

        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return new UpdateOrderResult(true);
    }

    private void updateOrder(Order order, OrderDto commandOrder)
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
        
        order.Update(
            OrderName.Of(commandOrder.OrderName), 
            shippingAddress, 
            billingAddress, 
            payment,
            commandOrder.Status);
    }
}