using System.Text.Json.Serialization;

namespace EmployeePerformance_AttendenceTracking.Models
{
    public class Performance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Year { get; set; }
        public int Score { get; set; }
        public int Rating { get; set; }
        public string Remarks { get; set; } = string.Empty;

        //Employee
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
