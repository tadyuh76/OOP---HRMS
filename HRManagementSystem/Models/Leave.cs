using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Leave
    {
        private string leaveId;
        private string employeeId;
        private DateTime startDate;
        private DateTime endDate;
        private LeaveType type;
        private LeaveStatus status;
        private string remarks;

        public Leave()
        {
            // Default constructor required for JSON serialization
        }

        public Leave(
            string leaveId,
            string employeeId,
            DateTime startDate,
            DateTime endDate,
            LeaveType type,
            LeaveStatus status,
            string remarks)
        {
            this.leaveId = leaveId ?? throw new ArgumentNullException(nameof(leaveId));
            this.employeeId = employeeId ?? throw new ArgumentNullException(nameof(employeeId));
            this.startDate = startDate;
            this.endDate = endDate;
            this.type = type;
            this.status = status;
            this.remarks = remarks;
        }

        [JsonPropertyName("leaveId")]
        public string LeaveId
        {
            get { return leaveId; }
            set { leaveId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("employeeId")]
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("startDate")]
        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        [JsonPropertyName("endDate")]
        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        [JsonPropertyName("type")]
        public LeaveType Type
        {
            get { return type; }
            set { type = value; }
        }

        [JsonPropertyName("status")]
        public LeaveStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        [JsonPropertyName("remarks")]
        public string Remarks
        {
            get { return remarks; }
            set { remarks = value; }
        }

        [JsonPropertyName("employee")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Employee Employee { get; set; }

        public int CalculateDays()
        {
            TimeSpan duration = endDate - startDate;
            return duration.Days + 1; // Including both start and end dates
        }
    }
}