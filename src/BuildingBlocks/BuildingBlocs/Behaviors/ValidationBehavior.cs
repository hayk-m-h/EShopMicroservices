using BuildingBlocks.CQRS;
using FluentValidation;
using MediatR;
using System.Windows.Input;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> _validators)
    : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull, ICommand<TResponse>
     where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await next(cancellationToken);
    }
}
