namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIdQueryHandler 
    (IDocumentSession _session, ILogger<GetProductByIdQueryHandler> _logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductByIdQuery for Id: {Id}", query.Id);

        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken);

        if (product == null)
        {
            _logger.LogWarning("Product with Id: {Id} not found", query.Id);
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(product);
    }
}
