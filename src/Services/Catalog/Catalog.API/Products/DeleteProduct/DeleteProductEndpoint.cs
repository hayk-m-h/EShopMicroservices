namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductRequest(Guid Id);
public record DeleteProductResponse(bool IsSuccess);

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products/{id:guid}", async (Guid id, ISender sender) =>
        {
            var command = new DeleteProductComand(id);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteProductResponse>();
            return Results.Ok(response);
        })
          .WithName("DeleteProducts")
          .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status400BadRequest)
          .ProducesProblem(StatusCodes.Status404NotFound)
          .WithSummary("Delete product")
          .WithDescription("Delete product");
    }
}
