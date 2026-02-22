namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(Guid Id, string Name, string Description, decimal Price, string ImageFileName, List<string> Category);
public record UpdateProductResponse(bool IsSuccess);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateProducCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateProductResponse>();
            return Results.Ok(response);
        })
          .WithName("UpdateProducts")
          .Produces<UpdateProductRequest>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Update product")
          .WithDescription("Update product");
    }
}
