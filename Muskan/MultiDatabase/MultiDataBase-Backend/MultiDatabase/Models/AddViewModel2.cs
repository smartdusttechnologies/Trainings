using System.ComponentModel.DataAnnotations;

namespace MultiDatabase.Models
{
    public class AddViewModel2
    {

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "File is required.")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits long.")]
        public string Phone { get; set; }

    }
}
