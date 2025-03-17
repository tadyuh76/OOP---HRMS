using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class ContractEmployee : Employee
    {
        // Private fields
        private decimal hourlyRate;
        private int hoursWorked;

        // Public properties with JSON serialization
        [JsonPropertyName("hourlyRate")]
        public decimal HourlyRate { get => hourlyRate; set => hourlyRate = value; }

        [JsonPropertyName("hoursWorked")]
        public int HoursWorked { get => hoursWorked; set => hoursWorked = value; }

        // Methods
        public override decimal CalculateSalary()
        {
            return HourlyRate * HoursWorked;
        }

        // Type discriminator for JSON deserialization
        [JsonPropertyName("employeeType")]
        public string EmployeeType => "Contract";
    }


}