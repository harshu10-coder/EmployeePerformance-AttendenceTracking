namespace EmployeePerformance_AttendenceTracking.DTOs
{
    public class AttendenceDto
    {
        public int AttendanceId { get; set; }
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public DateTime CheckIn { get; set; } = DateTime.UtcNow;
        public DateTime CheckOut { get; set; } = DateTime.UtcNow;
        public TimeSpan TotalHours { get; set; }
        public bool IsLate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
