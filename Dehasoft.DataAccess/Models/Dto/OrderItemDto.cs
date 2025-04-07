namespace Dehasoft.Business.DTOs
{
    public class OrderItemDto
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
        public string sale_price { get; set; } = "0";
        public ProductBasicDto? get_product_basic { get; set; }
    }
}
