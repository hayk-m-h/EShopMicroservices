namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(ShoppingCartRequest Cart);
public record ShoppingCartRequest(string UserName, List<ShoppingCartItemRequest> Items, decimal TotalPrice);
public record ShoppingCartItemRequest(int Quantity, string Color, decimal Price, Guid ProductId, string ProductName);
public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/basket", async (StoreBasketRequest request, ISender sender) =>
           {
               var command = request.Adapt<StoreBasketCommand>();
               var result = await sender.Send(command);
               var response = result.Adapt<StoreBasketResponse>();
               
               return Results.Created($"/basket/{response.UserName}", response);
           })
           .WithName("StoreBasketRequest")
           .Produces<StoreBasketResult>(StatusCodes.Status201Created)
           .ProducesProblem(StatusCodes.Status400BadRequest)
           .WithSummary("Store basket request.")
           .WithDescription("Store basket request.");
    }
}