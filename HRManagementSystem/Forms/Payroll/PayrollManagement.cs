using System;
using System.Collections;
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
    public partial class PayrollManagement : Form
    {
        private readonly PayrollService _payrollService;
        private DateTime _currentMonth;
        private List<HRManagementSystem.Payroll> _currentPayrolls;

        public PayrollManagement()
        {
            InitializeComponent();
            _payrollService = new PayrollService();
            _currentMonth = DateTime.Now;
            _currentPayrolls = new List<HRManagementSystem.Payroll>();

            SetupDataGridView();
            UpdateMonthDisplay();
            LoadPayrollData();
        }

        public PayrollManagement(PayrollService payrollService)
        {
            InitializeComponent();
            _payrollService = payrollService;
            _currentMonth = DateTime.Now;
            _currentPayrolls = new List<HRManagementSystem.Payroll>();

            SetupDataGridView();
            UpdateMonthDisplay();
            LoadPayrollData();
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

        private void UpdateMonthDisplay()
        {
            lblMonth.Text = _currentMonth.ToString("MM/yyyy");
        }
        
        private void LoadPayrollData()
        {
            try
            {
                _currentPayrolls = _payrollService.GetPayrollsByMonth(_currentMonth);                           
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

        private void btnPreviousMonth_Click(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(-1);
            UpdateMonthDisplay();
            LoadPayrollData();
        }

        private void btnNextMonth_Click(object sender, EventArgs e)
        {
            _currentMonth = _currentMonth.AddMonths(1);
            UpdateMonthDisplay();
            LoadPayrollData();
        }

        private void btnNewPayroll_Click(object sender, EventArgs e)
        {

            PayrollForm payrollForm = new PayrollForm(_payrollService);
            if (payrollForm.ShowDialog() == DialogResult.OK)
            {
                LoadPayrollData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPayrolls.SelectedRows.Count > 0)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString();
                HRManagementSystem.Payroll selectedPayroll = _payrollService.GetById(payrollId);

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
            if (dgvPayrolls.SelectedRows.Count > 0)
            {
                string payrollId = dgvPayrolls.SelectedRows[0].Cells["PayrollId"].Value.ToString();
                HRManagementSystem.Payroll selectedPayroll = _payrollService.GetById(payrollId);

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

        private void btnSearch_Click(object sender, EventArgs e)
        {

            PayrollSearch searchForm = new PayrollSearch(_payrollService);
            searchForm.ShowDialog();
        }

        private void PayrollManagement_Load(object sender, EventArgs e)
        {



            LoadPayrollData();

        }

        private void RunPayrollbtn_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult confirmation = MessageBox.Show(
                    $"Bạn có chắc chắn muốn tạo phiếu lương cho tất cả nhân viên trong tháng {_currentMonth.ToString("MM/yyyy")}?\n\n" +
                    "Lưu ý: Quá trình này có thể ghi đè lên các phiếu lương hiện có trong kỳ này.",
                    "Xác nhận tạo phiếu lương",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirmation == DialogResult.Yes)
                {
                    Cursor = Cursors.WaitCursor;

                    int count = _payrollService.GeneratePayslip(_currentMonth);

                    Cursor = Cursors.Default;
                    MessageBox.Show(
                        $"Đã tạo thành công {count} phiếu lương cho tháng {_currentMonth.ToString("MM/yyyy")}.",
                        "Tạo phiếu lương thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    LoadPayrollData();
                }
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(
                    $"Lỗi khi tạo phiếu lương: {ex.Message}\n\n{ex.StackTrace}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}