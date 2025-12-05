using System.Text.Json.Serialization;

namespace EmployeePerformance_AttendenceTracking.Models
{
    public class LeaveRequest
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateTime FromDate { get; set; } = DateTime.UtcNow;

        public DateTime ToDate { get; set; } = DateTime.UtcNow;

        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        //Navigaion Property
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
