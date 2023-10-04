using System.ComponentModel.DataAnnotations;

namespace DCE_Backend_Developer_Assesment.DTO.Requests
{
  
        public class CustomerRegistrationRequest
        {
            [Required(ErrorMessage = "Username is required.")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Email is required.")]
            [EmailAddress(ErrorMessage = "Invalid email address.")]
            public string Email { get; set; }

            [Required(ErrorMessage = "First Name is required.")]
            [StringLength(50, ErrorMessage = "First Name should be less than 50 characters.")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last Name is required.")]
            [StringLength(50, ErrorMessage = "Last Name should be less than 50 characters.")]
            public string LastName { get; set; }
        }

    }

