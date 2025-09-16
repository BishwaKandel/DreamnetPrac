using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface ILeaveService
    {
        Task<IEnumerable<LeaveRequestDTO>> GetAllLeavesAsync();
        Task<LeaveRequestDTO?> GetLeaveByIdAsync(Guid id);
        Task<LeaveRequestDTO> CreateLeaveAsync(LeaveRequestDTO leave);
        Task<LeaveRequestDTO> UpdateLeaveAsync(Guid id, LeaveRequestDTO leave);
        Task<bool> DeleteLeaveAsync(Guid id);
        Task<IEnumerable<LeaveRequestDTO>> GetLeavesByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<LeaveRequestDTO>> GetLeavesByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
