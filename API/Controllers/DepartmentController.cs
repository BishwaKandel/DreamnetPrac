using Application.Interface;
using Domain.DTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _deptService;

        public DepartmentController(IDepartmentService deptService)
        {
            _deptService = deptService;
        }

        //Create a Department
        [HttpPost("CreateDept")]
        public async Task<IActionResult> CreateDepartmentAsync(DepartmentDTO department)
        {
            var createdDepartment = await _deptService.CreateDepartmentAsync(department);
            return CreatedAtAction(
                    nameof(GetDepartmentById),
                    createdDepartment);
        }

        //Get All Departments
        [HttpGet("GetAllDept")]
        public async Task<IActionResult> GetAllDepartmentsAsync()
        {
            var departments = await _deptService.GetAllDepartmentsAsync();
            return Ok(departments);
        }

        //Get Department by ID
        [HttpGet("GetDeptById")]
        public async Task<IActionResult> GetDepartmentById(string id)
        {
            if (!Guid.TryParse(id, out Guid entityGuid))
            {
                return BadRequest("Format is incorrect");
            }
            var department = await _deptService.GetDepartmentByIdAsync(entityGuid);
            if (department == null)
            {
                return NotFound();
            }
            return Ok(department);
        }

        //Update Department
        [HttpPut("UpdateDept")]
        public async Task<IActionResult> UpdateDepartmentAsync(DepartmentDTO department)
        {
            var updatedDepartment = await _deptService.UpdateDepartmentAsync(department);
            if (updatedDepartment == null)
            {
                return NotFound();
            }
            return Ok(updatedDepartment);
        }

        //Delete Department
        [HttpDelete("DeleteDept")]
        public async Task<IActionResult> DeleteDepartmentAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid entityGuid))
            {
                return BadRequest("Format is incorrect");
            }
            var isDeleted = await _deptService.DeleteDepartmentAsync(entityGuid);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
