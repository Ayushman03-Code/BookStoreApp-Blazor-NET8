using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.User;
using BookStoreApp.API.Static;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStoreApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> logger;
        private readonly IMapper mapper;
        private readonly UserManager<ApiUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(ILogger<AuthController> logger, IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            logger.LogInformation($"Registration Attempt for (user, userDto.Password");
            try
            {
                //if (userDto == null)
                //{
                //    return BadRequest("Insufficient Data Provided");
                //}
                var user = mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;
                var result = await userManager.CreateAsync(user, userDto.Password);

                if (!result.Succeeded == false)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);

                    }
                    return BadRequest(ModelState);
                }
                //userManager.Claim
                await userManager.AddToRoleAsync(user, "User");// we want to await user manager to add to role async and then we give it the user that was just created as well as the role we want to add them to
                return Accepted();
            }
            catch (Exception ex)
            {
                // what we would catch is an exception which we would want to log
                logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthorResponse>>Login(LoginUserDto userDto)
        {
            logger.LogInformation($"Login Attempt for {userDto.Email}");
            try
            {
                var user = await userManager.FindByEmailAsync(userDto.Email);
                var passwordValid = await userManager.CheckPasswordAsync(user, userDto.Password);
                if(user == null || passwordValid == false)
                {
                    return Unauthorized(userDto);
                }

                string tokenString = await GenerateToken(user); // Generating tokenString from JWT
                var response = new AuthorResponse
                {
                    Email = userDto.Email,
                    Token = tokenString,
                    UserId = user.Id
                };
                return response;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, $"Something went wrong in the {nameof(Register)}");
                return Problem($"Something went wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        private async Task<string> GenerateToken(ApiUser user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings : Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(q => new Claim(ClaimTypes.Role, q)).ToList();
            var userClaims = await userManager.GetClaimsAsync(user);
            // now creating claims: claims are bits fo information that will let the system know who you are, what you do etc, likeyour name, role 
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(CustomClaimTypes.Uid, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                    issuer: configuration["JwtSettings: Issuer"],
                    audience: configuration["JwtSettings: Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(Convert.ToInt32(configuration["JwtSettings: Duration"])),
                    signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
