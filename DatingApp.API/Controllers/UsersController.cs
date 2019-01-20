using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Helper;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers {

    [ServiceFilter (typeof (LogUserActivity))]
    [Authorize]
    [Route ("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController (IDatingRepository repo, IMapper mapper) {
            _mapper = mapper;
            _repo = repo;
        }

        public async Task<IActionResult> GetUsers ([FromQuery] UserParms userParms) {

            var currentUserid = int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value);
            var userFromRepo = await _repo.GetUser (currentUserid);

            userParms.UserId = currentUserid;

            if (string.IsNullOrEmpty (userParms.Gender)) {
                userParms.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers (userParms);
            var usesToReturn = _mapper.Map<IEnumerable<UserForListsDto>> (users);

            Response.AddPagination (users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPage);

            return Ok (usesToReturn);
        }

        [HttpGet ("{id}", Name = "GetUser")]
        public async Task<IActionResult> GetUser (int id) {
            var user = await _repo.GetUser (id);
            var userToReturn = _mapper.Map<UserForDetaildDto> (user);

            return Ok (userToReturn);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateUser (int id, UserForUpdateDto userForUpdateDto) {
            if (id != int.Parse (User.FindFirst (ClaimTypes.NameIdentifier).Value))
                return Unauthorized ();

            var userFromRepo = await _repo.GetUser (id);

            _mapper.Map (userForUpdateDto, userFromRepo);

            if (await _repo.SaveAll ()) {
                return NoContent ();
            }
            throw new System.Exception ($"Updating user {id} failed on save.");
        }
    }
}