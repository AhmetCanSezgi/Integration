using Newtonsoft.Json;
using System.Text;

public class ApiService
{
    private readonly HttpClient _client;

    public ApiService(string apiKey, string apiSecret)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Dehasoft-Api-Key", apiKey);
        _client.DefaultRequestHeaders.Add("Dehasoft-Api-Secret", apiSecret);
    }

    public async Task<string> GetOrdersJsonAsync(int page, int size)
    {
        var body = JsonConvert.SerializeObject(new { page, size });
        var response = await _client.PostAsync(
            "https://staj.dehapi.com/api/seller/order/get",
            new StringContent(body, Encoding.UTF8, "application/json"));

        return await response.Content.ReadAsStringAsync();
    }

    public async Task UpdateStockAndPriceAsync(int StockCode, decimal price, decimal stock)
    {
        var payload = new
        {
            product = StockCode,
            price,
            price_id = 1,
            stock,
            matchMode = 1
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        await _client.PostAsync(
            "https://staj.dehapi.com/api/seller/product/update-stock-and-price",
            content);
    }
}
