using Catalog.API.Products.CreateProduct;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProducCommand(Guid Id, string Name, string Description, decimal Price, string ImageFile, List<string> Category)
    : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProducCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .Length(2, 150).WithMessage("{PropertyName} must be between 2 and 150 characters.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.ImageFile).MaximumLength(200).WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("{PropertyName} must be greater then 0.");
    }
}

public class UpdateProductCommandHandler
    (IDocumentSession _session, ILogger<UpdateProductCommandHandler> _logger) 
    : ICommandHandler<UpdateProducCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProducCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateProductCommand for Product Id: {ProductId}", command.Id);
        
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken);

        if (product == null) 
        {
            throw new ProductNotFoundException();
        }

        product.Name = command.Name;
        product.Description = command.Description;
        product.Price = command.Price;
        product.ImageFile = command.ImageFile;
        product.Category = [.. command.Category];

        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new UpdateProductResult(IsSuccess: true);
    }
}
