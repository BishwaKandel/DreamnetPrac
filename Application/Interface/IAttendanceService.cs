using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAttendanceService
    {
        //Task<ApiResponse<List<AttendanceDTO>>> GetAllAttendancesAsync();
        //Task<ApiResponse<AttendanceDTO?>> GetAttendanceByIdAsync(Guid id);
        Task<ApiResponse<AttendanceDTO>> CheckInAsync(string userId);
        Task<ApiResponse<AttendanceDTO>> CheckOutAsync(string userId);

        //Task<ApiResponse<AttendanceDTO>> UpdateAttendanceAsync(Guid id, AttendanceDTO attendance);
        //Task<bool> DeleteAttendanceAsync(Guid id);
        Task<ApiResponse<UserAttendanceDTO>> GetAttendancesByEmpIdAsync(string userId);
    }
}
