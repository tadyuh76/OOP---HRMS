using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class LeaveRequest
    {
        // Private fields
        private string requestId;
        private string employeeId;
        private DateTime requestDate;
        private Leave leaveDetails;
        private string approverId;

        // Public properties with JSON serialization
        [JsonPropertyName("requestId")]
        public string RequestId { get => requestId; set => requestId = value; }

        [JsonPropertyName("employeeId")]
        public string EmployeeId { get => employeeId; set => employeeId = value; }

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate { get => requestDate; set => requestDate = value; }

        [JsonPropertyName("leaveDetails")]
        public Leave LeaveDetails { get => leaveDetails; set => leaveDetails = value; }

        [JsonPropertyName("approverId")]
        public string ApproverId { get => approverId; set => approverId = value; }

        // Methods
        public void Submit()
        {
            RequestDate = DateTime.Now;
            LeaveDetails.Status = LeaveStatus.Pending;
        }

        public void Cancel()
        {
            LeaveDetails.Status = LeaveStatus.Cancelled;
        }
    }


}