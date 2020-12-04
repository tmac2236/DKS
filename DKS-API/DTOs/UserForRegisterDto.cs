using System.ComponentModel.DataAnnotations;

namespace DFPS.API.DTOs
{
    public class UserForRegisterDto
    {
        [Required]
        public string Account { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "you must specify password between 4 and 8.")]
        public string Password { get; set; }

    }
}