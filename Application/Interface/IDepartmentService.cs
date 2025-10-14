
using Domain.DTO;
using Domain.DTO.DepartmentDTO;
using Domain.Models;

namespace Application.Interface
{
    public interface IDepartmentService
    {
        Task<ApiResponse<List<DeptInfoDTO>>> GetAllDepartmentsAsync();
        Task<ApiResponse<DeptInfoDTO>> GetDepartmentByIdAsync(Guid id);
        Task<ApiResponse<DeptInfoDTO>> CreateDepartmentAsync(DeptInfoDTO department);
        Task<ApiResponse<DeptInfoDTO>> UpdateDepartmentAsync(DeptInfoDTO department);
        Task<bool> DeleteDepartmentAsync(Guid id);
        Task<ApiResponse<List<UserDTO>>> GetEmployeesExceptDeptIdAsync(Guid departmentId);
        Task<ApiResponse<string>> AddEmployeesToDepartmentAsync(Guid departmentId, List<string> employeeIds);
    }
}
