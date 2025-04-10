using Dehasoft.DataAccess.Models.Dto;
using Newtonsoft.Json;
using System.Text;

public class ApiService
{
    private readonly HttpClient _client;
    private readonly ILogService _logService;

    public ApiService(string apiKey, string apiSecret, ILogService logService)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("Dehasoft-Api-Key", apiKey);
        _client.DefaultRequestHeaders.Add("Dehasoft-Api-Secret", apiSecret);
        _logService = logService;
    }

    public async Task<string> GetOrdersJsonAsync(int page, int size)
    {
        var body = JsonConvert.SerializeObject(new { page, size });
        var api = new DehaSoftApi();
        try
        {
            var response = await _client.PostAsync(
                api.Listing,
                new StringContent(body, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode(); 
            var content = await response.Content.ReadAsStringAsync();

            await _logService.LogAsync("INFO", $"[ORDER FETCH] Sayfa: {page}, Boyut: {size} - Başarılı");
            return content;
        }
        catch (Exception ex)
        {
            await _logService.LogAsync("ERROR", $"[ORDER FETCH ERROR] Sayfa: {page}, Boyut: {size} - {ex.Message}");
            return string.Empty; 
        }
    }


    public async Task UpdateStockAndPriceAsync(int stockCode, decimal price, decimal stock)
    {


        var payload = new
        {
            product = stockCode,
            price,
            price_id = 1,
            stock,
            matchMode = 1
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

        try
        {
            var api = new DehaSoftApi();
            
            var response = await _client.PostAsync(
                api.Updating,
                content);

            response.EnsureSuccessStatusCode();

            await _logService.LogAsync("INFO", $"[STOCK UPDATE] StockCode={stockCode} -> SatisFiyati: {price} | Kalan Stok: {stock}");
        }
        catch (Exception ex)
        {
            await _logService.LogAsync("ERROR", $"[STOCK UPDATE ERROR] StockCode={stockCode} -> {ex.Message}");
        }
    }
}
