using Domain.DTO;

namespace HRMSmvc.ViewModel
{
    public class AddEmpToDeptViewModel
    {
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public List<UserDTO> Employees { get; set; } = new();
    }
}
