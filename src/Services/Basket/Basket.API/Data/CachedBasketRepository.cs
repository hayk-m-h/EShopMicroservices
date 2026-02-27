using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository
    (IBasketRepository _repository, IDistributedCache _cache) 
    : IBasketRepository
{
    public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);
        if (!string.IsNullOrEmpty(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket = await _repository.GetBasketAsync(userName, cancellationToken);
        await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
        return basket;
    }

    public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart basket, CancellationToken cancellationToken = default)
    {
        var basketResult = await _repository.StoreBasketAsync(basket, cancellationToken); 
        await _cache.SetStringAsync(basketResult.UserName, JsonSerializer.Serialize(basketResult), cancellationToken);
        return basketResult;
    }

    public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
    {
        await _repository.DeleteBasketAsync(userName, cancellationToken);
        await _cache.RemoveAsync(userName, cancellationToken);
        return true;
    }
}