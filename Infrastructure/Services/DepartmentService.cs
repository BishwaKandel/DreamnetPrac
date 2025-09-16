using Application.Interface;
using AutoMapper;
using Domain.DTO;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DepartmentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // Create
        public async Task<DepartmentDTO> CreateDepartmentAsync(DepartmentDTO department)
        {
            Department departments = new Department
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Location = department.Location,
                IsActive = department.IsActive
            };
            _context.Departments.Add(departments);
            await _context.SaveChangesAsync();

            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(departments);
            return departmentDTO;
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
        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentsAsync()
        {
            List<Department> dept = await _context.Departments.ToListAsync();
            List<DepartmentDTO> result = _mapper.Map<List<DepartmentDTO>>(dept);

            return result;
        }

        //Get Dept by ID
        public async Task<DepartmentDTO?> GetDepartmentByIdAsync(Guid id)
        {
            Department? dept = await _context.Departments.FindAsync(id);
            if (dept == null)
            {
                return null;
            }
            DepartmentDTO deptDTO = _mapper.Map<DepartmentDTO>(dept);
            return deptDTO;
        }


        //Update 
        public async Task<DepartmentDTO> UpdateDepartmentAsync(DepartmentDTO department)
        {
            Department? existingDept = await _context.Departments.FindAsync(department.Id);

            if (existingDept == null)
            {
                return new();
            }

            existingDept.Name = department.Name;
            existingDept.Description = department.Description;
            existingDept.Location = department.Location;
            existingDept.IsActive = department.IsActive;
            _context.Departments.Update(existingDept);
            await _context.SaveChangesAsync();
            DepartmentDTO deptDTO = _mapper.Map<DepartmentDTO>(existingDept);
            return deptDTO;
        }

        //Get all Employees in a Department
    }
}
