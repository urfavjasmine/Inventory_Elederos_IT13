using System.ComponentModel.DataAnnotations;

namespace Inventory_Elederos_IT13.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        [StringLength(100, ErrorMessage = "Supplier name cannot exceed 100 characters")]
        public string SupplierName { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Contact person cannot exceed 100 characters")]
        public string ContactPerson { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Product supplied cannot exceed 100 characters")]
        public string ProductSupplied { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
