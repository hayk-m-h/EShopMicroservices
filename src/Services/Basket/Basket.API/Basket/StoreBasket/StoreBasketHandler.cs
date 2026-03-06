namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) :  ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public  StoreBasketCommandValidator()
    {
        RuleFor(c => c.Cart).NotNull().WithMessage("The cart is required");
        RuleFor(c => c.Cart.UserName).NotEmpty().WithMessage("The username is required");
    }
}

public class StoreBasketCommandHandler 
    (IBasketRepository _repository,
     DiscountProtoService.DiscountProtoServiceClient _discountProtoServiceClient)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await DeductDiscountAsync(command.Cart, cancellationToken);
        
        await _repository.StoreBasketAsync(command.Cart, cancellationToken);
        
        return new StoreBasketResult(command.Cart.UserName);
    }

    private async Task DeductDiscountAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
    {
        foreach (var item in cart.Items)
        {
            var coupon = await _discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName }, cancellationToken: cancellationToken);
            item.Price -= coupon.Amount;
        }
    }
}