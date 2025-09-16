using Domain.Models;
using HRMSmvc.Controllers;
using Microsoft.AspNetCore.Mvc;

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
            var employee = await GetAsync<List<User>>("/api/Employee/GetEmp");

            if (employee == null)
            {
                ModelState.AddModelError(string.Empty, "Error fetching employee data.");
                return View(new List<User>());
            }

            // Return the list of employees to the view
            return View(employee);
        }


        [HttpPost]
        public async Task<IActionResult> Create(User employee)
        {
            var response = await PostAsync<User>("/api/Employee/CreateEmp", employee, null);

            if (response != null)
            {
                return RedirectToAction(nameof(Index));
            }

            // If creation failed, show the form again
            ModelState.AddModelError("", "Failed to create employee.");
            return View("CreateorEdit", employee);
        }

        [HttpGet]
        public async Task<IActionResult> CreateorEdit(string Id)
        {

            var response = await GetAsync<ApiResponse<User>>("/api/Employee/GetEmpbyID?id=" + Id);

            return View(response.Data);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(User employee)
        {
            var response = await PostAsync<ApiResponse<string>>("/api/Employee/UpdateEmp", employee, null);

            return Json(response);  //Get ma ni yestai garne 
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

