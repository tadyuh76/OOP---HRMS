using System.Data;

namespace HRManagementSystem
{
    public partial class PayrollForm : Form
    {
        private readonly PayrollService _payrollService;
        private readonly EmployeeService _employeeService;
        private Payroll _payroll;
        private bool _isEditMode = false;
        // Add a field to store the original employee name for validation
        private string _originalEmployeeName;

        public PayrollForm(PayrollService payrollService, EmployeeService employeeService)
        {
            InitializeComponent();
            _payrollService = payrollService;
            _employeeService = employeeService;
            _payroll = new Payroll();

            SetupForm();
        }

        public PayrollForm(PayrollService payrollService, EmployeeService employeeService, Payroll payroll)
        {
            InitializeComponent();
            _payrollService = payrollService;
            _employeeService = employeeService;
            _payroll = payroll;
            _isEditMode = true;

            SetupForm();
            LoadPayrollData();
        }

        private void LoadEmployeeList()
        {
            try
            {
                if (_employeeService == null)
                {
                    throw new InvalidOperationException("Employee service is not initialized.");
                }

                // Get employees from EmployeeService instead of payroll records
                List<Employee> employees = _employeeService.GetAll() ?? new List<Employee>();

                // Create a list for the combo box, with null checks
                var employeeList = employees
                    .Where(e => e != null) // Filter out any null employees
                    .Select(e => new
                    {
                        Id = e.Id ?? string.Empty,
                        EmployeeId = e.EmployeeId ?? string.Empty,
                        Name = e.Name ?? "[No Name]"
                    })
                    .ToList();

                // Add default option
                employeeList.Insert(0, new { Id = "", EmployeeId = "", Name = "-- Choose Employee --" });

                // Bind to combo box
                cboEmployee.DisplayMember = "Name";
                cboEmployee.ValueMember = "Id";
                cboEmployee.DataSource = employeeList;

                // Add event handler for employee selection - only add if not already added
                cboEmployee.SelectedIndexChanged -= CboEmployee_SelectedIndexChanged; // Remove first to avoid duplicates
                cboEmployee.SelectedIndexChanged += CboEmployee_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CboEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboEmployee.SelectedIndex > 0 && cboEmployee.SelectedItem != null)
            {
                try
                {
                    dynamic selectedEmployee = cboEmployee.SelectedItem;
                    string id = selectedEmployee.Id;

                    if (string.IsNullOrEmpty(id))
                    {
                        throw new InvalidOperationException("Selected employee has no ID");
                    }

                    // Find the employee with matching Id
                    Employee employee = _employeeService.GetById(id);

                    if (employee != null)
                    {
                        // Store the employee name - should not be editable
                        _originalEmployeeName = employee.Name;

                        // Prefill base salary from employee data
                        txtBaseSalary.Text = employee.BaseSalary.ToString("N0");
                        // Recalculate net salary based on the new base salary
                        CalculateNetSalary();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Employee with ID {id} not found");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading employee data: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SetupForm()
        {
            this.Text = _isEditMode ? "Update Payslip" : "Add Payslip";

            LoadEmployeeList();

            // Disable employee selection when in edit mode
            if (_isEditMode)
            {
                cboEmployee.Enabled = false;
            }

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
                if (item.EmployeeId == _payroll.EmployeeId)  // Match by EmployeeId
                {
                    cboEmployee.SelectedIndex = i;
                    // Store the original employee name
                    _originalEmployeeName = _payroll.EmployeeName;
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

                    _payroll.EmployeeId = selectedEmployee.EmployeeId;  // Use EmployeeId for payroll

                    // Always use the original employee name, never allow editing
                    if (_isEditMode && !string.IsNullOrEmpty(_payroll.EmployeeName))
                    {
                        // In edit mode, preserve the existing employee name
                        // No change needed to _payroll.EmployeeName
                    }
                    else
                    {
                        // In add mode, use the name from the selected employee
                        _payroll.EmployeeName = selectedEmployee.Name;
                    }

                    _payroll.PayPeriodStart = dtpPayPeriodStart.Value;
                    _payroll.PayPeriodEnd = dtpPayPeriodEnd.Value;
                    _payroll.BaseSalary = ParseCurrency(txtBaseSalary.Text);
                    _payroll.Allowances = ParseCurrency(txtAllowances.Text);
                    _payroll.Deductions = ParseCurrency(txtDeductions.Text);
                    _payroll.NetSalary = ParseCurrency(txtNetSalary.Text);
                    _payroll.IsPaid = chkIsPaid.Checked;

                    if (_isEditMode)
                    {
                        _payrollService.Update(_payroll);
                    }
                    else
                    {
                        _payrollService.Add(_payroll);
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