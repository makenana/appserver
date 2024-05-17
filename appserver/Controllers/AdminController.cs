using Microsoft.AspNetCore.Mvc;
using CityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using appserver.DTO;
using System.IdentityModel.Tokens.Jwt;

namespace appserver.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(UserManager<CityParkUser> userManager,
        JwtHandler jwtHandler) : ControllerBase
    {
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            CityParkUser? user = await userManager.FindByNameAsync(loginRequest.UserName);
            if (user == null)
            {
                return Unauthorized("Bad username :(");
            }
            bool success = await userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (!success)
            {
                return Unauthorized("Wrong password :/");
            }
            JwtSecurityToken token = await jwtHandler.GetTokenAsync(user);
            string jwtString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResult
            {
                Success = true,
                Message = "swEETTT",
                Token = jwtString
            });
        }
    }
}
