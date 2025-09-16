using HRMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interface
{
    public interface IAttendanceService
    {
        Task<IEnumerable<AttendanceDto>> GetAllAttendancesAsync();
        Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id);
        Task<AttendanceDto> CreateAttendanceAsync(AttendanceDto attendance);
        Task<AttendanceDto> UpdateAttendanceAsync(Guid id, AttendanceDto attendance);
        Task<bool> DeleteAttendanceAsync(Guid id);
        Task<IEnumerable<AttendanceDto>> GetAttendancesByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<AttendanceDto>> GetAttendancesByDateRangeAsync(DateTime startDate, DateTime endDate);

    }
}
