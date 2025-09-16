using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HRMSmvc.Areas.Client.Controllers
{
    [Area("Client")]
    public class DashboardController : BaseController
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> UserIndex()
        {
            // Get the logged-in user's ID from claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                // User ID claim not found or invalid
                return RedirectToAction("Login", "Auth");
            }
            // Call API to get user by ID
            var response = await GetAsync<ApiResponse<UserDTO>>($"/api/Employee/GetEmpbyID?id={userId}");

            if (response == null || !response.success || response.Data == null)
            {
                // Handle user not found scenario
                return View("Error", "User not found");
            }
            ViewData["APIurl"] = _configuration.GetValue<string>("ApiSettings:BaseUrl");

            return View(response.Data);  // Pass the user model to the view
        }


        //Change Photo
        [HttpPost]
        public async Task<IActionResult> ChangePhoto(IFormFile formFile, ChangePpDTO employee)
        {

            var response = await PostAsync<ApiResponse<UserDTO>>("/api/Employee/ChangePhoto" , employee , formFile);
            return Json(response);

        }

        [HttpPost]
        public async Task<IActionResult> DeletePhoto(string id)
        {
            var dto = new ChangePpDTO { Id = id };
            var response = await PostAsync<ApiResponse<UserDTO>>("/api/Employee/DeletePhoto", dto, null);
            return Json(response);
        }


        //[HttpGet]
        //public async Task<IActionResult> ChangePhoto(string Id)
        //{

        //    var response = await GetAsync<ApiResponse<UserDTO>>("/api/Employee/GetEmpbyID?id=" + Id);

        //   return View("Edit", response.Data);

        //}

        //EditPage
        [HttpPost]
        public async Task<IActionResult> Edit(UserDTO employee)
        {
            var response = await PostAsync<ApiResponse<UserDTO>>("/api/Employee/UpdateEmp", employee ,null);//, employee.formFile);

            return Json(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string Id)
        {

            var response = await GetAsync<ApiResponse<UserDTO>>("/api/Employee/GetEmpbyID?id=" + Id);

            return View("UserIndex", response.Data);

        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            var response = await PostAsync<ApiResponse<UserDTO>>("/api/Employee/ChangePassword", changePasswordDTO, null);
            return Json(response);
        }

        [HttpGet]

        public async Task<IActionResult> ChangePassword(string Id)
        {
            var response = await GetAsync<ApiResponse<UserDTO>>("/api/Employee/GetEmpbyID?id=" + Id);
            return View("UserIndex", response.Data);

        }
    }
}
