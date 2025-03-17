using System.Text.Json.Serialization;

namespace HRManagementSystem {
    public class Department
    {
        // Private fields
        private string departmentId;
        private string name;
        private string description;
        private decimal budget;
        private string managerId;

        // Public properties with JSON serialization
        [JsonPropertyName("departmentId")]
        public string DepartmentId { get => departmentId; set => departmentId = value; }
        
        [JsonPropertyName("name")]
        public string Name { get => name; set => name = value; }
        
        [JsonPropertyName("description")]
        public string Description { get => description; set => description = value; }
        
        [JsonPropertyName("budget")]
        public decimal Budget { get => budget; set => budget = value; }
        
        [JsonPropertyName("managerId")]
        public string ManagerId { get => managerId; set => managerId = value; }

        // Navigation properties - not serialized directly
        [JsonIgnore]
        public List<Employee> Employees { get; set; } = new List<Employee>();
        
        [JsonIgnore]
        public Employee Manager { get; set; }

        // Methods
        public int GetEmployeeCount()
        {
            return Employees?.Count ?? 0;
        }

        public decimal GetBudgetUtilization()
        {
            decimal totalSalaries = 0;
            if (Employees != null)
            {
                foreach (var employee in Employees)
                {
                    totalSalaries += employee.CalculateSalary();
                }
            }
            return Budget > 0 ? totalSalaries / Budget * 100 : 0;
        }
    }

}