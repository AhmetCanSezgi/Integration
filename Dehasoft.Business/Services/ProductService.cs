using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;

public class ProductService(IProductRepository productRepository, ApiService apiService, ILogService logService) : IProductService
{
    public async Task<bool> UpdateProductPriceAndStockAsync(int productId, decimal newPrice)
    {
        using var connection = productRepository.GetDbConnection();
        connection.Open();
        using var trx = connection.BeginTransaction();

        try
        {
            var product = await productRepository.GetByIdAsync(productId, connection, trx);
            if (product is null)
            {
                await logService.LogAsync("ERROR", $"Ürün bulunamadı. ID: {productId}", trx);
                trx.Rollback();
                return false;
            }

            if (product.Price == newPrice)
            {
                await logService.LogAsync("INFO", $"Ürün fiyatı zaten aynı: {product.Name}", trx);
                trx.Commit();
                return true;
            }

            await productRepository.UpdatePriceAndStockAsync(productId, newPrice, connection, trx);
            await apiService.UpdateStockAndPriceAsync(product.StockCode, newPrice, product.Stock);

            await logService.LogAsync("INFO", $"Ürün fiyatı güncellendi: {product.Name}, Yeni Fiyat: {newPrice}", trx);
            trx.Commit();
            return true;
        }
        catch (Exception ex)
        {
            trx.Rollback();
            await logService.LogAsync("ERROR", $"Fiyat güncelleme hatası: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ProcessOrderItemAsync(OrderItem item)
    {
        using var connection = productRepository.GetDbConnection();
        connection.Open();
        using var trx = connection.BeginTransaction();

        try
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, connection, trx);

            if (product is null || product.Stock < item.Quantity)
            {
                await logService.LogAsync("ERROR", $"Yetersiz stok veya ürün yok: ID={item.ProductId}", trx);
                trx.Rollback();
                return false;
            }

            await productRepository.UpdateStockAsync(product.Id, item.Quantity, connection, trx);
            await apiService.UpdateStockAndPriceAsync(product.StockCode, product.Price, product.Stock - item.Quantity);

            await logService.LogAsync("INFO", $"Satış işlemi başarılı: {product.Name}, Adet: {item.Quantity}", trx);
            trx.Commit();
            return true;
        }
        catch (Exception ex)
        {
            trx.Rollback();
            await logService.LogAsync("ERROR", $"Satış işlemi hatası: {ex.Message}");
            return false;
        }
    }
}
