using Basket.API.Basket.GetBasket;

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
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new StoreBasketResult(command.Cart.UserName));
    }
}