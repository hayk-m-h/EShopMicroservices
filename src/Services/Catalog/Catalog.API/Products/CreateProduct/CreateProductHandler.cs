using MediatR;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommad(string Name, List<string> Category, string Description, string ImageFile, decimal Price) 
    : IRequest<CreateProductResult>;
public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommad, CreateProductResult>
{
    public Task<CreateProductResult> Handle(CreateProductCommad request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

