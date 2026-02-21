namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler(IDocumentSession _session, ILogger<GetProductsQueryHandler> _logger) : 
    IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductsQuery with {@Query}", query);

        var products = await _session.Query<Product>().ToListAsync(cancellationToken);

        return new GetProductsResult(products);
    }
}
