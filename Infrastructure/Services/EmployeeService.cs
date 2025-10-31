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
        private readonly IEmailService _emailService;

        public EmployeeService(AppDbContext context, IMapper mapper, UserManager<User> userManager , IEmailService emailservice)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _emailService = emailservice;

        }

        public async Task<ApiResponse<List<UserDTO>>> GetAllEmployeesAsync(Guid? deptId)
        {
            IList<User> usersInRole = await _userManager.GetUsersInRoleAsync("User");

            if (deptId == null)
            {
                var employees = usersInRole.Where(e => e.IsDeleted==false).ToList();
                List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(employees);

                return new ApiResponse<List<UserDTO>>
                {
                    success = true,
                    message = "Employees retrieved successfully",
                    Data = userDTOs
                };
            }
            else
            {
                var employees = usersInRole.Where(e => e.DepartmentId == deptId && e.IsDeleted==false).ToList();
                List<UserDTO> userDTOs = _mapper.Map<List<UserDTO>>(employees);
                return new ApiResponse<List<UserDTO>>
                {
                    success = true,
                    message = "Employees retrieved successfully",
                    Data = userDTOs
                };
            }
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
            var today = DateTime.Today;
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.UserId == id && a.Date == today);

            bool? isCheckedIn = attendance != null;
            bool? isCheckedOut = attendance?.CheckOutTime != null;

            var employeeDTO = _mapper.Map<UserDTO>(employee);
            employeeDTO.IsCheckedIn = isCheckedIn;
            employeeDTO.IsCheckedOut = isCheckedOut;

            // Return the response with the employee details
            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Employee found",
                Data = employeeDTO
            };
        }

        public async Task<ApiResponse<UserDTO>> CreateEmployeeAsync(UserDTO user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "Email is already registered",
                    Data = null
                };
            }

            // Generate a random password
            var generator = new Generator();
            string generatedPassword = generator.GenerateRandomPassword();

            var newUser = new User
            {
                UserName = user.UserName,
                Name = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DOB = user.DOB,
                JoiningDate = user.JoiningDate,
                Position = user.Position,
                Salary = user.Salary,
                Address = user.Address,
                IsActive = user.IsActive,
                IsDeleted = false,
                ProfilePictureFileName = user.ProfilePictureFileName,
            };

            var result = await _userManager.CreateAsync(newUser, generatedPassword);
            if (!result.Succeeded)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = string.Join(" ", result.Errors.Select(e => e.Description))
                };
            }
            await _userManager.AddToRoleAsync(newUser, "User");

            var userDTO = _mapper.Map<UserDTO>(newUser);
            userDTO.Password = generatedPassword;
            await _emailService.SendEmailAsync(
                userDTO.Email,
                "Email and Password",
                $"Dear User ,\n Your email :  " + userDTO.Email +"\n"+ "and Password : " + userDTO.Password
                );

            return new ApiResponse<UserDTO>
            {
                success = true,
                message = "Employee created successfully",
                Data = userDTO
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
            try
            {
                var existingEmployee = await _userManager.FindByIdAsync(employee.Id);
                var existingEmployeeByEmail = await _userManager.FindByEmailAsync(employee.Email); //data

                if (existingEmployee == null)
                {
                    return new ApiResponse<UserDTO>
                    {
                        success = false,
                        message = "User doesn't exist"
                    };
                }

                if (existingEmployeeByEmail != null)
                {
                    if (employee.Id != existingEmployeeByEmail.Id)
                    {
                        return new ApiResponse<UserDTO>
                        {
                            success = false,
                            message = "User with this Email already exists"
                        };
                    }

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
                    existingEmployee.IsActive = employee.IsActive.HasValue ? employee.IsActive.Value : existingEmployee.IsActive;
                    //existingEmployee.ProfilePictureFileName = employee.ProfilePictureFileName ?? existingEmployee.ProfilePictureFileName;
                    _context.Users.Update(existingEmployee);
                    await _userManager.UpdateNormalizedEmailAsync(existingEmployee);
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

            catch (Exception e)
            {
                return new ApiResponse<UserDTO>
                {
                    success = false,
                    message = "An error occurred while updating the employee."
                };
            }

        }

        public async Task<ApiResponse<string>> DeleteEmployeeAsync(string Id)
        {
            try
            {
                User? user = await _context.Users.FindAsync(Id);
                if (user == null)
                {
                    return new ApiResponse<string>
                    {
                        success = false,
                        message = "Employee not found",
                        Data = null
                    };
                }
                user.IsDeleted = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new ApiResponse<string>
                {
                    success = true,
                    message = "Employee deleted successfully",
                    Data = null
                } ;
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "An error occurred while deleting the employee.",
                    Data = null
                };
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

        public async Task<ApiResponse<UserDTO>> ChangeActiveStatus(string id)
        {
            var user = await _context.Users.FindAsync(id);
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
                user.IsActive = !user.IsActive;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new ApiResponse<UserDTO>
                {
                    success = true,
                    message = "Employee active status changed successfully",
                    Data = new UserDTO
                    {
                        Id = user.Id,
                        IsActive = user.IsActive
                    }
                };
            }
        }

    };
}

