using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public EmployeeService(AppDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;

        }
        public async Task<ApiResponse<List<UserDTO>>> GetAllEmployeesAsync()
        {
            List<User> user = await _context.Users.ToListAsync();
            List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(user);

            return new ApiResponse<List<UserDTO>>
            {
                success = true,
                message = "Employees retrieved successfully",
                Data = userDTOs
            };
        }

        public async Task<ApiResponse<UserDTO>> GetEmployeeByIdAsync(String id)
        {
            User? employee = await _context.Users.FindAsync(id);
            if (employee == null)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "Employee not found"
                };
            }

            UserDTO employeeDTO = _mapper.Map<UserDTO>(employee);
            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Employee found",
                Data = new UserDTO
                {
                    Id = employeeDTO.Id,
                    Name = employeeDTO.Name,
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    Email = employeeDTO.Email,
                    PhoneNumber = employeeDTO.PhoneNumber,
                    DOB = employeeDTO.DOB,
                    JoiningDate = employeeDTO.JoiningDate,
                    Position = employeeDTO.Position,
                    Salary = employeeDTO.Salary,
                    Address = employeeDTO.Address,
                    IsActive = employeeDTO.IsActive,
                    ProfilePictureFileName = employeeDTO.ProfilePictureFileName
                }
            }; ;
        }

        public async Task<ApiResponse<UserDTO>> CreateEmployeeAsync(UserDTO user)
        {
            User userDTO = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DOB = user.DOB,
                JoiningDate = user.JoiningDate,
                Position = user.Position,
                Salary = user.Salary,
                Address = user.Address,
                isActive = user.IsActive,
                ProfilePictureFileName = user.ProfilePictureFileName,
            };
            _context.Users.Add(userDTO);

            await _context.SaveChangesAsync();

            var UserDTO = _mapper.Map<UserDTO>(userDTO);
            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Employee created Successfully",
                Data = new UserDTO
                {
                    Id = UserDTO.Id,
                    FirstName = UserDTO.FirstName,
                    LastName = UserDTO.LastName,
                    Email = UserDTO.Email,
                    PhoneNumber = UserDTO.PhoneNumber,
                    DOB = UserDTO.DOB,
                    JoiningDate = UserDTO.JoiningDate,
                    Position = UserDTO.Position,
                    Salary = UserDTO.Salary,
                    Address = UserDTO.Address,
                    IsActive = UserDTO.IsActive,
                    ProfilePictureFileName = UserDTO.ProfilePictureFileName
                }
            };
        }


        public async Task<ApiResponse<UserDTO>> UpdateProfilePicAsync(ChangePpDTO employee)
        {
            User? existingEmployee = await _context.Users.FindAsync(employee.Id);
            if (existingEmployee == null)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "Employee not found"
                };
            }
            existingEmployee.ProfilePictureFileName = employee.ProfilePictureFileName ?? existingEmployee.ProfilePictureFileName;
            _context.Users.Update(existingEmployee);
            await _context.SaveChangesAsync();
            UserDTO updatedEmployeeDTO = _mapper.Map<UserDTO>(existingEmployee);
            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Profile Picture updated Successfully",
                Data = new UserDTO
                {
                    Id = updatedEmployeeDTO.Id,
                    ProfilePictureFileName = updatedEmployeeDTO.ProfilePictureFileName,
                }
            };
        }
        public async Task<ApiResponse<UserDTO>> DeleteProfilePicAsync(ChangePpDTO employee)
        {
            //Console.WriteLine($"Looking for Id: '{employee.Id}'");
            //var existingEmployee = await _context.Users.FindAsync(employee.Id);
            //Console.WriteLine(existingEmployee == null ? "Not found in DB" : $"Found: {existingEmployee.Id}");

            //User? existingEmployee = await _context.Users.FindAsync(employee.Id);
            var existingEmployee = await _context.Users
            .SingleOrDefaultAsync(u => u.Id == employee.Id);

            if (existingEmployee == null)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "Employee not found"
                };
            }
            existingEmployee.ProfilePictureFileName = null;
            _context.Users.Update(existingEmployee);
            await _context.SaveChangesAsync();
            UserDTO updatedEmployeeDTO = _mapper.Map<UserDTO>(existingEmployee);
            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Profile Picture Deleted Successfully",
                Data = new UserDTO
                {
                    Id = updatedEmployeeDTO.Id
                }
            };
        }

        public async Task<ApiResponse<UserDTO>> UpdateEmployeeAsync(UserUpdateDTO employee)
        {
            User? existingEmployee = await _context.Users.FindAsync(employee.Id);
            if (existingEmployee == null)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "Employee not found"
                };
            }
            existingEmployee.Name = employee.Name ?? existingEmployee.Name;
            existingEmployee.FirstName = employee.FirstName ?? existingEmployee.FirstName;
            existingEmployee.LastName = employee.LastName ?? existingEmployee.LastName;
            existingEmployee.Email = employee.Email ?? existingEmployee.Email;
            existingEmployee.PhoneNumber = employee.PhoneNumber ?? existingEmployee.PhoneNumber;
            existingEmployee.DOB = employee.DOB.HasValue ? employee.DOB.Value : existingEmployee.DOB;

            existingEmployee.JoiningDate = employee.JoiningDate.HasValue ? employee.JoiningDate.Value : existingEmployee.JoiningDate;
            existingEmployee.Position = employee.Position ?? existingEmployee.Position;
            existingEmployee.Salary = employee.Salary.HasValue ? employee.Salary.Value : existingEmployee.Salary;
            existingEmployee.Address = employee.Address ?? existingEmployee.Address;
            existingEmployee.isActive = employee.IsActive.HasValue ? employee.IsActive.Value : existingEmployee.isActive;
            //existingEmployee.ProfilePictureFileName = employee.ProfilePictureFileName ?? existingEmployee.ProfilePictureFileName;
            _context.Users.Update(existingEmployee);
            await _context.SaveChangesAsync();
            UserDTO updatedEmployeeDTO = _mapper.Map<UserDTO>(existingEmployee);
            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Employee updated Successfully",
                Data = new UserDTO
                {
                    Id = updatedEmployeeDTO.Id,
                    Name = updatedEmployeeDTO.Name,
                    FirstName = updatedEmployeeDTO.FirstName,
                    LastName = updatedEmployeeDTO.LastName,
                    Email = updatedEmployeeDTO.Email,
                    PhoneNumber = updatedEmployeeDTO.PhoneNumber,
                    DOB = updatedEmployeeDTO.DOB,
                    JoiningDate = updatedEmployeeDTO.JoiningDate,
                    Position = updatedEmployeeDTO.Position,
                    Salary = updatedEmployeeDTO.Salary,
                    Address = updatedEmployeeDTO.Address,
                    ProfilePictureFileName = updatedEmployeeDTO.ProfilePictureFileName,
                    IsActive = updatedEmployeeDTO.IsActive
                }
            };
        }

        public async Task<bool> DeleteEmployeeAsync(Guid Id)
        {
            try
            {
                User? employees = await _context.Users.FindAsync(Id);
                if (employees == null)
                {
                    return false;
                }
                _context.Users.Remove(employees);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ApiResponse<UserDTO>> ChangePasswordAsync(ChangePasswordDTO employee)
        {
            var user = await _context.Users.FindAsync(employee.Id);
            if (user == null)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "Employee not found"

                };
            }
            else
            {
                if (employee.NewPassword != employee.ConfirmPassword)
                {
                    return new ApiResponse<UserDTO>
                    {
                        success = false,
                        message = "New password and confirm password do not match."
                    };
                }

                var result = await _userManager.ChangePasswordAsync(user, employee.CurrentPassword, employee.NewPassword);

                if (!result.Succeeded)
                {
                    // Collect errors
                    var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                    return new ApiResponse<UserDTO>
                    {
                        success = false,
                        message = $"Password change failed: {errors}"
                    };
                }
                return new ApiResponse<UserDTO>
                {
                    success = true,
                    message = "Password changed successfully",
                };
            }

            //Join tables left

            //public async Task<IEnumerable<DepartmentDTO>> GetDepartmentsByEmployeeIdAsync(Guid employeeId)
            //{
            //    List<Department> departments = await _context.Departments
            //        .Where(d => d.EId == employeeId)
            //        .ToListAsync();
            //    List<DepartmentDTO> departmentDTO = _mapper.Map<List<DepartmentDTO>>(departments);
            //    return departmentDTO;

            //}

            //public async Task<IEnumerable<LeaveRequestDTO>> GetLeaveRequestsByEmployeeIdAsync(Guid employeeId)
            //{
            //    List<LeaveRequest> LeaveRequests = await _context.LeaveRequests
            //        .Where(lr => lr.EId == employeeId)
            //        .ToListAsync();
            //    List<LeaveRequestDTO> leaveRequestDTOs = _mapper.Map<List<LeaveRequestDTO>>(LeaveRequests);

            //    return leaveRequestDTOs;

            //}

            //public async Task<IEnumerable<PayrollDTO>> GetPayrollsByEmployeeIdAsync(Guid employeeId)
            //{
            //    List<Payroll> payrolls = await _context.Payrolls
            //        .Where(p => p.EId == employeeId)
            //        .ToListAsync();
            //    List<PayrollDTO> payrollDTOs = _mapper.Map<List<PayrollDTO>>(payrolls);
            //    return payrollDTOs;
            //}


        }

    }

}

