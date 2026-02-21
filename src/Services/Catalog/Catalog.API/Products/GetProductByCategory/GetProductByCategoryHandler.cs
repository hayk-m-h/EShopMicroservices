namespace Catalog.API.Products.GetProductByCategory;


public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

public class GetProductByCategoryHandler
    (IDocumentSession _session, ILogger<GetProductByCategoryHandler> _logger)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetProductByCategoryQuery for Category: {Category}", query.Category);

        var products = await _session.Query<Product>()
            .Where(p => p.Category.Contains(query.Category)).ToListAsync();

        return new GetProductByCategoryResult(products);
    }
}
