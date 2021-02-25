using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required]
      
        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string  Username { get; set; }
        [Required]
       [StringLength(8,MinimumLength=4)]
        public string Password { get; set; }
    }
}