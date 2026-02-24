namespace Basket.API.Basket.GetBasket;

public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCartResponse Cart);
public record ShoppingCartResponse(string UserName, List<ShoppingCartItemResponse> Items, decimal TotalPrice);
public record ShoppingCartItemResponse(int Quantity, string Color, decimal Price, Guid ProductId, string ProductName);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
         app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
         {
             var  result = await sender.Send(new GetBasketRequest(userName));
             var response = result.Adapt<GetBasketResponse>();
             
             return Results.Ok(response);
         })
         .WithName("GetBasketRequest")
         .Produces<GetBasketResponse>(StatusCodes.Status201Created)
         .ProducesProblem(StatusCodes.Status404NotFound)
         .WithSummary("Get a basket by user name.")
         .WithDescription("Get a basket by user name.");
    }
}