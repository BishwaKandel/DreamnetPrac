using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ILeaveService
    {
        Task<ApiResponse<List<LeaveDetailsDTO>>> GetAllLeavesAsync();
        Task<ApiResponse<LeaveRequestDTO?>> GetLeaveByIdAsync(Guid id);
        Task<ApiResponse<LeaveRequestDTO>> CreateLeaveAsync(LeaveRequestDTO leave, string userId);
        Task<ApiResponse<LeaveRequestDTO>> UpdateLeaveAsync(Guid id, LeaveRequestDTO leave);
        Task<ApiResponse<bool>> DeleteLeaveAsync(Guid id);

        Task<ApiResponse<LeaveRequestDTO>> ApproveLeave(Guid leaveRequestId);
        Task<ApiResponse<LeaveRequestDTO>> RejectLeave(Guid leaveRequestId);
        Task<ApiResponse<List<LeaveRequestDTO>>> GetLeavesByEmployeeIdAsync(string employeeId);
        Task<ApiResponse<IEnumerable<LeaveRequestDTO>>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
