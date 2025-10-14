using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class AddEmpReq
    {
        public Guid DepartmentId { get; set; }
        public List<string> EmployeeIds { get; set; }
    }
}
