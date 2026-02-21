namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(ProductResponse Product);
public record ProductResponse(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductByIdResponse>();
            return Results.Ok(response);
        })
          .WithName("GetProductById")
          .Produces<GetProductByIdResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .WithSummary("Get product by Id")
          .WithDescription("Get product by Id");
    }
}
