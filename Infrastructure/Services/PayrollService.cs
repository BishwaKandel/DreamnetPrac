using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PayrollService : IPayrollService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public PayrollService(AppDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ApiResponse<PayrollDTO>> CreatePayrollAsync(PayrollDTO payroll)
        {
            var employee = await _userManager.FindByIdAsync(payroll.UserId);
            var query = _context.Payrolls.Where(p => p.UserId == payroll.UserId && p.Year == payroll.Year && p.Month == payroll.Month);
            var existingPayroll = query.FirstOrDefault();
            if (employee == null)
            {
                return new ApiResponse<PayrollDTO>
                {
                    Data = null,
                    message = "Employee not found",
                    success = false
                };
            }
            if (existingPayroll != null)
            {
                return new ApiResponse<PayrollDTO>
                {
                    Data = null,
                    message = "Payroll for this employee for the specified month and year already exists",
                    success = false
                };
            }

            Domain.Models.Payroll newpayroll = new Domain.Models.Payroll
            {
                Id = Guid.NewGuid(),
                UserId = payroll.UserId,
                Year = payroll.Year,
                Month = payroll.Month,
                BasicSalary = payroll.BasicSalary,
                Allowances = payroll.Allowances,
                Deductions = payroll.Deductions,
                NetSalary = payroll.BasicSalary + payroll.Allowances - payroll.Deductions
            };

            _context.Payrolls.Add(newpayroll);
            await _context.SaveChangesAsync();
            var payrollDTO = _mapper.Map<PayrollDTO>(newpayroll);
            return new ApiResponse<PayrollDTO>
            {
                Data = payrollDTO,
                message = "Payroll created successfully",
                success = true
            };
        }

        public Task<ApiResponse<bool>> DeletePayrollAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<PayrollDetailsDTO>>> GetAllPayrollAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApiResponse<PayrollDTO?>> GetPayrollByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<UserPayrollDTO>> GetPayrollsByEmployeeIdAsync(string employeeId, int Year, int Month)
        {
            var user = await _userManager.FindByIdAsync(employeeId);
            if (user == null)
            {
                return new ApiResponse<UserPayrollDTO>
                {
                    Data = null,
                    message = "Employee not found",
                    success = false
                };
            }

            var query = _context.Payrolls.Where(p => p.UserId == employeeId);

            if (Year != 0 && Month != 0)
            {
                query = query.Where(p => p.Year == Year && p.Month == Month);
            }

            var payrolls = query.ToList();

            var userPayrollDTO = new UserPayrollDTO
            {
                userId = user.Id,
                Name = user.Name, 
                payrolls = _mapper.Map<List<PayrollDetailsDTO>>(payrolls) 
            };

            return new ApiResponse<UserPayrollDTO>
            {
                Data = userPayrollDTO,
                message = "Payrolls retrieved successfully",
                success = true
            };

        }

        public Task<ApiResponse<PayrollDTO>> UpdatePayrollAsync(Guid id, PayrollDTO payroll)
        {
            throw new NotImplementedException();
        }
    }
}
