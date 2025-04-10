using Dapper;
using Dehasoft.DataAccess.Models;
using System.Data;

namespace Dehasoft.DataAccess.Repositories
{
    public class ProductRepository(DatabaseContext _db) : IProductRepository
    {
    
        public IDbConnection GetDbConnection() => _db.CreateConnection();

        public async Task<Product?> GetByExternalProductIdAsync(int productId, IDbConnection conn, IDbTransaction trx)
        {
            const string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
            return await conn.QueryFirstOrDefaultAsync<Product>(query, new { ProductId = productId }, trx);
        }

        public async Task<int> InsertAsync(Product product, IDbConnection conn, IDbTransaction trx)
        {
            const string query = @"
                INSERT INTO Products (ProductId, Barcode, StockCode, Name, Price, Stock)
                VALUES (@ProductId, @Barcode, @StockCode, @Name, @Price, @Stock);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await conn.ExecuteScalarAsync<int>(query, product, trx);
        }

        public async Task UpdateStockAsync(int productId, int quantity, IDbConnection conn, IDbTransaction trx)
        {
            const string query = "UPDATE Products SET Stock = Stock - @Qty WHERE Id = @Id";
            await conn.ExecuteAsync(query, new { Qty = quantity, Id = productId }, trx);
        }
        public async Task UpdatePriceAndStockAsync(int productId, decimal price, IDbConnection conn, IDbTransaction trx)
        {
            const string query = "UPDATE Products SET Price = @Price  WHERE Id = @Id";
            await conn.ExecuteAsync(query, new { Price = price, Id = productId }, trx);
        }

        public async Task<List<Product>> GetAllAsync(IDbConnection conn)
        {
            const string query = "SELECT * FROM Products ORDER BY Name";
            var products = await conn.QueryAsync<Product>(query);
            return products.AsList();
        }

    

        public async Task MarkAsUnsyncedAsync(int productId, IDbConnection conn, IDbTransaction? trx = null)
        {
            const string query = "UPDATE Products SET IsSynced = 1 WHERE Id = @Id";
            await conn.ExecuteAsync(query, new { Id = productId }, trx);
        }

        public async Task InsertLogAsync(string type, string message, IDbConnection conn, IDbTransaction? trx = null)
        {
            const string query = @"
        INSERT INTO Logs (LogTime, Type, Message)
        VALUES (@LogTime, @Type, @Message);";

            await conn.ExecuteAsync(query, new
            {
                LogTime = DateTime.Now,
                Type = type,
                Message = message
            }, trx);
        }
        public async Task<Product?> GetByIdAsync(int id, IDbConnection conn, IDbTransaction trx)
        {
            const string query = "SELECT * FROM Products WHERE Id = @Id";
            return await conn.QueryFirstOrDefaultAsync<Product>(query, new { Id = id }, trx);
        }

       
    }
}
