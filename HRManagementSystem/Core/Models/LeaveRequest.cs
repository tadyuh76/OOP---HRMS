using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class LeaveRequest
    {
        private string requestId;
        private string employeeId;
        private DateTime requestDate;
        private Leave leaveDetails;
        private string approverId;

        public LeaveRequest()
        {
            // Default constructor required for JSON serialization
        }

        public LeaveRequest(
            string requestId,
            string employeeId,
            DateTime requestDate,
            Leave leaveDetails,
            string approverId)
        {
            this.requestId = requestId ?? throw new ArgumentNullException(nameof(requestId));
            this.employeeId = employeeId ?? throw new ArgumentNullException(nameof(employeeId));
            this.requestDate = requestDate;
            this.leaveDetails = leaveDetails ?? throw new ArgumentNullException(nameof(leaveDetails));
            this.approverId = approverId ?? throw new ArgumentNullException(nameof(approverId));
        }

        [JsonPropertyName("requestId")]
        public string RequestId
        {
            get { return requestId; }
            set { requestId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("employeeId")]
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("requestDate")]
        public DateTime RequestDate
        {
            get { return requestDate; }
            set { requestDate = value; }
        }

        [JsonPropertyName("leaveDetails")]
        public Leave LeaveDetails
        {
            get { return leaveDetails; }
            set { leaveDetails = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("approverId")]
        public string ApproverId
        {
            get { return approverId; }
            set { approverId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        public void Submit()
        {
            if (leaveDetails != null)
            {
                leaveDetails.Status = LeaveStatus.Pending;
            }
        }

        public void Cancel()
        {
            if (leaveDetails != null)
            {
                leaveDetails.Status = LeaveStatus.Cancelled;
            }
        }
    }
}