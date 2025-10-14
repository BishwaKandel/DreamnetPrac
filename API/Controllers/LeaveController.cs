using Application.Interface;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : Controller
    {
        private readonly ILogger<LeaveController> _logger;
        private readonly ILeaveService _leaveService;
        public LeaveController(ILogger<LeaveController> logger , ILeaveService leaveService)
        {
            _logger = logger;
            _leaveService = leaveService;
        }

        [HttpPost("CreateLeave")]
        public async Task<IActionResult> CreateLeaveAsync([FromBody] LeaveRequestDTO leave )
        {

            var CreatedLeave = await _leaveService.CreateLeaveAsync(leave, leave.RequestedById);
            return CreatedAtAction(
                    nameof(GetLeaveById),
                    CreatedLeave);
        }

        [HttpGet("GetLeave")]
        public async Task<IActionResult> GetAllLeaves()
        {
            var leaves = await _leaveService.GetAllLeavesAsync();
            return Ok(leaves);
        }

        [HttpGet("GetLeaveById")]
        public async Task<IActionResult> GetLeaveById(Guid id)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return Ok(leave);
        }

        [HttpGet("GetLeaveByEmpID")]

        public async Task<IActionResult> GetLeaveByEmpId(string EmpId)
        {
            var leave = await _leaveService.GetLeavesByEmployeeIdAsync(EmpId);
            if (leave == null)
            {
                return NotFound();
            }
            return Ok(leave);
        }

        [HttpPost("ApproveLeave")]
        public async Task<IActionResult> ApproveLeave(Guid leaveRequestId)
        {
            var approvedLeave = await _leaveService.ApproveLeave(leaveRequestId);
            if (approvedLeave == null)
            {
                return NotFound();
            }
            return Ok(approvedLeave);
        }

        [HttpPost("RejectLeave")]
        public async Task<IActionResult> RejectLeave(Guid leaveRequestId)
        {
            var rejectedLeave = await _leaveService.RejectLeave(leaveRequestId);
            if (rejectedLeave == null)
            {
                return NotFound();
            }
            return Ok(rejectedLeave);
        }

    }
}
