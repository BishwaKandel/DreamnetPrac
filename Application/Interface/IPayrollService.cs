using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPayrollService
    {
        Task<ApiResponse<List<PayrollDetailsDTO>>> GetAllPayrollAsync();
        Task<ApiResponse<PayrollDTO?>> GetPayrollByIdAsync(Guid id);
        Task<ApiResponse<PayrollDTO>> CreatePayrollAsync(PayrollDTO payroll);
        Task<ApiResponse<PayrollDTO>> UpdatePayrollAsync(Guid id, PayrollDTO payroll);
        Task<ApiResponse<bool>> DeletePayrollAsync(Guid id);
        Task<ApiResponse<UserPayrollDTO>> GetPayrollsByEmployeeIdAsync(string employeeId, int Year, int Month);
    }
}
