using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using HRMSmvc.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMSmvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class LeaveController : BaseController
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public LeaveController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllLeaves()
        {
            var leaves = await GetAsync<ApiResponse<List<LeaveDetailsDTO>>>("/api/Leave/GetLeave");
            if (leaves == null)
            {
                ModelState.AddModelError(string.Empty, "Error fetching leave data.");
                return View(new List<LeaveDetailsDTO>());
            }
            // Return the list of leaves to the view
            return View("Index",leaves.Data);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveLeave(Guid leaveRequestId)
        {
            var response = await PostAsync<ApiResponse<LeaveRequestDTO>>($"/api/Leave/ApproveLeave?leaveRequestId={leaveRequestId}", null , null);
            if (response == null || !response.success)
            {
                ModelState.AddModelError(string.Empty, "Error approving leave request.");
            }
            return RedirectToAction("ViewAllLeaves");
        }

        [HttpPost]
        public async Task<IActionResult> RejectLeave(Guid leaveRequestId)
        {
            var response = await PostAsync<ApiResponse<LeaveRequestDTO>>($"/api/Leave/RejectLeave?leaveRequestId={leaveRequestId}", null , null);
            if (response == null || !response.success)
            {
                ModelState.AddModelError(string.Empty, "Error rejecting leave request.");
            }
            return RedirectToAction("ViewAllLeaves");
        }
    }
}
