using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class AttendanceController : BaseController
    {

        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public AttendanceController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttendanceByEmpID(string userId)
        {
            var response = await GetAsync<ApiResponse<UserAttendanceDTO>>($"/api/Attendance/GetAttendanceByEmpID?userId={userId}");
            return View("AttendanceIndex", response.Data);
        }
    }
}
