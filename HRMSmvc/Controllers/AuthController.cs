using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMSmvc.Controllers
{
    public class AuthController : BaseController
    {

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IConfiguration configuration, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor,  ILogger<BaseController> logger) : base(configuration, httpContextAccessor , logger)
        {
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterDTO registerDTO)
        {
            //var json = System.Text.Json.JsonSerializer.Serialize(registerDTO);
            //var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await PostAsync<ApiResponse<string>>("/api/Auth/Register", registerDTO, registerDTO.formFile);

            return Json(response);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("register");
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            var response = await PostAsync<ApiResponse<LoginResponseDto>>("/api/Auth/Login", loginDTO, null);

            // If the login is successful (API responds with success)
            if (response != null && response.success)
            {
                var loginResponse = response.Data;

                // Assuming you receive user info or token in the response

                // Create a list of claims (user info + roles)
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Hash, loginResponse.Token),  // User Id claim
                        new Claim(ClaimTypes.NameIdentifier, loginResponse.Id),  // Id claim
                    };

                // Add roles to the claims
                foreach (var role in loginResponse.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));  // Role claim
                }

                // Create a ClaimsIdentity with these claims and authentication type
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Create a ClaimsPrincipal with the ClaimsIdentity
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // Sign the user in by creating the authentication cookie
                await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                if (loginResponse.Roles.Contains("Admin"))
                {
                    response.message = Url.Action("Index", "Dashboard", new { area = "Admin" });
                }
                else if (loginResponse.Roles.Contains("User"))
                {
                    response.message = Url.Action("UserIndex", "Dashboard", new { area = "Client" });
                }


                // Redirect to the Employee index page after successful login
                return Json(response);
            }
            else
            {
                // If the login attempt failed, add an error to ModelState
                return Json(response);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("login");
        }





    }
}