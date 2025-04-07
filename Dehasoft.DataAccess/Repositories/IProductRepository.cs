using Dehasoft.DataAccess.Models;
using System.Data;

namespace Dehasoft.DataAccess.Repositories
{
    public interface IProductRepository
    {
        IDbConnection GetDbConnection();

        Task<Product?> GetByExternalProductIdAsync(int productId, IDbConnection conn, IDbTransaction trx);
        Task<Product?> GetByIdAsync(int id, IDbConnection conn, IDbTransaction trx);

        Task<int> InsertAsync(Product product, IDbConnection conn, IDbTransaction trx);
        Task UpdateStockAsync(int productId, int quantity, IDbConnection conn, IDbTransaction trx);

        Task<List<Product>> GetAllAsync(IDbConnection conn);
        Task<IEnumerable<Product>> GetUnsyncedProductsAsync(IDbConnection conn);

        Task MarkAsSyncedAsync(int productId, IDbConnection conn, IDbTransaction? trx = null);
        Task MarkAsUnsyncedAsync(int productId, IDbConnection conn, IDbTransaction? trx = null);


        Task InsertLogAsync(string type, string message, IDbConnection conn, IDbTransaction? trx = null);
    }
}
