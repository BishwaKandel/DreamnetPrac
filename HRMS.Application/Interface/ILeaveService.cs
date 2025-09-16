using HRMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interface
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequestDto>> GetAllLeavesAsync();
        Task<LeaveRequestDto?> GetLeaveByIdAsync(Guid id);
        Task<LeaveRequestDto> CreateLeaveAsync(LeaveRequestDto leave);
        Task<LeaveRequestDto> UpdateLeaveAsync(Guid id, LeaveRequestDto leave);
        Task<bool> DeleteLeaveAsync(Guid id);
        Task<IEnumerable<LeaveRequestDto>> GetLeavesByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<LeaveRequestDto>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
