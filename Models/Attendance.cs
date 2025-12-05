using System.Text.Json.Serialization;

namespace EmployeePerformance_AttendenceTracking.Models
{
    public class Attendance
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public DateTime CheckIn { get; set; } = DateTime.Now;
        public DateTime CheckOut { get; set; } = DateTime.  Now;
        public TimeSpan TotalHours { get; set; }
        public bool IsLate { get; set; }
        public string Status { get; set; } = string.Empty;

        [JsonIgnore]
        //Navigation Property
        public ApplicationUser ApplicationUser { get; set; }
    }
}
