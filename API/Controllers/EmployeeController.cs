using Application.Interface;
using Domain.DTO;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "User")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        //private readonly IDeptEmpService _deptEmpService;
        private readonly IImageService _imageService;
        private readonly IBirthdayEmailService _birthdayEmailService;
        private readonly IEmailService _email;

        public EmployeeController(IEmployeeService employeeService , IImageService imageservice, IBirthdayEmailService birthdayEmailService , IEmailService email   )
        {
            _employeeService = employeeService;
            //_deptEmpService = deptEmpService;
            _imageService = imageservice;
            _birthdayEmailService = birthdayEmailService;
            _email = email;
        }

        //Create
        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateEmp")]
        public async Task<IActionResult> CreateEmployeeAsync([FromBody] UserDTO employee)
        {
            var CreatedEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction(
                    nameof(GetEmpById),
                    CreatedEmployee);
        }


        //Retrieve All
        //[Authorize(Roles = "User")]
        [HttpGet("GetEmp")]
        public async Task<IActionResult> GetAllEmployees(Guid? deptId)
        {
            var emp = await _employeeService.GetAllEmployeesAsync(deptId);
            return Ok(emp);
        }

        //Retrieve by ID

        [HttpGet("GetEmpbyID")]

        public async Task<IActionResult> GetEmpById(string Id)
        {

            var emp = await _employeeService.GetEmployeeByIdAsync(Id);
            return Ok(emp);
        }


        //Change Photo 

        [HttpPost("ChangePhoto")]

        public async Task<IActionResult> ChangePhoto([FromForm] ChangePpDTO employee)
        {
            string profileImagePath = null;

            if (employee.formFile != null)
            {
                profileImagePath = await _imageService.UploadImageAsync(employee.formFile);
                employee.ProfilePictureFileName = profileImagePath;
            }
            
            var updatedEmployee = await _employeeService.UpdateProfilePicAsync(employee);
            return Ok(updatedEmployee);
        }

        //Delete Photo

        [HttpPost("DeletePhoto")]
        public async Task<IActionResult> DeletePhoto([FromBody] ChangePpDTO employee)
        {
            if (!string.IsNullOrEmpty(employee.ProfilePictureFileName))
            {
                await _imageService.DeleteImageAsync(employee.ProfilePictureFileName);
                employee.ProfilePictureFileName = null;
            }
            var updatedEmployee = await _employeeService.DeleteProfilePicAsync(employee);
            return Ok(updatedEmployee);
        }


        //Update Employee

        [HttpPost("UpdateEmp")]

        public async Task<IActionResult> UpdateEmployeeAsync([FromBody] UserUpdateDTO employee)
        {
            
            var updatedEmployee = await _employeeService.UpdateEmployeeAsync(employee);
            if (updatedEmployee.Data == null)
            {
                return NotFound();
            }
            return Ok(updatedEmployee);
        }

        //Delete by EmpID

        [HttpDelete("DeleteEmp")]
        public async Task<IActionResult> DeleteEmployeeAsync(Guid id)
        {
            var deleted = await _employeeService.DeleteEmployeeAsync(id);
            if (deleted == false)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordDTO changePasswordDTO)
        {
            var result = await _employeeService.ChangePasswordAsync(changePasswordDTO);
            if (result.success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        //Add Employee to Department
        //[HttpPost("AddEmpToDept")]
        //public async Task<IActionResult> AddEmpToDept(DeptEmpDTO deptEmp)
        //{
        //    var addedDeptEmp = await _deptEmpService.AddEmpToDept(deptEmp);
        //    return CreatedAtAction(
        //            nameof(GetEmpById),
        //            addedDeptEmp);
        //}

        //Get departmnet by Employee ID

        //[HttpGet("GetDeptByEmpId{id}")]

        //public async Task<IActionResult> GetDepartmentByEmployeeId(Guid id)
        //{
        //    var departments = await _deptEmpService.GetDeptByEmployeeIdAsync(id);
        //    if (departments == null || !departments.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(departments);
        //}

        //[HttpGet("/department")]

        //public async Task<IActionResult> GetDepartmentByEmployeeId(Guid id)
        //{
        //    var department = await _employeeService.GetDepartmentsByEmployeeIdAsync(id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(department);
        //}

        ////Get Leave Request by Employee ID
        //[HttpGet("GetLeaveRequestByEmpId{id}")]

        //public async Task<IActionResult> GetLeaveRequestByEmployeeId(Guid id)
        //{
        //    var leaveRequests = await _employeeService.GetLeaveRequestsByEmployeeIdAsync(id);
        //    if (leaveRequests == null || !leaveRequests.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(leaveRequests);
        //}

        ////Get Payrolls by Employee ID
        //[HttpGet("GetPayrollsByEmpId{id}")]
        //public async Task<IActionResult> GetPayrollsByEmployeeId(Guid id)
        //{
        //    var payrolls = await _employeeService.GetPayrollsByEmployeeIdAsync(id);
        //    if (payrolls == null || !payrolls.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(payrolls);
        //}

        //[HttpPost("send")]
        //public async Task<IActionResult> SendBirthdayEmails()
        //{
        //    await _birthdayEmailService.SendBirthdayEmailsAsync();
        //    return Ok("Birthday emails sent.");
        //}
        
        
    }

}
