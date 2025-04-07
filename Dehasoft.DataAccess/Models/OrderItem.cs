namespace Dehasoft.DataAccess.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal SalePrice { get; set; }
        public Product? Product { get; set; } 
    }
}
