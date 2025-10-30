using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IEmployeeService
    {
        Task<ApiResponse<List<UserDTO>>> GetAllEmployeesAsync(Guid? deptId);
        Task<ApiResponse<UserDTO>> GetEmployeeByIdAsync(String id);
        Task<ApiResponse<UserDTO>> CreateEmployeeAsync(UserDTO employee);
        Task<ApiResponse<UserDTO>> UpdateEmployeeAsync(UserUpdateDTO employee);
        Task<ApiResponse<string>> DeleteEmployeeAsync(string Id);
        Task<ApiResponse<UserDTO>> ChangePasswordAsync(ChangePasswordDTO employee);
        Task<ApiResponse<UserDTO>> UpdateProfilePicAsync(ChangePpDTO employee);
        Task<ApiResponse<UserDTO>> DeleteProfilePicAsync(ChangePpDTO employee);

        Task<ApiResponse<UserDTO>> ChangeActiveStatus(string id);



        //Task<IEnumerable<DepartmentDTO>> GetDepartmentsByEmployeeIdAsync(Guid employeeId);
        //Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId);
        //Task<IEnumerable<PayrollDTO>> GetPayrollsByEmployeeIdAsync(Guid employeeId);
    }
}
