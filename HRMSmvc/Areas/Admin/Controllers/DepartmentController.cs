using Domain.DTO;
using Domain.DTO.DepartmentDTO;
using Domain.Models;
using HRMSmvc.Controllers;
using HRMSmvc.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DepartmentController : BaseController
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await GetAsync<ApiResponse<List<DeptInfoDTO>>>("/api/Department/GetAllDept");
            if (departments == null)
            {
                ModelState.AddModelError(string.Empty, "Error fetching department data.");
                return View(new List<DeptInfoDTO>());
            }
            // Return the list of departments to the view
            return View(departments.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] DeptInfoDTO department)
        {
            var response = await PostAsync<ApiResponse<DeptInfoDTO>>("/api/Department/CreateDept", department, null);
            return Json(response);  // Always return JSON
        }

        [HttpGet]
        public async Task<IActionResult> CreateorEdit(string Id)
        {
            if (Id != null)
            {
                var response = await GetAsync<ApiResponse<DeptInfoDTO>>("/api/Department/GetDeptById?id=" + Id);
                return View(response.Data);
            }
            else
            {
                var dept = new DeptInfoDTO
                {
                    //Id = null // Ensure Id is not set
                };
                return View(dept);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] DeptInfoDTO department)
        {
            var response = await PostAsync<ApiResponse<DeptInfoDTO>>("/api/Department/UpdateDept", department, null);
            return Json(response);  // Always return JSON
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var response = await PostAsync<ApiResponse<DeptInfoDTO>>("/api/Department/DeleteDept?id=" + Id, null, null);
            return Json(response);  // Always return JSON
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeesToDepartment(Guid departmentId, List<string> employeeIds)
        {
            // Prepare request body
            var requestBody = new
            {
                departmentId = departmentId,
                employeeIds = employeeIds
            };

            // Call API with JSON body
            var response = await PostAsync<ApiResponse<string>>(
                "/api/Department/AddEmpToDept",
                requestBody,
                null // no file to upload
            );

            return Json(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetEmpExceptDeptId(Guid id)
        {
            var emp = await GetAsync<ApiResponse<List<UserDTO>>>("/api/Department/GetEmpExceptDeptId?id=" + id);

            var dept = await GetAsync<ApiResponse<DeptInfoDTO>>("/api/Department/GetDeptById?id=" + id);

            var vm = new AddEmpToDeptViewModel
            {
                DepartmentId = id,
                DepartmentName = dept?.Data?.Name ?? "Unknown Department",
                Employees = emp.Data ?? new List<UserDTO>()
            };

            return PartialView("_AddEmpToDept", vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpByDeptId(Guid id)
        {
            var emp = await GetAsync<ApiResponse<List<UserDTO>>>("/api/Department/GetAllEmpInDept?deptId=" + id);
            return View("DeptEmpList", emp.Data);
        }
    }
}
