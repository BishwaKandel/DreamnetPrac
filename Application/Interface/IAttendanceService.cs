using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDTO>> GetAllAttendancesAsync();
        Task<AttendanceDTO?> GetAttendanceByIdAsync(Guid id);
        Task<AttendanceDTO> CreateAttendanceAsync(AttendanceDTO attendance);
        Task<AttendanceDTO> UpdateAttendanceAsync(Guid id, AttendanceDTO attendance);
        Task<bool> DeleteAttendanceAsync(Guid id);
        Task<IEnumerable<AttendanceDTO>> GetAttendancesByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<AttendanceDTO>> GetAttendancesByDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}
