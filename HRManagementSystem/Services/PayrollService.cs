using System;
using System.Collections.Generic;
using System.Text.Json;
using HRManagementSystem;
namespace HRManagementSystem
{

    public class PayrollService : IService<Payroll>
    {
        private List<Payroll> payrolls;
        private readonly string payrollDataPath = @"C:\Users\ADMIN\source\repos\OOP-4\HRManagementSystem\Data\Payroll.json";
        private readonly FileManager fileManager;
        public PayrollService()
        {
            IFileStorage fileStorage = new JsonFileStorage(); 

            this.fileManager = new FileManager(fileStorage);

            try
            {
                List<Payroll> loadedData = (List<Payroll>)this.fileManager.LoadPayrolls();
                if (loadedData != null)
                {
                    this.payrolls = loadedData;
                }
                else
                {
                    this.payrolls = new List<Payroll>();
                }
            }
            catch (Exception)
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
            this.fileManager.SavePayrolls(this.payrolls);
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
                    this.fileManager.SavePayrolls(this.payrolls);
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
                    this.fileManager.SavePayrolls(this.payrolls);
                    return true;
                }
            }
            throw new InvalidOperationException("Payroll not found");
            return false;
        }
        public int GeneratePayslip(DateTime month)
        {
            int count = 0;
            List<Payroll> uniqueEmployees = new List<Payroll>();
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                Payroll currentPayroll = this.payrolls[i];
                string employeeId = currentPayroll.EmployeeId;
                int existingIndex = -1;
                for (int j = 0; j < uniqueEmployees.Count; j++)
                {
                    if (uniqueEmployees[j].EmployeeId == employeeId)
                    {
                        existingIndex = j;
                        break;
                    }
                }

                if (existingIndex == -1)
                {
                    uniqueEmployees.Add(currentPayroll);
                }
                else
                {
                    if (currentPayroll.PayPeriodEnd > uniqueEmployees[existingIndex].PayPeriodEnd)
                    {
                        uniqueEmployees[existingIndex] = currentPayroll;
                    }
                }
            }

            DateTime startDate = new DateTime(month.Year, month.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            List<Payroll> existingPayrolls = new List<Payroll>();
            for (int i = 0; i < this.payrolls.Count; i++)
            {
                Payroll payroll = this.payrolls[i];
                if (payroll.PayPeriodStart.Year == month.Year &&
                    payroll.PayPeriodStart.Month == month.Month)
                {
                    existingPayrolls.Add(payroll);
                }
            }

            for (int i = 0; i < existingPayrolls.Count; i++)
            {
                this.payrolls.Remove(existingPayrolls[i]);
            }
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
            for (int i = 0; i < uniqueEmployees.Count; i++)
            {
                Payroll lastPayroll = uniqueEmployees[i];
                try
                {
                    maxId++;

                    Payroll newPayroll = new Payroll
                    {
                        PayrollId = "PR-" + DateTime.Now.Year + "-" + (maxId + 1).ToString("D3"),
                        EmployeeId = lastPayroll.EmployeeId,
                        EmployeeName = lastPayroll.EmployeeName,
                        PayPeriodStart = startDate,
                        PayPeriodEnd = endDate,
                        BaseSalary = lastPayroll.BaseSalary,
                        Allowances = lastPayroll.Allowances,
                        Deductions = lastPayroll.Deductions,
                        IsPaid = false
                    };

                    newPayroll.NetSalary = newPayroll.BaseSalary + newPayroll.Allowances - newPayroll.Deductions;
                    this.payrolls.Add(newPayroll);
                    count++;
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần
                    Console.WriteLine("Lỗi khi tạo phiếu lương cho nhân viên " + lastPayroll.EmployeeId + ": " + ex.Message);
                }
            }
            this.fileManager.SavePayrolls(this.payrolls);
            return count;
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