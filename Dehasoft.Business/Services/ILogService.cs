using System.Data;

public interface ILogService
{
    Task LogAsync(string type, string message, IDbTransaction? trx = null);
}
