using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class ContractEmployee : Employee
    {
        private decimal hourlyRate;
        private int hoursWorked;

        public ContractEmployee() : base()
        {
            // Default constructor required for JSON serialization
        }

        public ContractEmployee(
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
            decimal hourlyRate,
            int hoursWorked) : base(id, name, email, phone, dateOfBirth, address, employeeId, hireDate, position, baseSalary, departmentId, status)
        {
            this.hourlyRate = hourlyRate;
            this.hoursWorked = hoursWorked;
        }

        [JsonPropertyName("hourlyRate")]
        public decimal HourlyRate
        {
            get { return hourlyRate; }
            set { hourlyRate = value; }
        }

        [JsonPropertyName("hoursWorked")]
        public int HoursWorked
        {
            get { return hoursWorked; }
            set { hoursWorked = value; }
        }

        public override decimal CalculateSalary()
        {
            return hourlyRate * hoursWorked;
        }
    }

}