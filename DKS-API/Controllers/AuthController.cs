using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using DKS_API.Data.Interface;
using DKS_API.DTOs;
using DKS.API.Models.DKSSys;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace DKS_API.Controllers
{
    public class AuthController : ApiController
    {

        private readonly IDKSDAO _dksDAO;
        public AuthController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger, IDKSDAO dksDAO)
                 : base(config, webHostEnvironment,logger)
        {
            _dksDAO = dksDAO;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userForLoginDto)
        {
            _logger.LogInformation(String.Format(@"******  AuthController Login fired!! ******"));
            //var userFromRepo = await _dksDAO.SearchStaffByLOGIN(userForLoginDto.Account);

            //if (userFromRepo == null)
            //{
            //改成只能用UserId登入
            decimal userId = Decimal.Parse(userForLoginDto.Account);
            var userFromRepo = await _dksDAO.GetRolesByUserId(userForLoginDto.Account);
            if (userFromRepo.Count < 1)
            {
                return Unauthorized();
            }
            //}
            IEnumerable<string> onlyGroupNos = from u in userFromRepo 
                                select u.GROUPNO ;
            string roleArray = string.Join(".",onlyGroupNos);

            var roles = _dksDAO.GetRolesByUserId(userFromRepo[0].USERID.ToString());
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo[0].USERID.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo[0].LOGIN),
                new Claim(ClaimTypes.Role, roleArray)
                };
            var tokenName = _config.GetSection("AppSettings:Token").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(tokenName));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),  // the expire time is one day.
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });

        }
    }
}