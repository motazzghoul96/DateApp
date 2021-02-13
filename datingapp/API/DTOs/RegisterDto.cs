using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string  Username { get; set; }
        [Required]
       
        public string Password { get; set; }
    }
}