using System.Text.Json.Serialization;

namespace HRManagementSystem {
    public class Leave
    {
        // Private fields
        private string leaveId;
        private string employeeId;
        private DateTime startDate;
        private DateTime endDate;
        private LeaveType type;
        private LeaveStatus status;
        private string remarks;

        // Public properties with JSON serialization
        [JsonPropertyName("leaveId")]
        public string LeaveId { get => leaveId; set => leaveId = value; }
        
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get => employeeId; set => employeeId = value; }
        
        [JsonPropertyName("startDate")]
        public DateTime StartDate { get => startDate; set => startDate = value; }
        
        [JsonPropertyName("endDate")]
        public DateTime EndDate { get => endDate; set => endDate = value; }
        
        [JsonPropertyName("type")]
        public LeaveType Type { get => type; set => type = value; }
        
        [JsonPropertyName("status")]
        public LeaveStatus Status { get => status; set => status = value; }
        
        [JsonPropertyName("remarks")]
        public string Remarks { get => remarks; set => remarks = value; }

        // Navigation property - not serialized directly
        [JsonIgnore]
        public Employee Employee { get; set; }

        // Methods
        public int CalculateDays()
        {
            return (EndDate - StartDate).Days + 1;
        }
    }

}