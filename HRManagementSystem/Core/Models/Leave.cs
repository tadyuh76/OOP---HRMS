using System;
using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Leave
    {
        public string LeaveId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; } // Added EmployeeName field
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveType Type { get; set; }
        public LeaveStatus Status { get; set; }
        public string Remarks { get; set; }
        
        [JsonIgnore]
        public Employee Employee { get; set; }
        
        public Leave()
        {
            LeaveId = Guid.NewGuid().ToString("N");
        }
        
        public int CalculateDays()
        {
            return (EndDate - StartDate).Days + 1;
        }
    }
}