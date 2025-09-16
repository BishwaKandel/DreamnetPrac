using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Models
{
    public class Department
    {
        public Guid Id { get; set; }

        [ForeignKey("Employee")]
        public Guid EId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
