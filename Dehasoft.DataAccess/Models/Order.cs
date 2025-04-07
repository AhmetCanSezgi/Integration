namespace Dehasoft.DataAccess.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public string Oid { get; set; } = default!;
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
