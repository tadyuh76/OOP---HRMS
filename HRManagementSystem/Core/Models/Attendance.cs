using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Attendance
    {
        [JsonPropertyName("attendanceId")]
        public string AttendanceId { get; set; } = string.Empty;

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get; set; } = string.Empty;

        [JsonPropertyName("employeeName")]
        public string EmployeeName { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("clockInTime")]
        public DateTime ClockInTime { get; set; }

        [JsonPropertyName("clockOutTime")]
        public DateTime ClockOutTime { get; set; }

        [JsonPropertyName("status")]
        public AttendanceStatus Status { get; set; }

        [JsonPropertyName("isAbsentRecord")]
        public bool IsAbsentRecord { get; set; } = false;

        [JsonIgnore]
        public Employee? Employee { get; set; }
    }
}