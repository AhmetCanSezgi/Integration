using AutoMapper;
using Dehasoft.Business.DTOs;
using Dehasoft.DataAccess.Models;
using Dehasoft.DataAccess.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dehasoft.Business.Services
{
    public class OrderService ( IOrderRepository _orderRepository, IProductRepository _productRepository, ApiService _apiService, IMapper _mapper,ILogService _logService) : IOrderService 
    {
       
        public async Task FetchAndProcessOrdersAsync(int page, int size)
        {
            var json = await _apiService.GetOrdersJsonAsync(page, size);
            var root = JObject.Parse(json);
            var data = root["orders"]?["data"]?.ToString();
            var appSettings=new AppSettings();
            if (string.IsNullOrEmpty(data)) return;

            var orderDtos = JsonConvert.DeserializeObject<List<OrderDto>>(data);
            if (orderDtos is null || !orderDtos.Any()) return;

            using var connection = _orderRepository.GetDbConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            
            try
            {
                foreach (var dto in orderDtos)
                {
                    var exists = await _orderRepository.ExistsByEntryIdAsync(dto.id, connection, transaction);
                    if (exists) continue;

                    var order = _mapper.Map<Order>(dto);
                    order.OrderItems = new();

                    foreach (var itemDto in dto.get_items)
                    {
                        var product = await _productRepository.GetByExternalProductIdAsync(itemDto.product_id, connection, transaction);
                        var salePrice = decimal.Parse(itemDto.sale_price);
                        if (product is null)
                        {
                            if (itemDto.get_product_basic is null) continue;

                            product = _mapper.Map<Product>(itemDto.get_product_basic);
                            product.ProductId = itemDto.product_id;
                            product.Price = salePrice;
                            product.Stock = appSettings.DefaultStock;

                            product.Id = await _productRepository.InsertAsync(product, connection, transaction);
                        }

                        var quantity = itemDto.quantity;
                   

                        if (product.Stock >= quantity)
                        {
                            order.OrderItems.Add(new OrderItem
                            {
                                ProductId = product.Id,
                                Quantity = quantity,
                                SalePrice = salePrice
                            });

                            await _orderRepository.UpdateStockAsync(product.Id, quantity, connection, transaction);
                            await _apiService.UpdateStockAndPriceAsync(product.StockCode, product.Price, product.Stock - quantity);
                        }
                        else
                        {
                            await _logService.LogAsync("ERROR", $"Yetersiz stok: {product.Name}", transaction);
                        }
                    }

                    await _orderRepository.InsertOrderAsync(order, connection, transaction);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                await _logService.LogAsync("ERROR", $"Sipariş işleme hatası: {ex.Message}", transaction);
            }
        }
    }
}
