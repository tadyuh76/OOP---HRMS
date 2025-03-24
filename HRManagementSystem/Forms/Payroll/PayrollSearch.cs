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
    public partial class PayrollSearch : Form
    {
        private readonly PayrollService _payrollService;
        private List<Payroll> _payrolls;

        public PayrollSearch(PayrollService payrollService)
        {
            InitializeComponent();
            _payrollService = payrollService ?? throw new ArgumentNullException(nameof(payrollService));
            _payrolls = new List<Payroll>();

            this.Text = "PAYROLL REPORT";
            SetupDataGridView();
            LoadPayrollData();
        }

        private void SetupDataGridView()
        {
            
            dgvPayrolls.AutoGenerateColumns = false;
            dgvPayrolls.Columns.Clear();

           
            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PayrollId",
                HeaderText = "PayrollId",
                DataPropertyName = "PayrollId",
                Width = 100
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "EmployeeName",
                HeaderText = "EmployeeName",
                DataPropertyName = "EmployeeName",
                Width = 150
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BaseSalary",
                HeaderText = "BaseSalary",
                DataPropertyName = "BaseSalary",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Allowances",
                HeaderText = "Allowances",
                DataPropertyName = "Allowances",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    NullValue = 0
                }
            });

           

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Deductions",
                HeaderText = "Deductions",
                DataPropertyName = "Deductions",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    NullValue = 0
                }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NetSalary",
                HeaderText = "NetSalary",
                DataPropertyName = "NetSalary",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "N0",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            

   
            dgvPayrolls.AllowUserToAddRows = false;
            dgvPayrolls.AllowUserToDeleteRows = false;
            dgvPayrolls.ReadOnly = true;
            dgvPayrolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void LoadPayrollData()
        {
            try
            {
            
                _payrolls = _payrollService.GetAllPayrolls();
                foreach(var payroll in _payrolls)
                {
              
                    if (payroll.Allowances == null) payroll.Allowances = 0;
                    if (payroll.Deductions == null) payroll.Deductions = 0;
                }
                dgvPayrolls.DataSource = _payrolls;

           
                UpdateSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateSummary()
        {
            if (_payrolls != null && _payrolls.Count > 0)
            {
                lblTotalRecords.Text = $"Total of record: {_payrolls.Count}";
                decimal totalAmount = _payrolls.Sum(p => p.NetSalary);
                decimal averageSalary = _payrolls.Average(p => p.NetSalary);

                lblTotalAmount.Text = $"Total : {totalAmount:N0} VNĐ";
                lblAverageSalary.Text = $"Average: {averageSalary:N0} VNĐ";
            }
            else
            {
                lblTotalRecords.Text = "Total of record: 0";
                lblTotalAmount.Text = "Total: 0 VNĐ";
                lblAverageSalary.Text = "Average: 0 VNĐ";
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (_payrolls == null || _payrolls.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất báo cáo!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx|CSV Files (*.csv)|*.csv",
                Title = "Xuất báo cáo"
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                 

                    MessageBox.Show("Xuất báo cáo thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvPayrolls_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _payrolls.Count)
            {
                var selectedPayroll = _payrolls[e.RowIndex];
               
            }
        }
    }
}
