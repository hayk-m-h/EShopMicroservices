using BuildingBlocks.Pagination;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders.Include(o => o.OrderItems)
            .AsNoTracking()
            .Skip(query.PaginationRequest.PageSize * query.PaginationRequest.PageIndex)
            .Take(query.PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);
        
        var count = await dbContext.Orders.LongCountAsync(cancellationToken);
        
        return new GetOrdersResult(new PaginatedResult<OrderDto>(
            query.PaginationRequest.PageIndex, query.PaginationRequest.PageSize, count, orders.ToOrderDtos()));
    }
}