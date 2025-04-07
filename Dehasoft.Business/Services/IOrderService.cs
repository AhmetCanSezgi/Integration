namespace Dehasoft.Business.Services
{
    public interface IOrderService
    {
        Task FetchAndProcessOrdersAsync(int page, int size);
     
    }
}
