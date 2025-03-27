using System.Text.Json.Serialization;

namespace HRManagementSystem
{
    public class Payroll
    {
        private string payrollId;
        private string employeeId;
        private string employeeName;
        private DateTime payPeriodStart;
        private DateTime payPeriodEnd;
        private decimal baseSalary;
        private decimal allowances;
        private decimal deductions;
        private decimal netSalary;
        private bool isPaid;

        public Payroll()
        {
            // Default constructor required for JSON serialization
        }

        public Payroll(
            string payrollId,
            string employeeId,
            string employeeName,
            DateTime payPeriodStart,
            DateTime payPeriodEnd,
            decimal baseSalary,
            decimal allowances,
            decimal deductions,
            decimal netSalary,
            bool isPaid)
        {
            this.payrollId = payrollId ?? throw new ArgumentNullException(nameof(payrollId));
            this.employeeId = employeeId ?? throw new ArgumentNullException(nameof(employeeId));
            this.employeeName = employeeName;
            this.payPeriodStart = payPeriodStart;
            this.payPeriodEnd = payPeriodEnd;
            this.baseSalary = baseSalary;
            this.allowances = allowances;
            this.deductions = deductions;
            this.netSalary = netSalary;
            this.isPaid = isPaid;
        }

        [JsonPropertyName("payrollId")]
        public string PayrollId
        {
            get { return payrollId; }
            set { payrollId = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        [JsonPropertyName("employeeId")]
        public string EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value ?? throw new ArgumentNullException(nameof(value)); }
        }
        [JsonPropertyName("employeeName")]
        public string EmployeeName { 
            get { return employeeName; }
            set { employeeName = value; }
        }
        [JsonPropertyName("payPeriodStart")]
        public DateTime PayPeriodStart
        {
            get { return payPeriodStart; }
            set { payPeriodStart = value; }
        }

        [JsonPropertyName("payPeriodEnd")]
        public DateTime PayPeriodEnd
        {
            get { return payPeriodEnd; }
            set { payPeriodEnd = value; }
        }

        [JsonPropertyName("baseSalary")]
        public decimal BaseSalary
        {
            get { return baseSalary; }
            set { baseSalary = value; }
        }

        [JsonPropertyName("allowances")]
        public decimal Allowances
        {
            get { return allowances; }
            set { allowances = value; }
        }

        [JsonPropertyName("deductions")]
        public decimal Deductions
        {
            get { return deductions; }
            set { deductions = value; }
        }

        [JsonPropertyName("netSalary")]
        public decimal NetSalary
        {
            get { return netSalary; }
            set { netSalary = value; }
        }

        [JsonPropertyName("isPaid")]
        public bool IsPaid
        {
            get { return isPaid; }
            set { isPaid = value; }
        }

        [JsonPropertyName("employee")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Employee Employee { get; set; }

        public void CalculateNetSalary()
        {
            netSalary = baseSalary + allowances - deductions;
        }

        public Payroll GeneratePayslip(DateTime month)
        {

            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            
            Payroll newPayroll = new Payroll
            {
                // PayrollId sẽ được tạo bởi service
                EmployeeId = this.EmployeeId,
                EmployeeName = this.EmployeeName,
                PayPeriodStart = startDate,
                PayPeriodEnd = endDate,
                BaseSalary = this.BaseSalary,
                Allowances = this.Allowances,
                Deductions = this.Deductions,
                IsPaid = false
            };

            
            newPayroll.NetSalary = newPayroll.BaseSalary + newPayroll.Allowances - newPayroll.Deductions;

            return newPayroll;
        }

    }
}