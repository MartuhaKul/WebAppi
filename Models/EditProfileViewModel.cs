using System.ComponentModel.DataAnnotations;

namespace WebAppi.Models
{
    public class EditProfileViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}