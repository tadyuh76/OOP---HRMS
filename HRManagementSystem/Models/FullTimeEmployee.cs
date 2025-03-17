using System.Text.Json.Serialization;

namespace HRManagementSystem {
    public class FullTimeEmployee : Employee
    {
        // Private fields
        private decimal annualBonus;

        // Public properties with JSON serialization
        [JsonPropertyName("annualBonus")]
        public decimal AnnualBonus { get => annualBonus; set => annualBonus = value; }

        // Methods
        public override decimal CalculateSalary()
        {
            return BaseSalary + (AnnualBonus / 12);
        }

        // Type discriminator for JSON deserialization
        [JsonPropertyName("employeeType")]
        public string EmployeeType => "FullTime";
    }

}