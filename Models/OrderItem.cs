namespace WebAppi.Models;

public class OrderItem
{
    public int Id { get; set; }
    public int ShoeId { get; set; }
    public Shoe Shoe { get; set; } // зв'язок з кросівками
    public int Quantity { get; set; }
    public double Price { get; set; }
    
    public int OrderId { get; set; }
    public Order Order { get; set; } // зв'язок з замовленням
}