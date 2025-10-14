using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using HRMSmvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HRMSmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PayrollController : BaseController
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public PayrollController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> ViewPayroll(string userId)
        {
            var response = await GetAsync<ApiResponse<UserPayrollDTO>>($"/api/Payroll/GetPayrollByEmpID?EmpId={userId}");
            if (response != null && response.success)
            {
                return View("PayrollList", response.Data);
            }
            return View("PayrollList", new UserPayrollDTO());
        }

        [HttpGet]

        public async Task<IActionResult> GeneratePayroll(string userId)
        {
            var user = new PayrollDTO
            {
                UserId = userId
            };

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> GeneratePayroll([FromForm] PayrollDTO payroll)
        {
           var response = await PostAsync<ApiResponse<PayrollDTO>>("/api/Payroll/CreatePayroll", payroll, null);
            return Json(response); 
        }
    }
}
