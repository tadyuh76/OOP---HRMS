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

            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }

            PAYROLL_FILE_PATH = Path.Combine(_dataDirectory, "payrolls.json");

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
                PayrollId = "PAY001",
                EmployeeId = "EMP001",
                EmployeeName = "Trần Văn An",
                PayPeriodStart = new DateTime(2025, 1, 1),
                PayPeriodEnd = new DateTime(2025, 1, 31),
                BaseSalary = 10000000,
                Allowances = 2000000,
                Deductions = 1000000,
                NetSalary = 11000000,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PAY002",
                EmployeeId = "EMP002",
                EmployeeName = "Ngô Ngọc Bình",
                PayPeriodStart = new DateTime(2025, 1, 1),
                PayPeriodEnd = new DateTime(2025, 1, 31),
                BaseSalary = 12000000,
                Allowances = 1500000,
                Deductions = 1200000,
                NetSalary = 12300000,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PAY003",
                EmployeeId = "EMP003",
                EmployeeName = "Nguyễn Văn Cường",
                PayPeriodStart = new DateTime(2025, 1, 1),
                PayPeriodEnd = new DateTime(2025, 1, 31),
                BaseSalary = 8000000,
                Allowances = 1000000,
                Deductions = 800000,
                NetSalary = 8200000,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PAY004",
                EmployeeId = "EMP001",
                EmployeeName = "Trần Mai Dung",
                PayPeriodStart = new DateTime(2025, 2, 1),
                PayPeriodEnd = new DateTime(2025, 2, 28),
                BaseSalary = 10000000,
                Allowances = 2000000,
                Deductions = 1000000,
                NetSalary = 11000000,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PAY005",
                EmployeeId = "EMP002",
                EmployeeName = "Nguyễn Thị Én",
                PayPeriodStart = new DateTime(2025, 2, 1),
                PayPeriodEnd = new DateTime(2025, 2, 28),
                BaseSalary = 12000000,
                Allowances = 1500000,
                Deductions = 1200000,
                NetSalary = 12300000,
                IsPaid = true
            },
            new Payroll
            {
                PayrollId = "PAY006",
                EmployeeId = "EMP003",
                EmployeeName = "Nguyễn Văn Phước",
                PayPeriodStart = new DateTime(2025, 2, 1),
                PayPeriodEnd = new DateTime(2025, 2, 28),
                BaseSalary = 8000000,
                Allowances = 1000000,
                Deductions = 800000,
                NetSalary = 8200000,
                IsPaid = false
            },
            new Payroll
            {
                PayrollId = "PAY007",
                EmployeeId = "EMP001",
                EmployeeName = "Ngô Ngọc Giang",
                PayPeriodStart = new DateTime(2025, 3, 1),
                PayPeriodEnd = new DateTime(2025, 3, 31),
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
                PayPeriodStart = new DateTime(2025, 3, 1),
                PayPeriodEnd = new DateTime(2025, 3, 31),
                BaseSalary = 15000000,
                Allowances = 3000000,
                Deductions = 1800000,
                NetSalary = 16200000,
                IsPaid = false
            }
        };
            this.payrolls = samplePayrolls;
            SavePayrolls();
        }
    }
}
