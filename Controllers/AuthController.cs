using EmployeePerformance_AttendenceTracking.DTOs;
using EmployeePerformance_AttendenceTracking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeePerformance_AttendenceTracking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _Config;
        public AuthController(UserManager<ApplicationUser> userManager,IConfiguration Config)
        {
            _userManager = userManager;
            _Config = Config;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterLoginDto dto)
        {
            var userExists = await _userManager.FindByEmailAsync(dto.Email);
            if (userExists != null)
                return BadRequest("User alerady Exist");

            var user = new ApplicationUser
            {
                EmployeeId = dto.EmployeeId,
                FullName = dto.FullName,
                Email = dto.Email,
                UserName=dto.Email,
                DepartmentId = dto.DepartmentId,
                Salary = dto.Salary,
                JoinDate=DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user,dto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            
            await _userManager.AddToRoleAsync(user, "Employee");
            
            return Ok("User Registered Successfully");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var userExist = await _userManager.FindByEmailAsync(dto.Email);
            if (userExist == null)
                return Unauthorized("Invalid Email");

            var checkPass = await _userManager.CheckPasswordAsync(userExist,dto.Password);
            if (!checkPass)
                return Unauthorized("Invalid password");

            var jwtsetting =_Config.GetSection("JWT");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtsetting["Key"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userExist.Id),
                new Claim(ClaimTypes.Email, userExist.Email)
            };

            //Add Roles
            var roles =await _userManager.GetRolesAsync(userExist);
            foreach(var role in roles)
            {
                claim.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken
            (
                issuer: jwtsetting["Issuer"],
                audience: jwtsetting["Audience"],
                expires: DateTime.UtcNow.AddHours(3),
                claims:claim,
                signingCredentials: cred

             );

            var jwttoken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = jwttoken,message=$"Login Successfull by {userExist.FullName}  {string.Join(", ",roles)}" });
        }
    }
}
