using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;

public class ProductService(IProductRepository _productRepository, ApiService _apiService) : IProductService
{
    
    public async Task SyncUpdatedProductsAsync()

    {

        using var connection = _productRepository.GetDbConnection();
        connection.Open();

        var productsToSync = await _productRepository.GetUnsyncedProductsAsync(connection);

        foreach (var product in productsToSync)
        {
            try
            {
                await _apiService.UpdateStockAndPriceAsync(product.StockCode, product.Price, product.Stock);
                await _productRepository.MarkAsSyncedAsync(product.Id, connection);
                await _productRepository.InsertLogAsync("INFO", $"[SYNC] Ürün güncellendi: {product.Name} | Stok: {product.Stock} | Fiyat: {product.Price}", connection, null);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Servise aktarım hatası: {product.Name} - {ex.Message}");
                await _productRepository.InsertLogAsync("ERROR", $"[SYNC ERROR] {product.Name} - {ex.Message}", connection, null);
            }
        }
    }

    public async Task MarkProductAsDirtyAsync(int productId)
    {
        using var connection = _productRepository.GetDbConnection();

        connection.Open();

        await _productRepository.MarkAsUnsyncedAsync(productId, connection);
        await _productRepository.InsertLogAsync("INFO", $"[DIRTY MARKED] Ürün ID: {productId} senkronizasyon için işaretlendi.", connection, null);
    }

    public async Task<bool> ProcessOrderItemAsync(OrderItem item)
    {
        using var connection = _productRepository.GetDbConnection();
        connection.Open();
        using var trx = connection.BeginTransaction();

        try
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, connection, trx);

            if (product == null || product.Stock < item.Quantity)
            {
                await _productRepository.InsertLogAsync("ERROR", $"[BUY ERROR] Yetersiz stok veya ürün yok: ProductId={item.ProductId}", connection, trx);
                return false;
            }

            await _productRepository.UpdateStockAsync(product.Id, item.Quantity, connection, trx);
            await _apiService.UpdateStockAndPriceAsync(product.StockCode, product.Price, product.Stock - item.Quantity);
            await _productRepository.MarkAsUnsyncedAsync(product.Id, connection, trx);

            await _productRepository.InsertLogAsync("INFO", $"[BUY] {product.Name} ürününden {item.Quantity} adet satın alındı.", connection, trx);

            trx.Commit();
            return true;
        }
        catch (Exception ex)
        {
            trx.Rollback();
            await _productRepository.InsertLogAsync("ERROR", $"[PROCESS ERROR] {item.ProductId} - {ex.Message}", connection, null);
            return false;
        }
    }
}