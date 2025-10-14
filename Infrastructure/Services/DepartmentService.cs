using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.DTO.DepartmentDTO;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            Department departments = new Department
            {
                Name = department.Name,
                Description = department.Description,
                Location = department.Location,
                IsActive = department.IsActive
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
        public async Task<bool> DeleteDepartmentAsync(Guid id)
        {
            try
            {
                Department? dept = await _context.Departments.FindAsync(id);
                if (dept == null)
                {
                    return false;
                }
                _context.Departments.Remove(dept);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //GetALL
        public async Task<ApiResponse<List<DeptInfoDTO>>> GetAllDepartmentsAsync()
        {
            List<Department> dept = await _context.Departments.ToListAsync();
            List<DeptInfoDTO> result = _mapper.Map<List<DeptInfoDTO>>(dept);

            return new ApiResponse<List<DeptInfoDTO>>
            {
                success = true,
                message = "Employees retrieved successfully",
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
            Department? existingDept = await _context.Departments.FindAsync(department.Id);

            if (existingDept == null)
            {
                return new ApiResponse<DeptInfoDTO>
                {
                    success = false,
                    message = "Department not found"
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
                message = "Employee updated Successfully",
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
                .Where(e => e.DepartmentId != departmentId || e.DepartmentId == null)
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
                .Where(e => employeeIds.Contains(e.Id))
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
