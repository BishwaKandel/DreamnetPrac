using HRMS.Application.DTO;
using HRMS.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        public IEmployeeService _employeeService;

        public Task<EmployeeDto> CreateEmployeeAsync(EmployeeDto employee)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteEmployeeAsync(Guid id)
        {
            throw new NotImplementedException();
        }


        //public EmployeeService(IEmployeeService employeeService)
        //{
        //    _employeeService = employeeService;
        //}
        public Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync()
        {
            return _employeeService.GetAllEmployeesAsync();

        }

        public Task<IEnumerable<DepartmentDto>> GetDepartmentsByEmployeeIdAsync(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto?> GetEmployeeByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Invalid employee ID.", nameof(id));
            }
            return _employeeService.GetEmployeeByIdAsync(id);
        }

        public Task<IEnumerable<LeaveRequestDto>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PayrollDto>> GetPayrollsByEmployeeIdAsync(Guid employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeDto> UpdateEmployeeAsync(Guid id, EmployeeDto employee)
        {
            throw new NotImplementedException();
        }
    }
}
