using System.Text.Json.Serialization;

namespace EmployeePerformance_AttendenceTracking.Models
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<ApplicationUser> ApplicationUser { get; set; } = new List<ApplicationUser>();

    }
}
