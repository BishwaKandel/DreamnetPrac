
using Domain.DTO;

namespace Application.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync();
        Task<DepartmentDTO?> GetDepartmentByIdAsync(Guid id);
        Task<DepartmentDTO> CreateDepartmentAsync(DepartmentDTO department);
        Task<DepartmentDTO> UpdateDepartmentAsync(DepartmentDTO department);
        Task<bool> DeleteDepartmentAsync(Guid id);
    }
}
