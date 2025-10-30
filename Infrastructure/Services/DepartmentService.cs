using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.DTO.DepartmentDTO;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public DepartmentService(UserManager<User> userManager, AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        // Create
        public async Task<ApiResponse<DeptInfoDTO>> CreateDepartmentAsync(DeptInfoDTO department)
        {
            var existingDept = await _context.Departments.FindAsync(department.Id);
            if (existingDept != null)
            {
                if (existingDept.Name == department.Name)
                {
                    return new ApiResponse<DeptInfoDTO>
                    {
                        success = false,
                        message = "Department with this name already Exists",
                        Data = null
                    };
                }
            }
            Department departments = new Department
            {
                Name = department.Name,
                Description = department.Description,
                Location = department.Location,
                IsActive = true
            };
            _context.Departments.Add(departments);
            await _context.SaveChangesAsync();

            var departmentDTO = _mapper.Map<DeptInfoDTO>(departments);
            return new ApiResponse<DeptInfoDTO>
            {
                success = true,
                message = "Department created Successfully",
                Data = new DeptInfoDTO
                {
                    Id = departmentDTO.Id,
                    Name = departmentDTO.Name,
                    Description = departmentDTO.Description,
                    Location = departmentDTO.Location,
                    IsActive = departmentDTO.IsActive
                }
            };
        }

        //Delete
        public async Task<ApiResponse<string>> DeleteDepartmentAsync(Guid id)
        {
            try
            {
                Department? dept = await _context.Departments.FindAsync(id);
                if (dept == null)
                {
                    return new ApiResponse<string>
                    {
                        success = false,
                        message = "Department Not Found",
                        Data = null

                    };
                }
                dept.IsDeleted = true;
                _context.Departments.Update(dept);
                await _context.SaveChangesAsync();
                var users = _context.Users.Where(u => u.DepartmentId == id);
                foreach(var user in users)
                {
                    user.DepartmentId = null;
                }
                _context.Users.UpdateRange(users);
                await _context.SaveChangesAsync();

                return new ApiResponse<string>
                {
                    success = true,
                    message = "Department deleted Succesfully",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "An error occurred while deleting the department.",
                    Data = null
                };
            }
        }

        //GetALL
        public async Task<ApiResponse<List<DeptInfoDTO>>> GetAllDepartmentsAsync()
        {
            var filteredDept = await _context.Departments
                                .Where(d => !d.IsDeleted)
                                .ToListAsync();

            List<DeptInfoDTO> result = _mapper.Map<List<DeptInfoDTO>>(filteredDept);

            return new ApiResponse<List<DeptInfoDTO>>
            {
                success = true,
                message = "Department retrieved successfully",
                Data = result
            };
        }

        //Get Dept by ID
        public async Task<ApiResponse<DeptInfoDTO>> GetDepartmentByIdAsync(Guid id)
        {
            Department? dept = await _context.Departments.FindAsync(id);
            if (dept == null)
            {
                return new ApiResponse<DeptInfoDTO>
                {
                    success = false,
                    message = "Department not found"
                };
            }
            DeptInfoDTO deptDTO = _mapper.Map<DeptInfoDTO>(dept);
            return new ApiResponse<DeptInfoDTO>
            {
                success = true,
                message = "Department found",
                Data = new DeptInfoDTO
                {
                    Id = deptDTO.Id,
                    Name = deptDTO.Name,
                    Description = deptDTO.Description,
                    Location = deptDTO.Location,
                    IsActive = deptDTO.IsActive
                }
            };
        }

        //Update 
        public async Task<ApiResponse<DeptInfoDTO>> UpdateDepartmentAsync(DeptInfoDTO department)
        {
            var existingDept = await _context.Departments.FindAsync(department.Id);
            var existingDeptByName = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == department.Name && d.Id != department.Id);
            if (existingDept == null)
            {
                return new ApiResponse<DeptInfoDTO>
                {
                    success = false,
                    message = "Department not found"
                };
            }

            if(existingDeptByName!=null)
            {
                return new ApiResponse<DeptInfoDTO>
                {
                    success = false,
                    message = "Department with this Name already exists",
                    Data = null
                };
            }

            existingDept.Name = department.Name;
            existingDept.Description = department.Description;
            existingDept.Location = department.Location;
            existingDept.IsActive = department.IsActive;
            _context.Departments.Update(existingDept);
            await _context.SaveChangesAsync();
            DeptInfoDTO deptDTO = _mapper.Map<DeptInfoDTO>(existingDept);
            return new ApiResponse<DeptInfoDTO>
            {
                success = true,
                message = "Department updated Successfully",
                Data = new DeptInfoDTO
                {
                    Id = deptDTO.Id,
                    Name = deptDTO.Name,
                    Description = deptDTO.Description,
                    Location = deptDTO.Location,
                    IsActive = deptDTO.IsActive
                }
            };
        }

        //Get all Employees in a Department

        public async Task<ApiResponse<List<UserDTO>>> GetEmployeesExceptDeptIdAsync(Guid departmentId)
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync("User");
            var employees = usersInRole
                .Where(e => (e.DepartmentId != departmentId || e.DepartmentId == null) && e.IsDeleted == false)
                .ToList();
            var employeeDTOs = _mapper.Map<List<UserDTO>>(employees);
            return new ApiResponse<List<UserDTO>>
            {
                success = true,
                message = "Employees retrieved successfully",
                Data = employeeDTOs
            };
        }

        //Add Employee to Department


        public async Task<ApiResponse<string>> AddEmployeesToDepartmentAsync(Guid departmentId, List<string> employeeIds)
        {
            var department = await _context.Departments.FindAsync(departmentId);
            if (department == null)
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "Department not found"
                };
            }
            var employees = await _context.Users
                .Where(e => employeeIds.Contains(e.Id) && e.IsDeleted == false)
                .ToListAsync();

            if (!employees.Any())
            {
                return new ApiResponse<string>
                {
                    success = false,
                    message = "No employees found for the provided IDs"
                };
            }

            foreach (var employee in employees)
            {
                employee.DepartmentId = departmentId;
            }

            _context.Users.UpdateRange(employees);
            await _context.SaveChangesAsync();

            return new ApiResponse<string>
            {
                success = true,
                message = "Employees added to Department successfully",
                Data = $"Employees {string.Join(", ", employees.Select(e => e.Id))} added to Department {departmentId}"
            };
        }


    }
}
