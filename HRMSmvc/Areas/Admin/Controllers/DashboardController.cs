using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HRMSmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashboardController : BaseController
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public DashboardController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }
        public async Task<IActionResult> Index()
        {
            var employee = await GetAsync<ApiResponse<List<UserDTO>>> ("/api/Employee/GetEmp");

            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Error fetching employee data.");
                return View(new List<User>());
            }

            // Return the list of employees to the view
            return View(employee.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] User employee)
        {
            var response = await PostAsync<ApiResponse<UserDTO>>("/api/Employee/CreateEmp", employee, null);
            return Json(response);  // Always return JSON
        }


        [HttpGet]
        public async Task<IActionResult> CreateorEdit(string Id)
        {
            if (Id != null)
            {
                var response = await GetAsync<ApiResponse<User>>("/api/Employee/GetEmpbyID?id=" + Id);

                return View(response.Data);
            }
            else
            {
                var user = new User
                {
                    DOB = DateTime.Today,
                    JoiningDate = DateTime.Today,
                    Id = null // Ensure Id is not set
                };
                return View(user);

            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] User employee)
        {
            var response = await PostAsync<ApiResponse<UserDTO>>("/api/Employee/UpdateEmp", employee, null);

            return Json(response);  
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var employee = await GetAsync<User>($"/api/Employee/GetEmpbyID?id={Id}");
            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Employee not found.");
                return View("Index");
            }

            // Use the HttpClient instance to send a DELETE request
            var response = await client.DeleteAsync($"/api/Employee/DeleteEmp?id={Id}");
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Error deleting employee.");
                return View("Index");
            }

            return Json(new { success = true, message = "Employee deleted successfully." });
        }
    }
}

