//using Domain.Models.Base;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Domain.Models
//{
//    public class Attendance : EntityBase

//    {
//        [ForeignKey("User")]
//        public Guid UserId { get; set; }
//        public User User { get; set; }
//        public DateTime Date { get; set; }
//        public TimeOnly CheckInTime { get; set; }
//        public TimeOnly CheckOutTime { get; set; }
//        public TimeSpan TotalHoursWorked { get; set; }
//        public string status { get; set; }
//    }
//}
