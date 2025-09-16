using Domain.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Department :EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
    }
}
