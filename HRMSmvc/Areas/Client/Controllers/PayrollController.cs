using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using HRMSmvc.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HRMSmvc.Areas.Client.Controllers
{
    [Area("Client")]

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
        public async Task<IActionResult> MyPayroll()
        {
            var userId = User.UserId();
            var response = await GetAsync<ApiResponse<UserPayrollDTO>>($"/api/Payroll/GetPayrollByEmpID?EmpId={userId}");
            if (response != null && response.success)
            {
                return View("PayrollIndex", response.Data);
            }
            return View("PayrollIndex", new List<PayrollDTO>());
        }
    }
}
