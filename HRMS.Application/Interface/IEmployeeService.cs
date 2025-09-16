using HRMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interface
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync();
        Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id);
        Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employee);
        Task<EmployeeDto> UpdateEmployeeAsync(Guid id, EmployeeDto employee);
        Task<bool> DeleteEmployeeAsync(Guid id);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<PayrollDto>> GetPayrollsByEmployeeIdAsync(Guid employeeId);
    }
}
