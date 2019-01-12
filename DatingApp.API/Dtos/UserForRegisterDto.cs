using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos {
    public class UserForRegisterDto {

        public UserForRegisterDto () {
            Created = DateTime.Now;
            LastActive = DateTime.Now;

        }

        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength (8, MinimumLength = 5, ErrorMessage = "Password length must be between 5 to 8")]
        public string Password { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public DateTime DateOFBirth { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive { get; set; }

    }
}