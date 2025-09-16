//using Domain.DTO;
//using Domain.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//namespace HRMSmvc.Controllers
//{
//    public class EmployeeController : BaseController
//    {

//        private readonly HttpClient client;
//        private readonly IConfiguration _configuration;

//        public EmployeeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : base(configuration, httpContextAccessor)
//        {
//            _configuration = configuration;
//        }

//        //public EmployeeController(IHttpClientFactory httpClientFactory)
//        //{
//        //    client = httpClientFactory.CreateClient("Api");
//        //}
//        public async Task<IActionResult> Index()
//        {
//            var employee = await GetAsync<List<User>>("/api/Employee/GetEmp");

//            if (employee == null)
//            {
//                ModelState.AddModelError(string.Empty, "Error fetching employee data.");
//                return View(new List<User>());
//            }

//            // Return the list of employees to the view
//            return View(employee);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create(User employee)
//        {
//            var response = await PostAsync<User>("/api/Employee/CreateEmp", employee, null);

//            if (response != null)
//            {
//                return RedirectToAction(nameof(Index));
//            }

//            // If creation failed, show the form again
//            ModelState.AddModelError("", "Failed to create employee.");
//            return View("CreateorEdit", employee);
//        }

//        [HttpGet]
//        public async Task<IActionResult> CreateorEdit(string Id)
//        {

//            var response = await GetAsync<ApiResponse<User>>("/api/Employee/GetEmpbyID?id=" +Id);

//            return View(response.Data);
//            //if (employee.Id == Guid.Empty)
//            //{
//            //    return View("CreateorEdit");
//            //}

//            //var employee = await GetAsync<User>($"/api/Employee/GetEmpbyID?id={Id}");

//            //if (employee == null)
//            //{
//            //    ModelState.AddModelError(string.Empty, "Employee not found.");
//            //    return View();
//            //}

//            //return View("CreateorEdit", employee);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Edit(User employee)
//        {
//            var response = await PostAsync<ApiResponse<string>>("/api/Employee/UpdateEmp", employee, null);

//            return Json(response);  //Get ma ni yestai garne 
//        }

//        [HttpPost]
//        public async Task<IActionResult> Delete(Guid Id)
//        {
//            var employee = await GetAsync<User>($"/api/Employee/GetEmpbyID?id={Id}");
//            if (employee == null)
//            {
//                ModelState.AddModelError(string.Empty, "Employee not found.");
//                return View("Index");
//            }

//            // Use the HttpClient instance to send a DELETE request
//            var response = await client.DeleteAsync($"/api/Employee/DeleteEmp?id={Id}");
//            if (!response.IsSuccessStatusCode)
//            {
//                ModelState.AddModelError(string.Empty, "Error deleting employee.");
//                return View("Index");
//            }

//            return Json(new { success = true, message = "Employee deleted successfully." });
//        }
//        [HttpGet]
//        public async Task<IActionResult> UserIndex()
//        {
//            // Get the logged-in user's ID from claims
//            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

//            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
//            {
//                // User ID claim not found or invalid
//                return RedirectToAction("Login", "Auth");
//            }

//            // Call API to get user by ID
//            var user = await GetAsync<User>($"/api/Employee/GetEmpbyID?id={userId}");

//            if (user == null)
//            {
//                // Handle user not found scenario
//                return View("Error", "User not found");
//            }
//            ViewData["APIurl"] =  _configuration.GetValue<string>("ApiSettings:BaseUrl");

//            return View(user);  // Pass the user model to the view
//        }

//    }
//}
