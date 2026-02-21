namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommad(string Name, List<string> Category, string Description, string ImageFile, decimal Price) 
    : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(IDocumentSession _session) : ICommandHandler<CreateProductCommad, CreateProductResult>
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

