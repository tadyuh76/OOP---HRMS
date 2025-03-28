namespace HRManagementSystem
{
    public partial class EditEmployeeForm : Form
    {
        private Employee _employee;
        private List<Department> _departments;
        private bool _isNewEmployee;

        // Property to access the updated employee after the form closes
        public Employee UpdatedEmployee => _employee;

        public EditEmployeeForm(Employee employee, List<Department> departments, bool isNewEmployee = false)
        {
            _employee = employee ?? throw new ArgumentNullException(nameof(employee));
            _departments = departments ?? throw new ArgumentNullException(nameof(departments));
            _isNewEmployee = isNewEmployee;

            InitializeComponent();
            LoadEmployeeData();
        }

        private void InitializeComponent()
        {
            Text = _isNewEmployee ? "Add New Employee" : "Edit Employee";
            Size = new Size(600, 700);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            Padding = new Padding(20);

            // Main container - Tab Control
            TabControl tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Padding = new Point(10),
                Margin = new Padding(10)
            };

            // Personal Information Tab
            TabPage tabPersonal = new TabPage
            {
                Text = "Personal Information",
                Padding = new Padding(10)
            };

            // Employment Tab
            TabPage tabEmployment = new TabPage
            {
                Text = "Employment Details",
                Padding = new Padding(10)
            };

            tabControl.TabPages.Add(tabPersonal);
            tabControl.TabPages.Add(tabEmployment);
            Controls.Add(tabControl);

            // Personal Information Panel
            TableLayoutPanel personalPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(5),
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 30),
                    new ColumnStyle(SizeType.Percent, 70)
                }
            };

            // Set row styles for personal panel
            for (int i = 0; i < 7; i++)
            {
                personalPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            tabPersonal.Controls.Add(personalPanel);

            // Employee ID field
            Label lblEmployeeId = new Label
            {
                Text = "Employee ID:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            personalPanel.Controls.Add(lblEmployeeId, 0, 0);

            TextBox txtEmployeeId = new TextBox
            {
                Name = "txtEmployeeId",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 10),
                ReadOnly = true
            };
            personalPanel.Controls.Add(txtEmployeeId, 1, 0);

            // Name field
            Label lblName = new Label
            {
                Text = "Full Name:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            personalPanel.Controls.Add(lblName, 0, 1);

            TextBox txtName = new TextBox
            {
                Name = "txtName",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 10)
            };
            personalPanel.Controls.Add(txtName, 1, 1);

            // Email field
            Label lblEmail = new Label
            {
                Text = "Email:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            personalPanel.Controls.Add(lblEmail, 0, 2);

            TextBox txtEmail = new TextBox
            {
                Name = "txtEmail",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 10)
            };
            personalPanel.Controls.Add(txtEmail, 1, 2);

            // Phone field
            Label lblPhone = new Label
            {
                Text = "Phone:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            personalPanel.Controls.Add(lblPhone, 0, 3);

            TextBox txtPhone = new TextBox
            {
                Name = "txtPhone",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 10)
            };
            personalPanel.Controls.Add(txtPhone, 1, 3);

            // Date of Birth field
            Label lblDob = new Label
            {
                Text = "Date of Birth:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            personalPanel.Controls.Add(lblDob, 0, 4);

            DateTimePicker dtpDob = new DateTimePicker
            {
                Name = "dtpDob",
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Margin = new Padding(0, 10, 0, 10)
            };
            personalPanel.Controls.Add(dtpDob, 1, 4);

            // Address field
            Label lblAddress = new Label
            {
                Text = "Address:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            personalPanel.Controls.Add(lblAddress, 0, 5);

            TextBox txtAddress = new TextBox
            {
                Name = "txtAddress",
                Dock = DockStyle.Fill,
                Multiline = true,
                Height = 80,
                Margin = new Padding(0, 10, 0, 10)
            };
            personalPanel.Controls.Add(txtAddress, 1, 5);

            // Employment Panel
            TableLayoutPanel employmentPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 7,
                Padding = new Padding(5),
                ColumnStyles = {
                    new ColumnStyle(SizeType.Percent, 30),
                    new ColumnStyle(SizeType.Percent, 70)
                }
            };

            // Set row styles for employment panel
            for (int i = 0; i < 7; i++)
            {
                employmentPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            tabEmployment.Controls.Add(employmentPanel);

            // Department field
            Label lblDepartment = new Label
            {
                Text = "Department:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            employmentPanel.Controls.Add(lblDepartment, 0, 0);

            ComboBox cmbDepartment = new ComboBox
            {
                Name = "cmbDepartment",
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 10, 0, 10)
            };
            employmentPanel.Controls.Add(cmbDepartment, 1, 0);

            // Position field
            Label lblPosition = new Label
            {
                Text = "Position:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            employmentPanel.Controls.Add(lblPosition, 0, 1);

            TextBox txtPosition = new TextBox
            {
                Name = "txtPosition",
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 10, 0, 10)
            };
            employmentPanel.Controls.Add(txtPosition, 1, 1);

            // Hire Date field
            Label lblHireDate = new Label
            {
                Text = "Hire Date:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            employmentPanel.Controls.Add(lblHireDate, 0, 2);

            DateTimePicker dtpHireDate = new DateTimePicker
            {
                Name = "dtpHireDate",
                Dock = DockStyle.Fill,
                Format = DateTimePickerFormat.Short,
                Margin = new Padding(0, 10, 0, 10)
            };
            employmentPanel.Controls.Add(dtpHireDate, 1, 2);

            // Status field
            Label lblStatus = new Label
            {
                Text = "Status:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            employmentPanel.Controls.Add(lblStatus, 0, 3);

            ComboBox cmbStatus = new ComboBox
            {
                Name = "cmbStatus",
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(0, 10, 0, 10)
            };
            // Add status options
            cmbStatus.Items.AddRange(Enum.GetNames(typeof(EmployeeStatus)));
            employmentPanel.Controls.Add(cmbStatus, 1, 3);

            // Base Salary field
            Label lblSalary = new Label
            {
                Text = "Base Salary:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            employmentPanel.Controls.Add(lblSalary, 0, 4);

            NumericUpDown nudSalary = new NumericUpDown
            {
                Name = "nudSalary",
                Dock = DockStyle.Fill,
                Maximum = 1000000,
                Increment = 500,
                ThousandsSeparator = true,
                DecimalPlaces = 2,
                Margin = new Padding(0, 10, 0, 10)
            };
            employmentPanel.Controls.Add(nudSalary, 1, 4);

            // Bottom button panel
            Panel buttonPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                Padding = new Padding(10)
            };
            Controls.Add(buttonPanel);

            // Save button
            Button btnSave = new Button
            {
                Text = "Save",
                Size = new Size(100, 40),
                Location = new Point(buttonPanel.Width - 230, 10),
                BackColor = Color.FromArgb(68, 93, 233),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;
            buttonPanel.Controls.Add(btnSave);

            // Cancel button
            Button btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(100, 40),
                Location = new Point(buttonPanel.Width - 120, 10),
                BackColor = Color.FromArgb(160, 160, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += BtnCancel_Click;
            buttonPanel.Controls.Add(btnCancel);

            // Fill department dropdown
            foreach (Department department in _departments)
            {
                cmbDepartment.Items.Add(new DepartmentItem(department));
            }

            // Set Anchor for buttons so they stay in position when form resizes
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Adjust tab control to make room for buttons
            tabControl.Height = ClientSize.Height - buttonPanel.Height;
        }

        private void LoadEmployeeData()
        {
            // Load data into form controls
            if (_employee != null)
            {
                TextBox txtEmployeeId = Controls.Find("txtEmployeeId", true).FirstOrDefault() as TextBox;
                TextBox txtName = Controls.Find("txtName", true).FirstOrDefault() as TextBox;
                TextBox txtEmail = Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
                TextBox txtPhone = Controls.Find("txtPhone", true).FirstOrDefault() as TextBox;
                DateTimePicker dtpDob = Controls.Find("dtpDob", true).FirstOrDefault() as DateTimePicker;
                TextBox txtAddress = Controls.Find("txtAddress", true).FirstOrDefault() as TextBox;
                ComboBox cmbDepartment = Controls.Find("cmbDepartment", true).FirstOrDefault() as ComboBox;
                TextBox txtPosition = Controls.Find("txtPosition", true).FirstOrDefault() as TextBox;
                DateTimePicker dtpHireDate = Controls.Find("dtpHireDate", true).FirstOrDefault() as DateTimePicker;
                ComboBox cmbStatus = Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
                NumericUpDown nudSalary = Controls.Find("nudSalary", true).FirstOrDefault() as NumericUpDown;

                if (txtEmployeeId != null) txtEmployeeId.Text = _employee.EmployeeId;
                if (txtName != null) txtName.Text = _employee.Name;
                if (txtEmail != null) txtEmail.Text = _employee.Email;
                if (txtPhone != null) txtPhone.Text = _employee.Phone;
                if (dtpDob != null && _employee.DateOfBirth != DateTime.MinValue) dtpDob.Value = _employee.DateOfBirth;
                if (txtAddress != null) txtAddress.Text = _employee.Address;
                if (txtPosition != null) txtPosition.Text = _employee.Position;
                if (dtpHireDate != null && _employee.HireDate != DateTime.MinValue) dtpHireDate.Value = _employee.HireDate;
                if (cmbStatus != null) cmbStatus.SelectedItem = _employee.Status.ToString();
                if (nudSalary != null) nudSalary.Value = _employee.BaseSalary;

                // Select the correct department
                if (cmbDepartment != null && !string.IsNullOrEmpty(_employee.DepartmentId))
                {
                    for (int i = 0; i < cmbDepartment.Items.Count; i++)
                    {
                        if (cmbDepartment.Items[i] is DepartmentItem deptItem &&
                            deptItem.Department.DepartmentId == _employee.DepartmentId)
                        {
                            cmbDepartment.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (!ValidateForm())
            {
                return;
            }

            // Get values from form controls
            TextBox txtName = Controls.Find("txtName", true).FirstOrDefault() as TextBox;
            TextBox txtEmail = Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
            TextBox txtPhone = Controls.Find("txtPhone", true).FirstOrDefault() as TextBox;
            DateTimePicker dtpDob = Controls.Find("dtpDob", true).FirstOrDefault() as DateTimePicker;
            TextBox txtAddress = Controls.Find("txtAddress", true).FirstOrDefault() as TextBox;
            ComboBox cmbDepartment = Controls.Find("cmbDepartment", true).FirstOrDefault() as ComboBox;
            TextBox txtPosition = Controls.Find("txtPosition", true).FirstOrDefault() as TextBox;
            DateTimePicker dtpHireDate = Controls.Find("dtpHireDate", true).FirstOrDefault() as DateTimePicker;
            ComboBox cmbStatus = Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;
            NumericUpDown nudSalary = Controls.Find("nudSalary", true).FirstOrDefault() as NumericUpDown;

            // Update employee object
            _employee.Name = txtName?.Text;
            _employee.Email = txtEmail?.Text;
            _employee.Phone = txtPhone?.Text;
            _employee.DateOfBirth = dtpDob?.Value ?? DateTime.Now;
            _employee.Address = txtAddress?.Text;
            _employee.Position = txtPosition?.Text;
            _employee.HireDate = dtpHireDate?.Value ?? DateTime.Now;

            // Parse and set status enum
            if (cmbStatus?.SelectedItem != null && Enum.TryParse(cmbStatus.SelectedItem.ToString(), out EmployeeStatus status))
            {
                _employee.Status = status;
            }

            _employee.BaseSalary = nudSalary?.Value ?? 0;

            // Set department ID and name
            if (cmbDepartment?.SelectedItem is DepartmentItem selectedDept)
            {
                _employee.DepartmentId = selectedDept.Department.DepartmentId;
                _employee.DepartmentName = selectedDept.Department.Name;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateForm()
        {
            TextBox txtName = Controls.Find("txtName", true).FirstOrDefault() as TextBox;
            TextBox txtEmail = Controls.Find("txtEmail", true).FirstOrDefault() as TextBox;
            ComboBox cmbDepartment = Controls.Find("cmbDepartment", true).FirstOrDefault() as ComboBox;
            TextBox txtPosition = Controls.Find("txtPosition", true).FirstOrDefault() as TextBox;
            ComboBox cmbStatus = Controls.Find("cmbStatus", true).FirstOrDefault() as ComboBox;

            // Basic validation
            if (string.IsNullOrWhiteSpace(txtName?.Text))
            {
                MessageBox.Show("Employee name is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName?.Focus();
                return false;
            }

            // Email validation (basic format check)
            if (!string.IsNullOrWhiteSpace(txtEmail?.Text) &&
                !txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail?.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPosition?.Text))
            {
                MessageBox.Show("Position is required.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPosition?.Focus();
                return false;
            }

            if (cmbDepartment?.SelectedItem == null)
            {
                MessageBox.Show("Please select a department.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbDepartment?.Focus();
                return false;
            }

            if (cmbStatus?.SelectedItem == null)
            {
                MessageBox.Show("Please select an employee status.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbStatus?.Focus();
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        // Helper class for department dropdown
        private class DepartmentItem
        {
            public Department Department { get; }

            public DepartmentItem(Department department)
            {
                Department = department;
            }

            public override string ToString()
            {
                return $"{Department.DepartmentId} - {Department.Name}";
            }
        }
    }
}
