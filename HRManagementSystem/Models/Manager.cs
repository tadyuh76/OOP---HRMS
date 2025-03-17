using System.Text.Json.Serialization;

namespace HRManagementSystem {
    public class Manager : Employee
    {
        // Private fields
        private List<string> managedEmployeeIds;
        private decimal budgetResponsibility;

        // Public properties with JSON serialization
        [JsonPropertyName("managedEmployeeIds")]
        public List<string> ManagedEmployeeIds { get => managedEmployeeIds; set => managedEmployeeIds = value; }
        
        [JsonPropertyName("budgetResponsibility")]
        public decimal BudgetResponsibility { get => budgetResponsibility; set => budgetResponsibility = value; }

        // Type discriminator for JSON deserialization
        [JsonPropertyName("employeeType")]
        public string EmployeeType => "Manager";

        // Methods
        public void ApproveLeaveRequest(LeaveRequest request)
        {
            if (request != null && request.LeaveDetails != null)
            {
                request.LeaveDetails.Status = LeaveStatus.Approved;
            }
        }

        public void RejectLeaveRequest(LeaveRequest request)
        {
            if (request != null && request.LeaveDetails != null)
            {
                request.LeaveDetails.Status = LeaveStatus.Rejected;
            }
        }
    }

}