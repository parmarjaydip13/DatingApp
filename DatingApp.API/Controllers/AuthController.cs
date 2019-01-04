using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers {
  
    [Route ("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IAuthRepository _repo;

        public AuthController (IAuthRepository repo) {
            _repo = repo;
        }

        [HttpPost ("Register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto) {
            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower ();

            if (await _repo.UserExists (userForRegisterDto.UserName)) {
                return BadRequest ("UserName already exists");
            }

            var UserToCreate = new User {
                UserName = userForRegisterDto.UserName,
            };

            var CreatedUser = await _repo.Register (UserToCreate, userForRegisterDto.Password);

            return StatusCode (201);
        }
    }
}