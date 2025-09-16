using HRMSwebAPI.Data;
using HRMSwebAPI.DTO;
using HRMSwebAPI.Models;
using HRMSwebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Claims;

namespace HRMSwebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        //private readonly IAuthService service;
        //public UserController(IAuthService service) {
        //                this.service = service;
        //}

        private readonly IAuthService service;
        private readonly AppDbContext context;
        public UserController(IAuthService service, AppDbContext context) 
        {
            this.service = service;
            this.context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User?>> Register( UserDto request)
        {
            var user = await service.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest("User already exists");
            }
            return Ok( new UserProfileDto
            {
                Name = user.Name,
                Email = user.Email,
                //UserRoles = user.ToList()
            });
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto request)
        {
            var response = await service.LoginAsync(request);

            if (response is null)
            {
                return BadRequest("Invalid email or password.");
            }
            return Ok(response);
        }

        [Authorize(Roles= "Admin")]
        [HttpGet("details")]
        public IActionResult GetDetails()
        {
            //var claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "NameIdentifier");
            //if (claim is null)
            //{
            //    return BadRequest(error: "User not found.");
            //}
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var userId = claim.Value;
            //var response = context.Users.Find(userId);
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return BadRequest("Invalid user ID format.");
            }

            var response = context.Users.Find(parsedUserId);
            if (response == null)
            {
                return NotFound("User not found.");
            }
            return Ok(response);

        }
    }
}
