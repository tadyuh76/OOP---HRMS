using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Attendance
    {
        private string attendanceId;
        private string employeeId;
        private DateTime date;
        private DateTime clockInTime;
        private DateTime clockOutTime;
        private AttendanceStatus status;

        public Attendance()
        {
            // Default constructor required for JSON serialization
        }

        public Attendance(
            string attendanceId,
            string employeeId,
            DateTime date,
            DateTime clockInTime,
            DateTime clockOutTime,
            AttendanceStatus status)
        {
            this.attendanceId = attendanceId ?? throw new ArgumentNullException(nameof(attendanceId));
            this.employeeId = employeeId ?? throw new ArgumentNullException(nameof(employeeId));
            this.date = date;
            this.clockInTime = clockInTime;
            this.clockOutTime = clockOutTime;
            this.status = status;
        }

        [JsonPropertyName("attendanceId")]
        public string AttendanceId
        {
            get { return attendanceId; }
            set { attendanceId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("employeeId")]
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("date")]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [JsonPropertyName("clockInTime")]
        public DateTime ClockInTime
        {
            get { return clockInTime; }
            set { clockInTime = value; }
        }

        [JsonPropertyName("clockOutTime")]
        public DateTime ClockOutTime
        {
            get { return clockOutTime; }
            set { clockOutTime = value; }
        }

        [JsonPropertyName("status")]
        public AttendanceStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        [JsonPropertyName("employee")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Employee Employee { get; set; }

        public TimeSpan CalculateWorkHours()
        {
            if (clockOutTime > clockInTime)
            {
                return clockOutTime - clockInTime;
            }
            return TimeSpan.Zero;
        }

        public bool IsLate()
        {
            // Assuming standard work start time is 9:00 AM
            DateTime standardTime = new DateTime(date.Year, date.Month, date.Day, 9, 0, 0);
            return clockInTime > standardTime;
        }
    }

}