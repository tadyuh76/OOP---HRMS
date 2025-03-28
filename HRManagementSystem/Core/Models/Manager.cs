using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Manager : Employee
    {
        private List<string> managedEmployeeIds;
        private decimal budgetResponsibility;

        public Manager() : base()
        {
            managedEmployeeIds = new List<string>();
        }

        public Manager(
            string id,
            string name,
            string email,
            string phone,
            DateTime dateOfBirth,
            string address,
            string employeeId,
            DateTime hireDate,
            string position,
            decimal baseSalary,
            string departmentId,
            EmployeeStatus status,
            List<string> managedEmployeeIds,
            decimal budgetResponsibility) : base(id, name, email, phone, dateOfBirth, address, employeeId, hireDate, position, baseSalary, departmentId, status)
        {
            this.managedEmployeeIds = managedEmployeeIds ?? new List<string>();
            this.budgetResponsibility = budgetResponsibility;
        }

        [JsonPropertyName("managedEmployeeIds")]
        public List<string> ManagedEmployeeIds
        {
            get { return managedEmployeeIds; }
            set { managedEmployeeIds = value ?? new List<string>(); }
        }

        [JsonPropertyName("budgetResponsibility")]
        public decimal BudgetResponsibility
        {
            get { return budgetResponsibility; }
            set { budgetResponsibility = value; }
        }

        // public void ApproveLeaveRequest(LeaveRequest request)
        // {
        //     if (request == null)
        //     {
        //         throw new ArgumentNullException(nameof(request));
        //     }

        //     if (request.LeaveDetails != null)
        //     {
        //         request.LeaveDetails.Status = LeaveStatus.Approved;
        //     }
        // }

        // public void RejectLeaveRequest(LeaveRequest request)
        // {
        //     if (request == null)
        //     {
        //         throw new ArgumentNullException(nameof(request));
        //     }

        //     if (request.LeaveDetails != null)
        //     {
        //         request.LeaveDetails.Status = LeaveStatus.Rejected;
        //     }
        // }
    }

}