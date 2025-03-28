using System;
using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Attendance
    {
        public string AttendanceId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; } // Added EmployeeName field
        public DateTime Date { get; set; }
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set; }
        public AttendanceStatus Status { get; set; }
        
        [JsonIgnore]
        public Employee Employee { get; set; }
        
        public Attendance()
        {
            AttendanceId = Guid.NewGuid().ToString("N");
        }
    }
}