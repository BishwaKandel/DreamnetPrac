using Domain.DTO;
using Domain.Models;
using HRMSmvc.Controllers;
using HRMSmvc.Extensions;
using HRMSmvc.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HRMSmvc.Areas.Client.Controllers
{
    [Area("Client")]
    public class LeaveController : BaseController
    {
        private readonly HttpClient client;
        private readonly IConfiguration _configuration;
        public LeaveController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ILogger<BaseController> logger)
       : base(configuration, httpContextAccessor, logger)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> LeaveApply(LeaveRequestViewModel leaveRequestVm)
        {
            var dto = new LeaveRequestDTO
            {
                Id = leaveRequestVm.Id,
                RequestedById = User.UserId(),
                AppliedOn = leaveRequestVm.AppliedOn,
                StartDate = leaveRequestVm.StartDate,
                EndDate = leaveRequestVm.EndDate,
                Reason = leaveRequestVm.Reason,
                Status = leaveRequestVm.Status,
                LeaveType = leaveRequestVm.LeaveType,
                Description = leaveRequestVm.Description
            };

            var response = await PostAsync<ApiResponse<LeaveRequestDTO>>("/api/Leave/CreateLeave", dto, null);
            return Json(response);
        }

        [HttpGet]
        public IActionResult LeaveApply()
        {
            var viewModel = new LeaveRequestViewModel
            {
                // Populate LeaveTypes with enum values as SelectListItems
                LeaveTypes = Enum.GetValues(typeof(LeaveType))
                                .Cast<LeaveType>()
                                .Select(e => new SelectListItem
                                {
                                    Value = ((int)e).ToString(),
                                    Text = e.ToString()
                                })
            };


            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> MyLeaves()
        {
            var userId = User.UserId();
            var response = await GetAsync<ApiResponse<List<LeaveRequestDTO>>>(
                $"/api/Leave/GetLeaveByEmpID?EmpId={userId}");
            if (response != null && response.success)
            {
                return View("LeaveIndex",response.Data);
            }
            return View("LeaveIndex",new List<LeaveRequestDTO>());
        }
    }
}
