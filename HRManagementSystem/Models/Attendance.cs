using System.Text.Json.Serialization;

namespace HRManagementSystem {
    public class Attendance
    {
        // Private fields
        private string attendanceId;
        private string employeeId;
        private DateTime date;
        private DateTime clockInTime;
        private DateTime clockOutTime;
        private AttendanceStatus status;

        // Public properties with JSON serialization
        [JsonPropertyName("attendanceId")]
        public string AttendanceId { get => attendanceId; set => attendanceId = value; }
        
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get => employeeId; set => employeeId = value; }
        
        [JsonPropertyName("date")]
        public DateTime Date { get => date; set => date = value; }
        
        [JsonPropertyName("clockInTime")]
        public DateTime ClockInTime { get => clockInTime; set => clockInTime = value; }
        
        [JsonPropertyName("clockOutTime")]
        public DateTime ClockOutTime { get => clockOutTime; set => clockOutTime = value; }
        
        [JsonPropertyName("status")]
        public AttendanceStatus Status { get => status; set => status = value; }

        // Navigation property - not serialized directly
        [JsonIgnore]
        public Employee Employee { get; set; }

        // Methods
        public TimeSpan CalculateWorkHours()
        {
            if (ClockOutTime > ClockInTime)
            {
                return ClockOutTime - ClockInTime;
            }
            return TimeSpan.Zero;
        }

        public bool IsLate()
        {
            // Assuming work starts at 9:00 AM
            var workStartTime = new DateTime(Date.Year, Date.Month, Date.Day, 9, 0, 0);
            return ClockInTime > workStartTime;
        }
    }

}