using Catalog.API.Products.GetProductById;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IEnumerable<ProductResponse> Products);

public record ProductResponse(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/category/{category}", 
            async (string category, ISender sender) =>
        {
            var query = new GetProductByCategoryQuery(category);
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductByCategoryResponse>();
            return Results.Ok(response);
        })
          .WithName("GetProductByCategory")
          .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get product by Category")
          .WithDescription("Get product by Category");
    }
}
