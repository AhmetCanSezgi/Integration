namespace Dehasoft.Business.DTOs
{
    public class OrderDto
    {
        public int id { get; set; }
        public string oid { get; set; } = string.Empty;
        public int user_id { get; set; }
        public string total { get; set; } = "0";
        public string order_date { get; set; } = string.Empty;
        public List<OrderItemDto> get_items { get; set; } = new();
    }
}
