using Dehasoft.DataAccess.Models;

public interface IProductService
{
    Task SyncUpdatedProductsAsync();
    Task MarkProductAsDirtyAsync(int productId);
    Task<bool> ProcessOrderItemAsync(OrderItem item);
}