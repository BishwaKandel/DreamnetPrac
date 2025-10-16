using Application.Interface;
using Domain.DTO;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IImageService _imageService;
        public AuthController(IAuthService authService , IImageService imageservice)
        {
            _authService = authService;
            _imageService = imageservice;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegisterDTO dto)
        {
            string profileImagePath = null;

            if (dto.formFile != null)
            {
                profileImagePath = await _imageService.UploadImageAsync(dto.formFile);
                dto.ProfilePictureFileName = profileImagePath;
            }

            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(result);
        }

        [HttpPost("role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var result = await _authService.CreateRoleAsync(roleName);
            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutUserAsync();
            return Ok(result);
        }
    }
}
