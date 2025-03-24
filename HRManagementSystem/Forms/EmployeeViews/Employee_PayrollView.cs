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
    public partial class Employee_PayrollView : Form
    {
        private readonly PayrollService _payrollService;
        private string _selectedEmployeeName;
        private List<HRManagementSystem.Payroll> _currentPayrolls;

        public Employee_PayrollView()
        {
            InitializeComponent();
            
            _payrollService = new PayrollService();
            _selectedEmployeeName = string.Empty;
            _currentPayrolls = new List<HRManagementSystem.Payroll>();

            SetupDataGridView();
            LoadEmployeeNames();

            
            btnSearch.Click += BtnSearch_Click;
        }

        public Employee_PayrollView(PayrollService payrollService)
        {
            InitializeComponent();
            _payrollService = payrollService;
            _selectedEmployeeName = string.Empty;
            _currentPayrolls = new List<HRManagementSystem.Payroll>();

            SetupDataGridView();
            LoadEmployeeNames();

            
            btnSearch.Click += BtnSearch_Click;
        }

        private void LoadEmployeeNames()
        {
            try
            {
                
                var allPayrolls = _payrollService.GetAllPayrolls();

               
                var employeeNames = allPayrolls
                    .Select(p => p.EmployeeName)
                    .Where(name => !string.IsNullOrEmpty(name))
                    .Distinct()
                    .ToList();

               
                cboEmployee.Items.Clear();
                cboEmployee.Items.Add("-- Choose Employee --");

                foreach (var name in employeeNames)
                {
                    cboEmployee.Items.Add(name);
                }

                cboEmployee.SelectedIndex = 0;

                // Đăng ký sự kiện cho ComboBox
                cboEmployee.SelectedIndexChanged += CboEmployee_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách nhân viên: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupDataGridView()
        {
            // Giữ nguyên code thiết lập DataGridView
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

            // Thêm các cột vào DataGridView
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

                
                var payrollViewModels = _currentPayrolls.Select(payroll => new
                {
                    PayrollId = payroll.PayrollId,
                    EmployeeId = payroll.EmployeeId,
                    EmployeeName = payroll.EmployeeName ?? "[Không tìm thấy]",
                    PayPeriodStart = payroll.PayPeriodStart,
                    PayPeriodEnd = payroll.PayPeriodEnd,
                    BaseSalary = payroll.BaseSalary,
                    Allowances = payroll.Allowances,
                    Deductions = payroll.Deductions,
                    NetSalary = payroll.NetSalary,
                    IsPaid = payroll.IsPaid
                }).ToList();

                
                dgvPayrolls.DataSource = null;
                dgvPayrolls.DataSource = payrollViewModels;
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

       
        private void CboEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboEmployee.SelectedIndex > 0) 
            {
                _selectedEmployeeName = cboEmployee.SelectedItem.ToString();
                Console.WriteLine($"Đã chọn nhân viên: {_selectedEmployeeName}");
            }
            else
            {
                _selectedEmployeeName = string.Empty;
            }
        }

       
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Đã nhấn nút Xem");
            Console.WriteLine($"Tên nhân viên đã chọn: {_selectedEmployeeName}");
            LoadPayrollData();
        }

       
        private void btnNewPayroll_Click(object sender, EventArgs e)
        {
           
            PayrollForm payrollForm = new PayrollForm(_payrollService);
            if (payrollForm.ShowDialog() == DialogResult.OK)
            {
                LoadEmployeeNames(); 
                LoadPayrollData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPayrolls.SelectedRows.Count > 0)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString();
                HRManagementSystem.Payroll selectedPayroll = _payrollService.GetPayrollById(payrollId);

                if (selectedPayroll != null)
                {
                    PayrollForm payrollForm = new PayrollForm(_payrollService, selectedPayroll);
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
            if (dgvPayrolls.SelectedRows.Count > 0)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString();

                DialogResult result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa phiếu lương này không?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        _payrollService.DeletePayroll(payrollId);
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
            if (dgvPayrolls.SelectedRows.Count > 0)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString();
                HRManagementSystem.Payroll selectedPayroll = _payrollService.GetPayrollById(payrollId);

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
            
            if (_payrollService.GetAllPayrolls().Count == 0)
            {
                _payrollService.CreateSampleData();
                
                LoadEmployeeNames();
            }
        }
    }
}
