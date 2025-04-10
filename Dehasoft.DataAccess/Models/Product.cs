
namespace Dehasoft.DataAccess.Models
{
    public class Product 
    {
        public int Id { get; set; }
        public int ProductId { get; set; }  
        public int Barcode { get; set; } 
        public int StockCode { get; set; }  
 
        public string Name { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal Stock { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
      
    }
}
