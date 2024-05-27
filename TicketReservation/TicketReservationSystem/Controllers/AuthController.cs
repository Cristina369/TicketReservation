using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketReservationSystem.Models.DTO;
using TicketReservationSystem.Repositories.Interface;

namespace TicketReservationSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            var identityUser = await userManager.FindByEmailAsync(loginRequest.Email); 
            if (identityUser != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, loginRequest.Password);
                
                if(checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    var jwtToken = tokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new LoginResponseDto()
                    {
                        Email = loginRequest.Email,
                        Roles = roles.ToList(),
                        Token = jwtToken
                    };

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");

            return ValidationProblem(ModelState);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            var user = new IdentityUser
            {
                UserName = registerRequest.Email?.Trim(),
                Email = registerRequest.Email?.Trim()
            };

            var identityResult = await userManager.CreateAsync(user, registerRequest.Password);

            if(identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    if(identityResult.Errors.Any())
                    {
                        foreach(var error in identityResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

            }
            return ValidationProblem(ModelState);

        }
    }
}
