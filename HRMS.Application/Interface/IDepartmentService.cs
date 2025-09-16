using HRMS.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interface
{
    public interface IDepartmentService
    {
        Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(Guid id);
        Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto department);
        Task<DepartmentDto> UpdateDepartmentAsync(Guid id, DepartmentDto department);
        Task<bool> DeleteDepartmentAsync(Guid id);
        Task<IEnumerable<DepartmentDto>> GetDepartmentsByCompanyIdAsync(Guid companyId);
    }
}
