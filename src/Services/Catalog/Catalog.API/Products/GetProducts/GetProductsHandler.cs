namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber, int? PageSize = 10) : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

public class GetProductsQueryHandler
    (IDocumentSession _session) : 
    IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Product>? result = null;

        if (query.PageSize == null)
        {
            result = await _session.Query<Product>().ToListAsync();
        }
        else
        {
            result = await _session.Query<Product>()
                                     .ToPagedListAsync(query.PageNumber?? 1, query.PageSize.Value,  cancellationToken);
            
        }
        
        return new GetProductsResult(result);
    }
}
