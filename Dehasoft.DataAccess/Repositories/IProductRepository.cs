using Dehasoft.DataAccess.Models;
using System.Data;

namespace Dehasoft.DataAccess.Repositories
{
    public interface IProductRepository
    {
        IDbConnection GetDbConnection();
        Task UpdateStockAsync(int productId, int quantity, IDbConnection conn, IDbTransaction trx);
        Task<Product?> GetByExternalProductIdAsync(int productId, IDbConnection conn, IDbTransaction trx);
        Task<Product?> GetByIdAsync(int id, IDbConnection conn, IDbTransaction trx);
        Task<int> InsertAsync(Product product, IDbConnection conn, IDbTransaction trx);
        Task UpdatePriceAndStockAsync(int productId, decimal price, IDbConnection conn, IDbTransaction trx);
        Task<List<Product>> GetAllAsync(IDbConnection conn);
        Task InsertLogAsync(string type, string message, IDbConnection conn, IDbTransaction? trx = null);
    }
}
