using System.Text.Json.Serialization;

namespace HRManagementSystem {
    public class Payroll
    {
        // Private fields
        private string payrollId;
        private string employeeId;
        private DateTime payPeriodStart;
        private DateTime payPeriodEnd;
        private decimal baseSalary;
        private decimal allowances;
        private decimal deductions;
        private decimal netSalary;
        private bool isPaid;

        // Public properties with JSON serialization
        [JsonPropertyName("payrollId")]
        public string PayrollId { get => payrollId; set => payrollId = value; }
        
        [JsonPropertyName("employeeId")]
        public string EmployeeId { get => employeeId; set => employeeId = value; }
        
        [JsonPropertyName("payPeriodStart")]
        public DateTime PayPeriodStart { get => payPeriodStart; set => payPeriodStart = value; }
        
        [JsonPropertyName("payPeriodEnd")]
        public DateTime PayPeriodEnd { get => payPeriodEnd; set => payPeriodEnd = value; }
        
        [JsonPropertyName("baseSalary")]
        public decimal BaseSalary { get => baseSalary; set => baseSalary = value; }
        
        [JsonPropertyName("allowances")]
        public decimal Allowances { get => allowances; set => allowances = value; }
        
        [JsonPropertyName("deductions")]
        public decimal Deductions { get => deductions; set => deductions = value; }
        
        [JsonPropertyName("netSalary")]
        public decimal NetSalary { get => netSalary; set => netSalary = value; }
        
        [JsonPropertyName("isPaid")]
        public bool IsPaid { get => isPaid; set => isPaid = value; }

        // Navigation property - not serialized directly
        [JsonIgnore]
        public Employee Employee { get; set; }

        // Methods
        public void CalculateNetSalary()
        {
            NetSalary = BaseSalary + Allowances - Deductions;
        }

        public void GeneratePayslip()
        {
            // Logic for generating payslip
            // This would typically create a formatted report
            CalculateNetSalary();
        }
    }

}