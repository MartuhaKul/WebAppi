namespace WebAppi.Models;

public class Order
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public DateTime OrderDate { get; set; }

    // Зв'язок з елементами замовлення
    public ICollection<OrderItem> OrderItems { get; set; }
}
