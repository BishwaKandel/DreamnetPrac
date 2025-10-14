using Application.Interface;
using Domain.DTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : Controller
    {
        private readonly ILogger<AttendanceController> _logger;
        private readonly IAttendanceService _attendanceService;
        public AttendanceController(ILogger<AttendanceController> logger, IAttendanceService attendanceService)
        {
            _logger = logger;
            _attendanceService = attendanceService;
        }

        [HttpPost("CheckIn")]
        public async Task<IActionResult> CheckIn(string userId)
        {
            var result = await _attendanceService.CheckInAsync(userId);
            return Ok(result);
        }

        [HttpPost("CheckOut")]
        public async Task<IActionResult> CheckOut(string userId)
        {
            var result = await _attendanceService.CheckOutAsync(userId);
            return Ok(result);
        }

        [HttpGet("GetAttendanceByEmpID")]
        public async Task<IActionResult> GetAttendanceByEmpID(string userId)
        {
            var result = await _attendanceService.GetAttendancesByEmpIdAsync(userId);
            return Ok(result);
        }
    }
}
