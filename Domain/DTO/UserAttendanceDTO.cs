using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class UserAttendanceDTO
    {
        public string userId { get; set; }
        public string Name { get; set; }

        public List<AttendanceDTO> attendances { get; set; }
    }
}
