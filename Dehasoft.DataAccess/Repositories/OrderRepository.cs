using Dapper;
using Dehasoft.DataAccess.Models;
using System.Data;

namespace Dehasoft.DataAccess.Repositories
{
    public class OrderRepository (DatabaseContext _db) : IOrderRepository
    {
       
        public IDbConnection GetDbConnection() => _db.CreateConnection();

        public async Task InsertOrderAsync(Order order, IDbConnection conn, IDbTransaction transaction)
        {
            const string queryOrder = @"
                INSERT INTO Orders (EntryId, Oid, UserId, Total, OrderDate) 
                VALUES (@EntryId, @Oid, @UserId, @Total, @OrderDate);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            order.Id = await conn.ExecuteScalarAsync<int>(queryOrder, order, transaction);

            foreach (var item in order.OrderItems)
            {
                const string queryItem = @"
                    INSERT INTO OrderItems (OrderId, ProductId, Quantity, SalePrice)
                    VALUES (@OrderId, @ProductId, @Quantity, @SalePrice);";

                await conn.ExecuteAsync(queryItem, new
                {
                    OrderId = order.Id,
                    item.ProductId,
                    item.Quantity,
                    item.SalePrice
                }, transaction);
            }
        }

        public async Task<bool> ExistsByEntryIdAsync(int entryId, IDbConnection conn, IDbTransaction trx)
        {
            const string query = "SELECT COUNT(1) FROM Orders WHERE EntryId = @EntryId";
            return await conn.ExecuteScalarAsync<bool>(query, new { EntryId = entryId }, trx);
        }

        public async Task UpdateStockAsync(int productId, int quantity, IDbConnection connection, IDbTransaction transaction)
        {
            const string query = "UPDATE Products SET Stock = Stock - @Qty WHERE Id = @Id";
            await connection.ExecuteAsync(query, new { Qty = quantity, Id = productId }, transaction);
        }
    }
}
