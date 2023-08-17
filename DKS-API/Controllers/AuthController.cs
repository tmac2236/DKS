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
using DKS.API.Models.DKS;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Http;
using Newtonsoft.Json;

namespace DKS_API.Controllers
{
    public class AuthController : ApiController
    {

        private readonly IDKSDAO _dksDAO;
        private readonly IDtrLoginHistoryDAO _dtrLoginHistoryDAO;
        public AuthController(IConfiguration config, IWebHostEnvironment webHostEnvironment, ILogger<AuthController> logger,
         IDKSDAO dksDAO,IDtrLoginHistoryDAO dtrLoginHistoryDAO)
                 : base(config, webHostEnvironment,logger)
        {
            _dksDAO = dksDAO;
            _dtrLoginHistoryDAO = dtrLoginHistoryDAO;
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "DKS外掛Web用的API",
            Description = "用UserId登入"
        )]        
        public async Task<IActionResult> Login(UserDto userForLoginDto)
        {
            _logger.LogInformation(String.Format(@"******  AuthController Login fired!! ******"));
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };

            using (var client = new HttpClient(httpClientHandler))
            {

                HttpResponseMessage res = client.GetAsync(String.Format(@"http://10.4.0.39:8080/ArcareAccount/Validate?account={0}&password={1}",userForLoginDto.Account,userForLoginDto.Password)).Result;
                if (res.IsSuccessStatusCode)
                {
                    var value = res.Content.ReadAsStringAsync().Result;
                    var jsonObj = JsonConvert.DeserializeAnonymousType(value, new { result = false, error = "" });
                    if(!jsonObj.result) return Unauthorized();
                }else{
                    return Unauthorized();
                }

            }
            var userFromRepo = await _dksDAO.GetRolesByAccount(userForLoginDto.Account);
            if (userFromRepo.Count < 1)
            {
                return Unauthorized();
            }
            //}
            IEnumerable<string> onlyGroupNos = from u in userFromRepo 
                                select u.GROUPNO ;
            string roleArray = string.Join(".",onlyGroupNos);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo[0].USERID.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo[0].LOGIN),
                new Claim(ClaimTypes.Role, roleArray),
                new Claim(ClaimTypes.Actor, userFromRepo[0].FACTORYID),
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
        [HttpPost("loginRecord")]
        public async Task<IActionResult> LoginRecord(DtrLoginHistoryDto dtrLoginHistoryDto)
        {
            DtrLoginHistory aRecord = new DtrLoginHistory();
            aRecord.Account = dtrLoginHistoryDto.Account;
            aRecord.SystemName = dtrLoginHistoryDto.SystemName;
            aRecord.PcName ="";
            aRecord.IP = HttpContext.Connection.RemoteIpAddress.ToString();
            aRecord.LoginTime = DateTime.Now;
            _dtrLoginHistoryDAO.Add(aRecord);
            await _dtrLoginHistoryDAO.SaveAll();
            return Ok();
        }
    }
}