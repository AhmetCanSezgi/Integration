using Dehasoft.DataAccess.Models;

public interface IProductService
{

    Task<bool> UpdateProductPriceAndStockAsync(int productId, decimal newPrice);
    Task<bool> ProcessOrderItemAsync(OrderItem item);
}