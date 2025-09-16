//using Domain.Models.Base;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Domain.Models
//{
//    public class LeaveRequest : EntityBase
//    {

//        [ForeignKey("User")]
//        public Guid Id { get; set; }
//        public User User { get; set; } 
//        public DateOnly StartDate { get; set; }
//        public DateOnly EndDate { get; set; }
//        public string Reason { get; set; }
//        public string Status { get; set; }
//        public string LeaveType { get; set; }

//    }
//}
