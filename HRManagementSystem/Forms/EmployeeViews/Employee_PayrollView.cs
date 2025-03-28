using System.Data;
using System.Reflection;

namespace HRManagementSystem
{
    public partial class Employee_PayrollView : Form
    {
        // Define delegate types
        public delegate void SearchEventHandler(object? sender, EventArgs e);
        public delegate void EmployeeSelectionChangedEventHandler(object? sender, EventArgs e);

        // Define events using the delegate types
        private event SearchEventHandler SearchRequested;
        private event EmployeeSelectionChangedEventHandler EmployeeSelectionChanged;

        private readonly PayrollService _payrollService;
        private readonly EmployeeService _employeeService;
        private string _selectedEmployeeName;
        private List<Payroll> _currentPayrolls;

        public Employee_PayrollView()
        {
            InitializeComponent();

            _payrollService = new PayrollService();
            _employeeService = new EmployeeService();
            _selectedEmployeeName = string.Empty;
            _currentPayrolls = new List<Payroll>();

            // Initialize and subscribe to events directly in constructor
            SearchEventHandler searchHandler = new SearchEventHandler(BtnSearch_Click);
            EmployeeSelectionChangedEventHandler selectionChangedHandler = new EmployeeSelectionChangedEventHandler(CboEmployee_SelectedIndexChanged);

            // Subscribe to our custom events
            SearchRequested += searchHandler;
            EmployeeSelectionChanged += selectionChangedHandler;

            // Wire up the UI controls to our event handlers
            btnSearch.Click += new EventHandler(SearchRequested);
            cboEmployee.SelectedIndexChanged += new EventHandler(EmployeeSelectionChanged);

            SetupDataGridView();
            LoadEmployeeNames();
        }

        public Employee_PayrollView(PayrollService payrollService)
        {
            InitializeComponent();
            _payrollService = payrollService ?? new PayrollService();
            _employeeService = new EmployeeService();
            _selectedEmployeeName = string.Empty;
            _currentPayrolls = new List<Payroll>();

            // Initialize and subscribe to events directly in constructor
            SearchEventHandler searchHandler = new SearchEventHandler(BtnSearch_Click);
            EmployeeSelectionChangedEventHandler selectionChangedHandler = new EmployeeSelectionChangedEventHandler(CboEmployee_SelectedIndexChanged);

            // Subscribe to our custom events
            SearchRequested += searchHandler;
            EmployeeSelectionChanged += selectionChangedHandler;

            // Wire up the UI controls to our event handlers
            btnSearch.Click += new EventHandler(SearchRequested);
            cboEmployee.SelectedIndexChanged += new EventHandler(EmployeeSelectionChanged);

            SetupDataGridView();
            LoadEmployeeNames();
        }

        // Method to unsubscribe from events (call this in Dispose or when closing the form)
        private void UnsubscribeEvents()
        {
            // Remove event handlers from controls
            btnSearch.Click -= new EventHandler(SearchRequested);
            cboEmployee.SelectedIndexChanged -= new EventHandler(EmployeeSelectionChanged);

            // Clear all event subscriptions
            SearchRequested -= SearchRequested;
            EmployeeSelectionChanged -= EmployeeSelectionChanged;
        }

        private void LoadEmployeeNames()
        {
            try
            {
                // Get payroll data
                List<Payroll> allPayrolls = _payrollService.GetAll();
                if (allPayrolls == null)
                {
                    allPayrolls = new List<Payroll>();
                }

                // Get employee data
                List<Employee> employees = _employeeService.GetAll();
                if (employees == null)
                {
                    employees = new List<Employee>();
                }

                // Create a list of unique employee names from both payrolls and employee list
                List<string> employeeNamesFromPayrolls = new List<string>();
                foreach (Payroll payroll in allPayrolls)
                {
                    if (!string.IsNullOrEmpty(payroll.EmployeeName) &&
                        !employeeNamesFromPayrolls.Contains(payroll.EmployeeName))
                    {
                        employeeNamesFromPayrolls.Add(payroll.EmployeeName);
                    }
                }

                List<string> employeeNamesFromEmployees = new List<string>();
                foreach (Employee employee in employees)
                {
                    if (!string.IsNullOrEmpty(employee.Name) &&
                        !employeeNamesFromEmployees.Contains(employee.Name))
                    {
                        employeeNamesFromEmployees.Add(employee.Name);
                    }
                }

                // Combine both lists and remove duplicates
                List<string> employeeNames = new List<string>();
                foreach (string name in employeeNamesFromPayrolls)
                {
                    if (!employeeNames.Contains(name))
                    {
                        employeeNames.Add(name);
                    }
                }

                foreach (string name in employeeNamesFromEmployees)
                {
                    if (!employeeNames.Contains(name))
                    {
                        employeeNames.Add(name);
                    }
                }

                // Sort the list
                employeeNames.Sort();

                // Clear and populate the combobox
                cboEmployee.Items.Clear();
                cboEmployee.Items.Add("-- Choose Employee --");

                foreach (string name in employeeNames)
                {
                    cboEmployee.Items.Add(name);
                }

                cboEmployee.SelectedIndex = 0;

                // Register event for ComboBox
                if (!IsEventHandlerRegistered(cboEmployee, "SelectedIndexChanged", new EventHandler(CboEmployee_SelectedIndexChanged)))
                {
                    cboEmployee.SelectedIndexChanged += new EventHandler(CboEmployee_SelectedIndexChanged);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Helper method to check if event handler is already registered
        private bool IsEventHandlerRegistered(Control control, string eventName, Delegate eventHandler)
        {
            if (control == null || string.IsNullOrEmpty(eventName) || eventHandler == null)
                return false;

            FieldInfo? field = typeof(Control).GetField(eventName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
                return false;

            object? eventField = field.GetValue(control);
            if (eventField == null)
                return false;

            Delegate[] delegates = ((Delegate)eventField).GetInvocationList();
            return delegates.Contains(eventHandler);
        }

        private void SetupDataGridView()
        {
            dgvPayrolls.AutoGenerateColumns = false;
            dgvPayrolls.Columns.Clear();

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "PayrollId",
                HeaderText = "PayrollId",
                DataPropertyName = "PayrollId",
                Width = 100
            };

            DataGridViewTextBoxColumn employeeIdColumn = new DataGridViewTextBoxColumn
            {
                Name = "EmployeeId",
                HeaderText = "EmployeeId",
                DataPropertyName = "EmployeeId",
                Width = 100
            };

            DataGridViewTextBoxColumn employeeNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "EmployeeName",
                HeaderText = "EmployeeName",
                DataPropertyName = "EmployeeName",
                Width = 150
            };

            DataGridViewTextBoxColumn startDateColumn = new DataGridViewTextBoxColumn
            {
                Name = "PayPeriodStart",
                HeaderText = "PayPeriodStart",
                DataPropertyName = "PayPeriodStart",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            };

            DataGridViewTextBoxColumn endDateColumn = new DataGridViewTextBoxColumn
            {
                Name = "PayPeriodEnd",
                HeaderText = "PayPeriodEnd",
                DataPropertyName = "PayPeriodEnd",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            };

            DataGridViewTextBoxColumn baseSalaryColumn = new DataGridViewTextBoxColumn
            {
                Name = "BaseSalary",
                HeaderText = "BaseSalary",
                DataPropertyName = "BaseSalary",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            };

            DataGridViewTextBoxColumn allowancesColumn = new DataGridViewTextBoxColumn
            {
                Name = "Allowances",
                HeaderText = "Allowances",
                DataPropertyName = "Allowances",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            };

            DataGridViewTextBoxColumn deductionsColumn = new DataGridViewTextBoxColumn
            {
                Name = "Deductions",
                HeaderText = "Deductions",
                DataPropertyName = "Deductions",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            };

            DataGridViewTextBoxColumn netSalaryColumn = new DataGridViewTextBoxColumn
            {
                Name = "NetSalary",
                HeaderText = "NetSalary",
                DataPropertyName = "NetSalary",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            };

            DataGridViewCheckBoxColumn isPaidColumn = new DataGridViewCheckBoxColumn
            {
                Name = "IsPaid",
                HeaderText = "IsPaid",
                DataPropertyName = "IsPaid",
                Width = 80
            };

            dgvPayrolls.Columns.Add(idColumn);
            dgvPayrolls.Columns.Add(employeeIdColumn);
            dgvPayrolls.Columns.Add(employeeNameColumn);
            dgvPayrolls.Columns.Add(startDateColumn);
            dgvPayrolls.Columns.Add(endDateColumn);
            dgvPayrolls.Columns.Add(baseSalaryColumn);
            dgvPayrolls.Columns.Add(allowancesColumn);
            dgvPayrolls.Columns.Add(deductionsColumn);
            dgvPayrolls.Columns.Add(netSalaryColumn);
            dgvPayrolls.Columns.Add(isPaidColumn);
        }

        private void LoadPayrollData()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_selectedEmployeeName) || _selectedEmployeeName == "-- Choose Employee --")
                {
                    dgvPayrolls.DataSource = null;
                    lblTotalPayroll.Text = "Total: 0 $";
                    lblAverageSalary.Text = "Average: 0 $";
                    lblSalaryRange.Text = "Min-Max: 0 - 0 $";
                    MessageBox.Show("Vui lòng chọn nhân viên để xem phiếu lương", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                _currentPayrolls = _payrollService.GetPayrollsByEmployee(_selectedEmployeeName);
                if (_currentPayrolls == null)
                {
                    _currentPayrolls = new List<Payroll>();
                }

                if (_currentPayrolls.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy phiếu lương cho nhân viên: {_selectedEmployeeName}", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPayrolls.DataSource = null;
                    lblTotalPayroll.Text = "Total: 0 $";
                    lblAverageSalary.Text = "Average: 0 $";
                    lblSalaryRange.Text = "Min-Max: 0 - 0 $";
                    return;
                }

                // Use the payroll objects directly instead of creating view models
                dgvPayrolls.DataSource = null;
                dgvPayrolls.DataSource = _currentPayrolls;
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateStatistics()
        {
            if (_currentPayrolls.Count > 0)
            {
                decimal totalSalary = _payrollService.CalculateTotalPayroll(_currentPayrolls);
                decimal avgSalary = _payrollService.CalculateAverageSalary(_currentPayrolls);
                decimal minSalary, maxSalary;
                _payrollService.GetSalaryRange(_currentPayrolls, out minSalary, out maxSalary);

                lblTotalPayroll.Text = $"Total: {totalSalary:N0} $";
                lblAverageSalary.Text = $"Average: {avgSalary:N0} $";
                lblSalaryRange.Text = $"Min-Max: {minSalary:N0} - {maxSalary:N0} $";
            }
            else
            {
                lblTotalPayroll.Text = "Total: 0 $";
                lblAverageSalary.Text = "Average: 0 $";
                lblSalaryRange.Text = "Min-Max: 0 - 0 $";
            }
        }

        private void CboEmployee_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cboEmployee.SelectedIndex > 0 && cboEmployee.SelectedItem != null)
            {
                _selectedEmployeeName = cboEmployee.SelectedItem.ToString() ?? string.Empty;
                Console.WriteLine($"Đã chọn nhân viên: {_selectedEmployeeName}");
            }
            else
            {
                _selectedEmployeeName = string.Empty;
            }
        }

        private void BtnSearch_Click(object? sender, EventArgs e)
        {
            LoadPayrollData();
        }

        private void btnNewPayroll_Click(object? sender, EventArgs e)
        {
            PayrollForm payrollForm = new PayrollForm(_payrollService, _employeeService);
            if (payrollForm.ShowDialog() == DialogResult.OK)
            {
                LoadEmployeeNames();
                LoadPayrollData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPayrolls.SelectedRows.Count > 0 && dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value != null)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(payrollId))
                {
                    MessageBox.Show("Invalid payroll ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Payroll selectedPayroll = _payrollService.GetById(payrollId);

                if (selectedPayroll != null)
                {
                    PayrollForm payrollForm = new PayrollForm(_payrollService, _employeeService, selectedPayroll);
                    if (payrollForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadPayrollData();
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu lương để sửa", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPayrolls.SelectedRows.Count > 0 && dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value != null)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(payrollId))
                {
                    MessageBox.Show("Invalid payroll ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phiếu lương này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _payrollService.Delete(payrollId);
                        LoadPayrollData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa phiếu lương: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu lương để xóa", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvPayrolls.SelectedRows.Count > 0 && dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value != null)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(payrollId))
                {
                    MessageBox.Show("Invalid payroll ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Payroll selectedPayroll = _payrollService.GetById(payrollId);

                if (selectedPayroll != null)
                {
                    MessageBox.Show("Chức năng in phiếu lương đang được phát triển", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu lương để in", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            PayrollReport reportForm = new PayrollReport(_payrollService);
            reportForm.ShowDialog();
        }

        private void PayrollManagement_Load(object sender, EventArgs e)
        {
            LoadEmployeeNames();
        }
    }
}