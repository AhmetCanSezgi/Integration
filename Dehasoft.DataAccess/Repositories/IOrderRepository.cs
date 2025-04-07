using Dehasoft.DataAccess.Models;
using System.Data;

namespace Dehasoft.DataAccess.Repositories
{
    public interface IOrderRepository
    {
        Task InsertOrderAsync(Order order, IDbConnection conn, IDbTransaction transaction);
        Task<bool> ExistsByEntryIdAsync(int entryId, IDbConnection conn, IDbTransaction trx);
        IDbConnection GetDbConnection();
        Task UpdateStockAsync(int productId, int quantity, IDbConnection connection, IDbTransaction transaction);
       
    }
}
