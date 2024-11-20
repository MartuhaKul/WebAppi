using System.ComponentModel.DataAnnotations;

namespace WebAppi.Models;

public class Order
{
    public int Id { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string Address { get; set; }

    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public DateTime OrderDate { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } // Зв'язок з OrderItems
}
