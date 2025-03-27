using System.ComponentModel;
using System.Data;

namespace HRManagementSystem
{
    public class DepartmentManagement : Form
    {
        private DataGridView dgvDepartments;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;
        private Panel pnlDetails;

        private TextBox txtDepartmentId;
        private TextBox txtName;
        private TextBox txtDescription;
        private NumericUpDown nudBudget;
        private ComboBox cmbManager;
        private TextBox txtManagerName;
        private Button btnSave;
        private Button btnCancel;

        private DepartmentService _departmentService;
        private List<Department> _departments;
        private bool _isNewDepartment = false;

        public DepartmentManagement()
        {
            InitializeComponent();
            _departmentService = new DepartmentService();
            LoadDepartmentsAsync();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.WhiteSmoke;
            this.Dock = DockStyle.Fill;
            this.Size = new Size(1000, 600);

            // Title
            Label lblTitle = new Label();
            lblTitle.Text = "Department Management";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // Description
            Label lblDescription = new Label();
            lblDescription.Text = "Create and manage departments, assign managers and employees";
            lblDescription.Font = new Font("Segoe UI", 10);
            lblDescription.Location = new Point(20, 60);
            lblDescription.AutoSize = true;
            this.Controls.Add(lblDescription);

            // Search panel
            Panel pnlSearch = new Panel();
            pnlSearch.Location = new Point(20, 100);
            pnlSearch.Size = new Size(960, 40);
            this.Controls.Add(pnlSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(0, 5);
            txtSearch.Size = new Size(250, 30);
            txtSearch.PlaceholderText = "Search departments...";
            pnlSearch.Controls.Add(txtSearch);

            btnSearch = new Button();
            btnSearch.Location = new Point(260, 5);
            btnSearch.Size = new Size(80, 30);
            btnSearch.Text = "Search";
            btnSearch.Click += BtnSearch_Click;
            pnlSearch.Controls.Add(btnSearch);

            btnRefresh = new Button();
            btnRefresh.Location = new Point(350, 5);
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.Text = "Refresh";
            btnRefresh.Click += BtnRefresh_Click;
            pnlSearch.Controls.Add(btnRefresh);

            // Action buttons
            btnAdd = new Button();
            btnAdd.Location = new Point(700, 5);
            btnAdd.Size = new Size(80, 30);
            btnAdd.Text = "Add New";
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.ForeColor = Color.White;
            btnAdd.Click += BtnAdd_Click;
            pnlSearch.Controls.Add(btnAdd);

            btnEdit = new Button();
            btnEdit.Location = new Point(790, 5);
            btnEdit.Size = new Size(80, 30);
            btnEdit.Text = "Edit";
            btnEdit.BackColor = Color.RoyalBlue;
            btnEdit.ForeColor = Color.White;
            btnEdit.Click += BtnEdit_Click;
            pnlSearch.Controls.Add(btnEdit);

            btnDelete = new Button();
            btnDelete.Location = new Point(880, 5);
            btnDelete.Size = new Size(80, 30);
            btnDelete.Text = "Delete";
            btnDelete.BackColor = Color.Crimson;
            btnDelete.ForeColor = Color.White;
            btnDelete.Click += BtnDelete_Click;
            pnlSearch.Controls.Add(btnDelete);

            // DataGridView
            dgvDepartments = new DataGridView();
            dgvDepartments.Location = new Point(20, 150);
            dgvDepartments.Size = new Size(600, 400);
            dgvDepartments.AutoGenerateColumns = false;
            dgvDepartments.AllowUserToAddRows = false;
            dgvDepartments.AllowUserToDeleteRows = false;
            dgvDepartments.ReadOnly = true;
            dgvDepartments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDepartments.MultiSelect = false;
            dgvDepartments.CellDoubleClick += DgvDepartments_CellDoubleClick;
            dgvDepartments.SelectionChanged += DgvDepartments_SelectionChanged;

            // Define columns
            DataGridViewTextBoxColumn colId = new DataGridViewTextBoxColumn();
            colId.DataPropertyName = "DepartmentId";
            colId.HeaderText = "ID";
            colId.Width = 80;
            dgvDepartments.Columns.Add(colId);

            DataGridViewTextBoxColumn colName = new DataGridViewTextBoxColumn();
            colName.DataPropertyName = "Name";
            colName.HeaderText = "Name";
            colName.Width = 150;
            dgvDepartments.Columns.Add(colName);

            DataGridViewTextBoxColumn colDescription = new DataGridViewTextBoxColumn();
            colDescription.DataPropertyName = "Description";
            colDescription.HeaderText = "Description";
            colDescription.Width = 200;
            dgvDepartments.Columns.Add(colDescription);

            DataGridViewTextBoxColumn colBudget = new DataGridViewTextBoxColumn();
            colBudget.DataPropertyName = "Budget";
            colBudget.HeaderText = "Budget";
            colBudget.Width = 100;
            colBudget.DefaultCellStyle.Format = "C2";
            dgvDepartments.Columns.Add(colBudget);

            DataGridViewTextBoxColumn colManagerId = new DataGridViewTextBoxColumn();
            colManagerId.DataPropertyName = "ManagerId";
            colManagerId.HeaderText = "Manager ID";
            colManagerId.Width = 90;
            dgvDepartments.Columns.Add(colManagerId);

            DataGridViewTextBoxColumn colManagerName = new DataGridViewTextBoxColumn();
            colManagerName.DataPropertyName = "ManagerName";
            colManagerName.HeaderText = "Manager";
            colManagerName.Width = 150;
            dgvDepartments.Columns.Add(colManagerName);

            this.Controls.Add(dgvDepartments);

            // Details Panel
            pnlDetails = new Panel();
            pnlDetails.Location = new Point(640, 150);
            pnlDetails.Size = new Size(340, 400);
            pnlDetails.BorderStyle = BorderStyle.FixedSingle;
            pnlDetails.BackColor = Color.WhiteSmoke;
            this.Controls.Add(pnlDetails);

            Label lblDetailsTitle = new Label();
            lblDetailsTitle.Text = "Department Details";
            lblDetailsTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblDetailsTitle.Location = new Point(10, 10);
            lblDetailsTitle.AutoSize = true;
            pnlDetails.Controls.Add(lblDetailsTitle);

            Label lblId = new Label();
            lblId.Text = "Department ID:";
            lblId.Location = new Point(10, 50);
            lblId.AutoSize = true;
            pnlDetails.Controls.Add(lblId);

            txtDepartmentId = new TextBox();
            txtDepartmentId.Location = new Point(130, 50);
            txtDepartmentId.Size = new Size(190, 25);
            txtDepartmentId.ReadOnly = true;
            pnlDetails.Controls.Add(txtDepartmentId);

            Label lblName = new Label();
            lblName.Text = "Name:";
            lblName.Location = new Point(10, 90);
            lblName.AutoSize = true;
            pnlDetails.Controls.Add(lblName);

            txtName = new TextBox();
            txtName.Location = new Point(130, 90);
            txtName.Size = new Size(190, 25);
            pnlDetails.Controls.Add(txtName);

            Label lblDescr = new Label();
            lblDescr.Text = "Description:";
            lblDescr.Location = new Point(10, 130);
            lblDescr.AutoSize = true;
            pnlDetails.Controls.Add(lblDescr);

            txtDescription = new TextBox();
            txtDescription.Location = new Point(130, 130);
            txtDescription.Size = new Size(190, 25);
            txtDescription.Multiline = true;
            txtDescription.Height = 60;
            pnlDetails.Controls.Add(txtDescription);

            Label lblBudget = new Label();
            lblBudget.Text = "Budget:";
            lblBudget.Location = new Point(10, 210);
            lblBudget.AutoSize = true;
            pnlDetails.Controls.Add(lblBudget);

            nudBudget = new NumericUpDown();
            nudBudget.Location = new Point(130, 210);
            nudBudget.Size = new Size(190, 25);
            nudBudget.Maximum = 10000000;
            nudBudget.Increment = 10000;
            nudBudget.ThousandsSeparator = true;
            nudBudget.DecimalPlaces = 2;
            pnlDetails.Controls.Add(nudBudget);

            Label lblManager = new Label();
            lblManager.Text = "Manager ID:";
            lblManager.Location = new Point(10, 250);
            lblManager.AutoSize = true;
            pnlDetails.Controls.Add(lblManager);

            cmbManager = new ComboBox();
            cmbManager.Location = new Point(130, 250);
            cmbManager.Size = new Size(190, 25);
            cmbManager.Items.AddRange(new object[] { "EMP001", "EMP002", "EMP003", "EMP005", "EMP007", "EMP010", "EMP011" });
            cmbManager.SelectedIndexChanged += CmbManager_SelectedIndexChanged;
            pnlDetails.Controls.Add(cmbManager);

            Label lblManagerName = new Label();
            lblManagerName.Text = "Manager Name:";
            lblManagerName.Location = new Point(10, 290);
            lblManagerName.AutoSize = true;
            pnlDetails.Controls.Add(lblManagerName);

            txtManagerName = new TextBox();
            txtManagerName.Location = new Point(130, 290);
            txtManagerName.Size = new Size(190, 25);
            txtManagerName.ReadOnly = true;
            pnlDetails.Controls.Add(txtManagerName);

            btnSave = new Button();
            btnSave.Location = new Point(130, 330);
            btnSave.Size = new Size(90, 30);
            btnSave.Text = "Save";
            btnSave.BackColor = Color.ForestGreen;
            btnSave.ForeColor = Color.White;
            btnSave.Click += BtnSave_Click;
            pnlDetails.Controls.Add(btnSave);

            btnCancel = new Button();
            btnCancel.Location = new Point(230, 330);
            btnCancel.Size = new Size(90, 30);
            btnCancel.Text = "Cancel";
            btnCancel.BackColor = Color.Gray;
            btnCancel.ForeColor = Color.White;
            btnCancel.Click += BtnCancel_Click;
            pnlDetails.Controls.Add(btnCancel);

            // Initially disable fields
            SetDetailsEnabled(false);
        }

        private void SetDetailsEnabled(bool enabled)
        {
            txtName.Enabled = enabled;
            txtDescription.Enabled = enabled;
            nudBudget.Enabled = enabled;
            cmbManager.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            // txtManagerName is always read-only
        }

        private void ClearDetails()
        {
            txtDepartmentId.Text = string.Empty;
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            nudBudget.Value = 0;
            cmbManager.SelectedIndex = -1;
            txtManagerName.Text = string.Empty;
        }

        private async void LoadDepartmentsAsync()
        {
            try
            {
                _departments = await _departmentService.GetAllDepartmentsAsync();
                dgvDepartments.DataSource = null;
                dgvDepartments.DataSource = new BindingList<Department>(_departments);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading departments: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayDepartmentDetails(Department department)
        {
            if (department == null)
            {
                ClearDetails();
                return;
            }

            txtDepartmentId.Text = department.DepartmentId;
            txtName.Text = department.Name;
            txtDescription.Text = department.Description;
            nudBudget.Value = department.Budget;
            cmbManager.Text = department.ManagerId;
            txtManagerName.Text = department.ManagerName;
        }

        private Department GetDepartmentFromForm()
        {
            return new Department(
                txtDepartmentId.Text,
                txtName.Text,
                txtDescription.Text,
                nudBudget.Value,
                cmbManager.Text,
                txtManagerName.Text
            );
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                dgvDepartments.DataSource = new BindingList<Department>(_departments);
                return;
            }

            string searchTerm = txtSearch.Text.ToLower();
            var filteredList = _departments.Where(d =>
                d.DepartmentId.ToLower().Contains(searchTerm) ||
                d.Name.ToLower().Contains(searchTerm) ||
                d.Description.ToLower().Contains(searchTerm) ||
                d.ManagerId.ToLower().Contains(searchTerm) ||
                d.ManagerName.ToLower().Contains(searchTerm)
            ).ToList();

            dgvDepartments.DataSource = new BindingList<Department>(filteredList);
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadDepartmentsAsync();
        }

        private async void BtnAdd_Click(object sender, EventArgs e)
        {
            _isNewDepartment = true;
            ClearDetails();
            txtDepartmentId.Text = await _departmentService.GenerateNewDepartmentId();
            SetDetailsEnabled(true);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a department to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _isNewDepartment = false;
            SetDetailsEnabled(true);
        }

        private async void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a department to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var department = (Department)dgvDepartments.SelectedRows[0].DataBoundItem;
            var result = MessageBox.Show($"Are you sure you want to delete department {department.Name}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    bool success = await _departmentService.DeleteDepartmentAsync(department.DepartmentId);
                    if (success)
                    {
                        MessageBox.Show("Department deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadDepartmentsAsync();
                        ClearDetails();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Department name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(cmbManager.Text))
            {
                MessageBox.Show("Manager ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Department department = GetDepartmentFromForm();
                bool success;

                if (_isNewDepartment)
                {
                    success = await _departmentService.AddDepartmentAsync(department);
                    if (success)
                    {
                        MessageBox.Show("Department added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add department. ID may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    success = await _departmentService.UpdateDepartmentAsync(department);
                    if (success)
                    {
                        MessageBox.Show("Department updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update department.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                LoadDepartmentsAsync();
                SetDetailsEnabled(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (_isNewDepartment)
            {
                ClearDetails();
            }
            else if (dgvDepartments.SelectedRows.Count > 0)
            {
                DisplayDepartmentDetails((Department)dgvDepartments.SelectedRows[0].DataBoundItem);
            }

            SetDetailsEnabled(false);
        }

        private void DgvDepartments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                _isNewDepartment = false;
                SetDetailsEnabled(true);
            }
        }

        private void DgvDepartments_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDepartments.SelectedRows.Count > 0)
            {
                DisplayDepartmentDetails((Department)dgvDepartments.SelectedRows[0].DataBoundItem);
            }
        }

        private void CmbManager_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbManager.SelectedIndex != -1)
            {
                // Get the corresponding manager name from employees
                string managerId = cmbManager.SelectedItem.ToString();
                // This is simplified - in a real implementation you'd fetch from an employee service
                string managerName = GetManagerNameById(managerId);
                txtManagerName.Text = managerName;
            }
        }

        // Helper method to get manager name by ID
        private string GetManagerNameById(string managerId)
        {
            // This is a simplified implementation
            // In a real application, you would query the EmployeeService
            Dictionary<string, string> managerMap = new Dictionary<string, string>
            {
                { "EMP001", "John Smith" },
                { "EMP002", "Emily Johnson" },
                { "EMP003", "Michael Brown" },
                { "EMP005", "James Wilson" },
                { "EMP007", "David Martinez" },
                { "EMP010", "Lisa Anderson" },
                { "EMP011", "Thomas Clark" }
            };

            if (managerMap.ContainsKey(managerId))
            {
                return managerMap[managerId];
            }

            return string.Empty;
        }
    }
}
