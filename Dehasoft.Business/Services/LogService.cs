using Dehasoft.DataAccess.Repositories;
using System.Data;

public class LogService : ILogService
{
    private readonly IProductRepository _productRepository;

    public LogService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task LogAsync(string type, string message, IDbTransaction? trx = null)
    {
        if (trx != null)
        {
         
            await _productRepository.InsertLogAsync(type, message, trx.Connection!, trx);
        }
        else
        {
            using var conn = _productRepository.GetDbConnection();
            await _productRepository.InsertLogAsync(type, message, conn);
        }
    }

}
