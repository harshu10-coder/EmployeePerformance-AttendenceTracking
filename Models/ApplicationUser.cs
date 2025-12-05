using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeePerformance_AttendenceTracking.Models
{
    public class ApplicationUser:IdentityUser
    {
      
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime JoinDate { get; set; }
        public decimal Salary { get; set; }
        public int DepartmentId { get; set; }

        //Navigation Properties
        public Department Department { get; set; }
        public List<Attendance> Attendances { get; set; }
        public List<LeaveRequest> LeaveRequests { get; set; }
        public List<Performance> Performances { get; set; }
    }
}
