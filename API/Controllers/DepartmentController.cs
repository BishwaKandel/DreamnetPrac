using Application.Interface;
using Domain.DTO;
using Domain.DTO.DepartmentDTO;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _deptService;
        private readonly IEmployeeService _empService;

        public DepartmentController(IDepartmentService deptService , IEmployeeService empService)
        {
            _deptService = deptService;
            _empService = empService;
        }

        //Create a Department
        [HttpPost("CreateDept")]
        public async Task<IActionResult> CreateDepartmentAsync(DeptInfoDTO department)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
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
        [HttpPost("UpdateDept")]
        public async Task<IActionResult> UpdateDepartmentAsync([FromBody] DeptInfoDTO department)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
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

        //Get Employees EXCEPT it's department 
        [HttpGet("GetEmpExceptDeptId")]
        public async Task<IActionResult> GetEmpExceptDeptId(Guid id)
        {
            var employees = await _deptService.GetEmployeesExceptDeptIdAsync(id);
            return Ok(employees);
        }

        //Add Employees to a Department 
        [HttpPost("AddEmpToDept")]
        public async Task<IActionResult> AddEmployeesToDepartmentAsync([FromBody] AddEmpReq request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _deptService.AddEmployeesToDepartmentAsync(request.DepartmentId , request.EmployeeIds);
            if (result == null || !result.success)  
            {
                return BadRequest(result?.message ?? "Failed to add employees to department");
            }
            return Ok(result);
        }

        //Get All Employees in a Department 
        [HttpGet("GetAllEmpInDept")]

        public async Task<IActionResult> GetAllEmployeesInDepartmentAsync(Guid deptId)
        {
            var employees = await _empService.GetAllEmployeesAsync(deptId);
            return Ok(employees);
        }
    }
}
