using Application.Interface;
using Domain.DTO;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollController : Controller
    {
        private readonly ILogger<PayrollController> _logger;
        private readonly IPayrollService _payrollService;
        public PayrollController(ILogger<PayrollController> logger, IPayrollService payrollService)
        {
            _logger = logger;
            _payrollService = payrollService;
        }

        [HttpGet("GetPayrollByEmpID")]

        public async Task<IActionResult> GetPayrollByEmpId(string EmpId , int Year, int Month)
        {
            var payroll = await _payrollService.GetPayrollsByEmployeeIdAsync(EmpId , Year , Month);
            if (payroll == null)
            {
                return NotFound();
            }
            return Ok(payroll);
        }

        [HttpPost("CreatePayroll")]
        public async Task<IActionResult> CreatePayroll([FromBody] PayrollDTO payroll)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _payrollService.CreatePayrollAsync(payroll);
            return Ok(result);
        }

    }
}
