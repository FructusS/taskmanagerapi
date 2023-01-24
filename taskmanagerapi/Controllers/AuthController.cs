using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using taskmanagerapi.Models.UserModels;
using taskmanagerapi.Service;

namespace taskmanagerapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AuthService _authService;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, AuthService authService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
        }
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration(UserRegistrationModel userRegistrationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data not valid");
            }
            var user = new User
            {
                UserName = userRegistrationModel.Username
            };
            var createdUser = await _userManager.CreateAsync(user, userRegistrationModel.Password);
            if (createdUser.Succeeded)
            {

                var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userRegistrationModel.Username),
                        new Claim("Id", user.Id)
                    };

                var token = _authService.GenerateAccessToken(claims);
                return Ok(token);
            }
            foreach (var error in createdUser.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        
        }     
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Data not valid");
            }

            var getUser = await _userManager.FindByNameAsync(userLoginModel.Username);
            if (getUser == null)
            {
                return NotFound("user not found");
            }
            var checkUserPassword = await _signInManager.CheckPasswordSignInAsync(getUser, userLoginModel.Password, false);

            if (checkUserPassword.Succeeded)
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, userLoginModel.Username),
                        new Claim("Id", getUser.Id)
                    };

                var token = _authService.GenerateAccessToken(claims);
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}
