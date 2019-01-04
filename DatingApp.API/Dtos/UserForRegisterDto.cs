using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
       [Required]
        public string UserName { get; set; }
       
       [Required]
       [StringLength(8,MinimumLength=5,ErrorMessage="Password length must be between 5 to 8")]
        public string Password { get; set; }

    }
}