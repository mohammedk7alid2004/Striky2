using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Striky2.Dto.Request;
using Striky2.Services.Users;

namespace Striky2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] UserRequest userRequest)
        {
            var issuccess = await _userServices.Create(userRequest);
            if (!issuccess)
            {
                return BadRequest("User registration failed");
            }

            return Ok("User registered successfully");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var issuccess = await _userServices.Login(loginDto);
            if (!issuccess)
            {
                return BadRequest("Login failed");
            }
            return Ok("Login successful");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetALL()
        {
            var users = await _userServices.GetAll();
            return Ok(users);
        }
        public IActionResult Logoout()
        {
            return Ok();
        }
    }
}
