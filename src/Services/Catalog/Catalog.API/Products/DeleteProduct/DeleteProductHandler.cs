using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductComand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductComand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required.");
    }
}

public class DeleteProductCommadHandler
    (IDocumentSession _session, ILogger<DeleteProductCommadHandler> _logger)
    : ICommandHandler<DeleteProductComand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductComand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteProductComand for Product Id: {ProductId}", command.Id);
        _session.Delete<Product>(command.Id);
        await _session.SaveChangesAsync(cancellationToken);

        return new DeleteProductResult(IsSuccess: true);
    }
}
