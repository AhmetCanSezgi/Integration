using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;

public class ProductService(IProductRepository _productRepository, ApiService _apiService, ILogService _logService) : IProductService
{

    public async Task<bool> UpdateProductPriceAndStockAsync(int productId, decimal newPrice)
    {
        using var connection = _productRepository.GetDbConnection();
        connection.Open();
        using var trx = connection.BeginTransaction();

        try
        {
            var product = await _productRepository.GetByIdAsync(productId, connection, trx);
            if (product == null)
            {
                await _logService.LogAsync("ERROR", $"[UPDATE ERROR] Ürün bulunamadı. ID: {productId}", trx);
                trx.Rollback();
                return false;
            }

            
            bool priceChanged = product.Price != newPrice;

            if (!priceChanged)
            {
                await _logService.LogAsync("INFO", $"[UPDATE SKIPPED] Ürün değişmedi: {product.Name}", trx);
                trx.Commit();
                return true;
            }

           
            await _productRepository.UpdatePriceAndStockAsync(productId, newPrice, connection, trx);

           
            await _apiService.UpdateStockAndPriceAsync(product.StockCode, newPrice, product.Stock);

            // Mark as unsynced
            await _productRepository.MarkAsUnsyncedAsync(productId, connection, trx);

            await _logService.LogAsync("INFO", $"[UPDATE SUCCESS] Ürün güncellendi: {product.Name} | Stok: {product.Stock} | Fiyat: {newPrice}", trx);
            trx.Commit();
            return true;
        }
        catch (Exception ex)
        {
            trx.Rollback();
            await _logService.LogAsync("ERROR", $"[UPDATE EXCEPTION] Ürün ID={productId} | {ex.Message}");
            return false;
        }
    }



    public async Task<bool> ProcessOrderItemAsync(OrderItem item)
    {
        using var connection = _productRepository.GetDbConnection();
        connection.Open();
        using var trx = connection.BeginTransaction();

        try
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, connection, trx);

            if (product is null || product.Stock < item.Quantity)
            {
                await _logService.LogAsync("ERROR", $"[BUY ERROR] Yetersiz stok veya ürün yok: ProductId={item.ProductId}", trx);
                trx.Rollback();
                return false;
            }

            await _productRepository.UpdateStockAsync(product.Id, item.Quantity, connection, trx);
            await _apiService.UpdateStockAndPriceAsync(product.StockCode, product.Price, product.Stock - item.Quantity);
            await _productRepository.MarkAsUnsyncedAsync(product.Id, connection, trx);
          

            trx.Commit();
            return true;
        }
        catch (Exception ex)
        {
            trx.Rollback();
            await _logService.LogAsync("ERROR", $"[BUY PROCESS EXCEPTION] {ex.Message}");
            return false;
        }
    }
}
