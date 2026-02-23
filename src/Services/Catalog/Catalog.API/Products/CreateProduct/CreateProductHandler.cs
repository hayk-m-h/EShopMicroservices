namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommad(string Name, List<string> Category, string Description, string ImageFile, decimal Price) 
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommad>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Category).NotEmpty().WithMessage("{PropertyName} is required.");
        RuleFor(x => x.ImageFile).MaximumLength(200).WithMessage("{PropertyName} is required.");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("{PropertyName} must be greater then 0.");
    }
}

internal class CreateProductCommandHandler
    (IDocumentSession _session) 
    : ICommandHandler<CreateProductCommad, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommad commad, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = commad.Name,
            Category = commad.Category,
            Description = commad.Description,
            ImageFile = commad.ImageFile,
            Price = commad.Price
        };

        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken);

        return new CreateProductResult(product.Id);
    }
}

