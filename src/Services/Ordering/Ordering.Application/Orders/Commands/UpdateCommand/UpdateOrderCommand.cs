using FluentValidation;

namespace Ordering.Application.Orders.Commands.UpdateCommand;

public record UpdateOrderCommand(OrderDto Order)
    : ICommand<UpdateOrderResult>;

public record UpdateOrderResult(bool IsSuccess);

public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderValidator()
    {
        RuleFor(x => x.Order.Id).NotEmpty().WithMessage("Id is required.");
        RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is required.");
        RuleFor(x => x.Order.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
    }
}
