using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DKS_API.Controllers;
using DKS_API.DTOs;
using DKS_API.Services.Implement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DFPS.API.Controllers
{
    [Authorize]
    public class UsersController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;
        public UsersController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetAllAsync();
            var usersToReturn = _mapper.Map<IEnumerable<UserDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(int id)
        {
            var user = _authService.GetById(id);
            var userToReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userToReturn);
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser(UserForUpdateDto userForUpdateDto)
        {
            //if the user of login is not match the useforupdateDto
            if (userForUpdateDto.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromDAO =  _authService.GetById(userForUpdateDto.Id);

            _mapper.Map(userForUpdateDto, userFromDAO);

            if (await _authService.Update(userFromDAO))
                return NoContent();

            throw new Exception($"UpdateUser API Error On Server");
        }
        

    }
}