using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HRManagementSystem
{
    public partial class PayrollForm : Form
    {
        private readonly PayrollService _payrollService;
        private HRManagementSystem.Payroll _payroll;
        private bool _isEditMode = false;

        public PayrollForm(PayrollService payrollService)
        {
            InitializeComponent();
            _payrollService = payrollService;
            _payroll = new HRManagementSystem.Payroll();

            SetupForm();
        }

        public PayrollForm(PayrollService payrollService, HRManagementSystem.Payroll payroll)
        {
            InitializeComponent();
            _payrollService = payrollService;
            _payroll = payroll;
            _isEditMode = true;

            SetupForm();
            LoadPayrollData();
        }
        private void LoadEmployeeList()
        {
            try
            {

                List<Payroll> allPayrolls = _payrollService.GetAllPayrolls();


                var distinctEmployees = allPayrolls
                    .GroupBy(p => p.EmployeeId)
                    .Select(g => new
                    {
                        Id = g.Key,
                        Name = g.First().EmployeeName
                    })
                    .ToList();


                distinctEmployees.Insert(0, new { Id = "", Name = "-- Choose Employee --" });


                cboEmployee.DisplayMember = "Name";
                cboEmployee.ValueMember = "Id";
                cboEmployee.DataSource = distinctEmployees;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupForm()
        {

            this.Text = _isEditMode ? "Update Payslip" : "Add Payslip";


            LoadEmployeeList();


            dtpPayPeriodStart.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtpPayPeriodEnd.Value = dtpPayPeriodStart.Value.AddMonths(1).AddDays(-1);


            txtBaseSalary.TextChanged += SalaryField_TextChanged;
            txtAllowances.TextChanged += SalaryField_TextChanged;
            txtDeductions.TextChanged += SalaryField_TextChanged;
        }


        private void LoadPayrollData()
        {

            for (int i = 0; i < cboEmployee.Items.Count; i++)
            {
                dynamic item = cboEmployee.Items[i];
                if (item.Id == _payroll.EmployeeId)
                {
                    cboEmployee.SelectedIndex = i;
                    break;
                }
            }

            dtpPayPeriodStart.Value = _payroll.PayPeriodStart;
            dtpPayPeriodEnd.Value = _payroll.PayPeriodEnd;
            txtBaseSalary.Text = _payroll.BaseSalary.ToString("N0");
            txtAllowances.Text = _payroll.Allowances.ToString("N0");
            txtDeductions.Text = _payroll.Deductions.ToString("N0");
            txtNetSalary.Text = _payroll.NetSalary.ToString("N0");
            chkIsPaid.Checked = _payroll.IsPaid;
        }


        private void SalaryField_TextChanged(object sender, EventArgs e)
        {
            CalculateNetSalary();
        }

        private void CalculateNetSalary()
        {
            try
            {
                decimal baseSalary = ParseCurrency(txtBaseSalary.Text);
                decimal allowances = ParseCurrency(txtAllowances.Text);
                decimal deductions = ParseCurrency(txtDeductions.Text);

                decimal netSalary = _payrollService.CalculateNetSalary(baseSalary, allowances, deductions);
                txtNetSalary.Text = netSalary.ToString("N0");
            }
            catch
            {
                txtNetSalary.Text = "0";
            }
        }

        private decimal ParseCurrency(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;


            string numericValue = new string(value.Where(c => char.IsDigit(c) || c == '.').ToArray());

            if (decimal.TryParse(numericValue, out decimal result))
                return result;

            return 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {

                    dynamic selectedEmployee = cboEmployee.SelectedItem;


                    _payroll.EmployeeId = selectedEmployee.Id;
                    _payroll.EmployeeName = selectedEmployee.Name;
                    _payroll.PayPeriodStart = dtpPayPeriodStart.Value;
                    _payroll.PayPeriodEnd = dtpPayPeriodEnd.Value;
                    _payroll.BaseSalary = ParseCurrency(txtBaseSalary.Text);
                    _payroll.Allowances = ParseCurrency(txtAllowances.Text);
                    _payroll.Deductions = ParseCurrency(txtDeductions.Text);
                    _payroll.NetSalary = ParseCurrency(txtNetSalary.Text);
                    _payroll.IsPaid = chkIsPaid.Checked;

                    if (_isEditMode)
                    {
                        _payrollService.UpdatePayroll(_payroll);
                    }
                    else
                    {
                        _payrollService.AddPayroll(_payroll);
                    }

                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu phiếu lương: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private bool ValidateInput()
        {

            if (cboEmployee.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboEmployee.Focus();
                return false;
            }


            if (dtpPayPeriodEnd.Value < dtpPayPeriodStart.Value)
            {
                MessageBox.Show("Ngày kết thúc kỳ lương phải sau ngày bắt đầu", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpPayPeriodEnd.Focus();
                return false;
            }


            if (string.IsNullOrWhiteSpace(txtBaseSalary.Text) || ParseCurrency(txtBaseSalary.Text) <= 0)
            {
                MessageBox.Show("Vui lòng nhập lương cơ bản hợp lệ", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBaseSalary.Focus();
                return false;
            }

            return true;
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dtpPayPeriodStart_ValueChanged(object sender, EventArgs e)
        {

            dtpPayPeriodEnd.Value = new DateTime(
                dtpPayPeriodStart.Value.Year,
                dtpPayPeriodStart.Value.Month,
                DateTime.DaysInMonth(dtpPayPeriodStart.Value.Year, dtpPayPeriodStart.Value.Month)
            );
        }
    }
}
