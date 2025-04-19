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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            var token = await _userServices.Login(loginDto);

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { message = "Invalid email or password" });

            return Ok(new { token });
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetALL()
        {
            var users = await _userServices.GetAll();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userServices.GetById(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UserRequest userRequest)
        {
            var issuccess = await _userServices.Update(id, userRequest);
            if (!issuccess)
                return BadRequest("User update failed");
            return Ok("User updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var issuccess = await _userServices.Delete(id);
            if (!issuccess)
                return BadRequest("User deletion failed");
            return Ok("User deleted successfully");
        }

    }

}
