using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Employee : Person
    {
        // Private fields
        private string employeeId;
        private DateTime hireDate;
        private string position;
        private decimal baseSalary;
        private string departmentId;
        private EmployeeStatus status;

        // Public properties with JSON serialization
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get => employeeId; set => employeeId = value; }

        [JsonPropertyName("hireDate")]
        public DateTime HireDate { get => hireDate; set => hireDate = value; }

        [JsonPropertyName("position")]
        public string Position { get => position; set => position = value; }

        [JsonPropertyName("baseSalary")]
        public decimal BaseSalary { get => baseSalary; set => baseSalary = value; }

        [JsonPropertyName("departmentId")]
        public string DepartmentId { get => departmentId; set => departmentId = value; }

        [JsonPropertyName("status")]
        public EmployeeStatus Status { get => status; set => status = value; }

        // Navigation property - not serialized directly
        [JsonIgnore]
        public Department Department { get; set; }

        // Methods
        public int CalculateYearsOfService()
        {
            return (DateTime.Today - HireDate).Days / 365;
        }

        public virtual decimal CalculateSalary()
        {
            return BaseSalary;
        }
    }
}