using System.Diagnostics;
using System.Text.Json;
namespace HRManagementSystem
{
    public class PayrollService : IService<Payroll>
    {
        private List<Payroll> payrolls;
        private readonly string payrollDataPath = FileManager.payrollDataPath;
        public PayrollService()
        {
            if (File.Exists(payrollDataPath))
            {
                string jsonData = File.ReadAllText(payrollDataPath);
                this.payrolls = JsonSerializer.Deserialize<List<Payroll>>(jsonData);
            }
            else
            {
                this.payrolls = new List<Payroll>();
            }

        }

        public List<Payroll> GetAll()
        {
            return new List<Payroll>(this.payrolls);
        }

        public List<Payroll> GetPayrollsByMonth(DateTime month)
        {
            List<Payroll> result = new List<Payroll>();
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                Payroll payroll = this.payrolls[i];
                if (payroll.PayPeriodStart.Year == month.Year &&
                    payroll.PayPeriodStart.Month == month.Month)
                {
                    result.Add(payroll);
                }
            }
            return result;
        }

        public Payroll GetById(string id)
        {
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                Payroll payroll = this.payrolls[i];
                if (payroll.PayrollId == id)
                {
                    return payroll;
                }
            }
            return null;
        }

        public List<Payroll> GetPayrollsByEmployee(string employeeName)
        {
            List<Payroll> result = new List<Payroll>();
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                Payroll payroll = this.payrolls[i];
                if (payroll.EmployeeName == employeeName)
                {
                    result.Add(payroll);
                }
            }
            return result;
        }

        public bool Add(Payroll payroll)
        {
            if (payroll == null)
            {
                throw new ArgumentNullException("payroll");
            }

            if (string.IsNullOrEmpty(payroll.PayrollId))
            {
                int maxId = 0;
                for (int i = 0; i < this.payrolls.Count; i++)
                {
                    Payroll existingPayroll = this.payrolls[i];
                    if (!string.IsNullOrEmpty(existingPayroll.PayrollId))
                    {
                        string[] parts = existingPayroll.PayrollId.Split('-');
                        if (parts.Length == 3)
                        {
                            int currentId;
                            if (int.TryParse(parts[2], out currentId) && currentId > maxId)
                            {
                                maxId = currentId;
                            }
                        }
                    }
                }
                payroll.PayrollId = "PR-" + DateTime.Now.Year + "-" + (maxId + 1).ToString("D3");

            }

            for (int i = 0; i < this.payrolls.Count; i++)
            {
                Payroll existingPayroll = this.payrolls[i];
                if (existingPayroll.PayrollId == payroll.PayrollId)
                {
                    throw new InvalidOperationException("Payroll ID already exists");
                    return false;
                }
            }

            this.payrolls.Add(payroll);

            // Ghi danh sách cập nhật trở lại tệp
            string jsonData = JsonSerializer.Serialize(payrolls, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(payrollDataPath, jsonData);
            return true;

        }

        public bool Update(Payroll payroll)
        {
            if (payroll == null)
            {
                throw new ArgumentNullException("payroll");
            }

            for (int i = 0; i < this.payrolls.Count; i++)
            {
                if (this.payrolls[i].PayrollId == payroll.PayrollId)
                {
                    this.payrolls[i] = payroll;
                    this.payrolls.RemoveAt(i);
                    // Thêm dữ liệu mới vào danh sách
                    payrolls.Add(payroll);

                    // Ghi danh sách cập nhật trở lại tệp
                    string jsonData = JsonSerializer.Serialize(payrolls, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(payrollDataPath, jsonData);
                    return true;
                }
            }
            throw new InvalidOperationException("Payroll not found");
            return false;

        }

        public bool Delete(string id)
        {
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                if (this.payrolls[i].PayrollId == id)
                {
                    this.payrolls.RemoveAt(i);

                    // Ghi danh sách cập nhật trở lại tệp
                    string jsonData = JsonSerializer.Serialize(payrolls, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(payrollDataPath, jsonData);
                    return true;
                }
            }
            throw new InvalidOperationException("Payroll not found");
            return false;
        }

        public decimal CalculateNetSalary(decimal baseSalary, decimal allowances, decimal deductions)
        {
            return baseSalary + allowances - deductions;
        }

        public decimal CalculateTotalPayroll(List<Payroll> payrolls)
        {
            decimal total = 0;
            for (int i = 0; i < payrolls.Count; i++)
            {
                total += payrolls[i].NetSalary;
            }
            return total;
        }

        public decimal CalculateAverageSalary(List<Payroll> payrolls)
        {
            if (payrolls.Count == 0)
            {
                return 0;
            }
            return CalculateTotalPayroll(payrolls) / payrolls.Count;
        }

        public void GetSalaryRange(List<Payroll> payrolls, out decimal minSalary, out decimal maxSalary)
        {
            minSalary = decimal.MaxValue;
            maxSalary = decimal.MinValue;

            if (payrolls.Count == 0)
            {
                minSalary = 0;
                maxSalary = 0;
                return;
            }

            for (int i = 0; i < payrolls.Count; i++)
            {
                Payroll payroll = payrolls[i];
                if (payroll.NetSalary < minSalary)
                {
                    minSalary = payroll.NetSalary;
                }
                if (payroll.NetSalary > maxSalary)
                {
                    maxSalary = payroll.NetSalary;
                }
            }
        }
    }
}
