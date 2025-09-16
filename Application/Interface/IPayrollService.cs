using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO;

namespace Application.Interface
{
    public interface IPayrollService
    {
        Task<IEnumerable<PayrollDTO>> GetAllPayrollsAsync();
        Task<PayrollDTO?> GetPayrollByIdAsync(Guid id);
        Task<PayrollDTO> CreatePayrollAsync(PayrollDTO payroll);
        Task<PayrollDTO> UpdatePayrollAsync(Guid id, PayrollDTO payroll);
        Task<bool> DeletePayrollAsync(Guid id);
        Task<IEnumerable<PayrollDTO>> GetPayrollsByEmployeeIdAsync(Guid employeeId);
        Task<IEnumerable<PayrollDTO>> GetPayrollsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
