using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace HRManagementSystem
{
    public interface IPayrollService
    {
        List<Payroll> GetAllPayrolls();
        List<Payroll> GetPayrollsByMonth(DateTime month);
        Payroll GetPayrollById(string id);
        List<Payroll> GetPayrollsByEmployee(string employeeName);
        void AddPayroll(Payroll payroll);
        void UpdatePayroll(Payroll payroll);
        void DeletePayroll(string id);
        decimal CalculateNetSalary(decimal baseSalary, decimal allowances, decimal deductions);
        decimal CalculateTotalPayroll(List<Payroll> payrolls);
        decimal CalculateAverageSalary(List<Payroll> payrolls);
        void GetSalaryRange(List<Payroll> payrolls, out decimal minSalary, out decimal maxSalary);
        void CreateSampleData();
    
    }
    public class PayrollService : IPayrollService
    {
        private List<Payroll> payrolls;
        private readonly JsonFileStorage _fileStorage;
        private readonly string _dataDirectory = "Data";
        private readonly string PAYROLL_FILE_PATH;

        public PayrollService()
        {
            this._fileStorage = new JsonFileStorage();

            // Lấy đường dẫn thư mục thực thi
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Tạo đường dẫn đến thư mục Data
            _dataDirectory = Path.Combine(baseDirectory, "Data");

            // Đảm bảo thư mục tồn tại
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            // Tạo đường dẫn đầy đủ đến file
            PAYROLL_FILE_PATH = Path.Combine(_dataDirectory, "Payroll.json");
            this.payrolls = this.LoadPayrolls();
        }

        private List<Payroll> LoadPayrolls()
        {
            try
            {
                return this._fileStorage.LoadData<List<Payroll>>(PAYROLL_FILE_PATH) ?? new List<Payroll>();
            }
            catch
            {
                return new List<Payroll>();
            }
        }

        private void SavePayrolls()
        {
            string jsonData = System.Text.Json.JsonSerializer.Serialize(this.payrolls);
            this._fileStorage.SaveData(jsonData, PAYROLL_FILE_PATH);
        }

        public List<Payroll> GetAllPayrolls()
        {
            return new List<Payroll>(this.payrolls);
        }

        public List<Payroll> GetPayrollsByMonth(DateTime month)
        {
            List<Payroll> result = new List<Payroll>();
            foreach (Payroll payroll in this.payrolls)
            {
                if (payroll.PayPeriodStart.Year == month.Year &&
                    payroll.PayPeriodStart.Month == month.Month)
                {
                    result.Add(payroll);
                }
            }
            return result;
        }

        public Payroll GetPayrollById(string id)
        {
            foreach (Payroll payroll in this.payrolls)
            {
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
            foreach (Payroll payroll in this.payrolls)
            {
                if (payroll.EmployeeName == employeeName)
                {
                    result.Add(payroll);
                }
            }
            return result;
        }

        public void AddPayroll(Payroll payroll)
        {
            if (payroll == null)
            {
                throw new ArgumentNullException("payroll");
            }

            foreach (Payroll p in this.payrolls)
            {
                if (p.PayrollId == payroll.PayrollId)
                {
                    throw new InvalidOperationException("Payroll ID already exists");
                }
            }

            this.payrolls.Add(payroll);
            this.SavePayrolls();
        }

        public void UpdatePayroll(Payroll payroll)
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
                    this.SavePayrolls();
                    return;
                }
            }
            throw new InvalidOperationException("Payroll not found");
        }

        public void DeletePayroll(string id)
        {
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                if (this.payrolls[i].PayrollId == id)
                {
                    this.payrolls.RemoveAt(i);
                    this.SavePayrolls();
                    return;
                }
            }
            throw new InvalidOperationException("Payroll not found");
        }

        public decimal CalculateNetSalary(decimal baseSalary, decimal allowances, decimal deductions)
        {
            return baseSalary + allowances - deductions;
        }

        public decimal CalculateTotalPayroll(List<Payroll> payrolls)
        {
            decimal total = 0;
            foreach (Payroll payroll in payrolls)
            {
                total += payroll.NetSalary;
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

            foreach (Payroll payroll in payrolls)
            {
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

        public void CreateSampleData()
        {
            PayrollService employeeService = new PayrollService();

            List<Payroll> samplePayrolls = new List<Payroll>
            {
            new Payroll
            {
                PayrollId = "PR-2025-001",
                EmployeeId = "p001",
                EmployeeName = "John Smith",
                PayPeriodStart=  new DateTime(2025, 02, 01),
                PayPeriodEnd = new DateTime(2025,02,28),
                BaseSalary = 4500,
                Allowances = 750,
                Deductions = 850,
                NetSalary =  4400,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PR-2025-002",
                EmployeeId = "o002",
                EmployeeName = "Emily Johnson",
                PayPeriodStart = new DateTime(2025, 02, 01),
                PayPeriodEnd = new DateTime(2025,02,28),
                BaseSalary = 3800,
                Allowances = 500,
                Deductions = 750,
                NetSalary =  3550,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PR-2025-003",
                EmployeeId = "p003",
                EmployeeName = "Michael Brown",
                PayPeriodStart = new DateTime(2025, 02, 01),
                PayPeriodEnd = new DateTime(2025,02,28),
                BaseSalary = 5200,
                Allowances = 850,
                Deductions = 1200,
                NetSalary = 4850,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PR-2025-004",
                EmployeeId = "o004",
                EmployeeName = "Sarah Davis",
                PayPeriodStart = new DateTime(2025, 02, 01),
                PayPeriodEnd = new DateTime(2025,02,28),
                BaseSalary = 4000,
                Allowances = 600,
                Deductions = 780,
                NetSalary = 3820,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PR-2025-005",
                EmployeeId = "p001",
                EmployeeName = "James Wilson",
                PayPeriodStart = new DateTime(2025,03,01),
                PayPeriodEnd = new DateTime(2025,03,31),
                BaseSalary = 4500,
                Allowances = 750,
                Deductions = 850,
                NetSalary = 4400,
                IsPaid = false
            },
            new Payroll
            {
                PayrollId = "PR-2025-006",
                EmployeeId = "o002",
                EmployeeName = "Jessica Lee",
                PayPeriodStart = new DateTime(2025,03,01),
                PayPeriodEnd = new DateTime(2025,03,31),
                BaseSalary = 3800,
                Allowances = 500,
                Deductions = 750,
                NetSalary =  3550,
                IsPaid = false
            },
            new Payroll
            {
                PayrollId = "PR-2025-007",
                EmployeeId = "p005",
                EmployeeName = "David Martinez",
                PayPeriodStart = new DateTime(2025,03,01),
                PayPeriodEnd = new DateTime(2025,03,31),
                BaseSalary = 10500000,
                Allowances = 2000000,
                Deductions = 1050000,
                NetSalary = 11450000,
                IsPaid = false
            },
            new Payroll
            {
                PayrollId = "PAY008",
                EmployeeId = "EMP004",
                EmployeeName = "Vũ Tiến Hoàng",
                PayPeriodStart = new DateTime(2025,03,01),
                PayPeriodEnd = new DateTime(2025,03,31),
                BaseSalary = 4800,
                Allowances = 900,
                Deductions = 1050,
                NetSalary = 4650,
                IsPaid = false
            }
        };
            this.payrolls = samplePayrolls;
            SavePayrolls();
        }
    }
}
