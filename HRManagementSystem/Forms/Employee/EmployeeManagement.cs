using System.Data;

namespace HRManagementSystem
{
    public partial class EmployeeManagement : Form
    {
        private List<Employee> employees;
        private List<Department> departments;

        public EmployeeManagement()
        {
            InitializeComponent();
            LoadSampleData();
            PopulateEmployeeDataGridView();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee Management";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // Main layout panel
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 2,
                ColumnCount = 1
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(mainLayout);

            // Header panel with title and add button
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60
            };
            mainLayout.Controls.Add(headerPanel, 0, 0);

            // Title label
            Label lblTitle = new Label
            {
                Text = "Employee Management",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(0, 10)
            };
            headerPanel.Controls.Add(lblTitle);

            // Add New Employee button
            Button btnAddEmployee = new Button
            {
                Text = "+ Add New Employee",
                BackColor = Color.FromArgb(68, 93, 233), // Blue color from the image
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(200, 40),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                Location = new Point(headerPanel.Width - 220, 10),
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnAddEmployee.FlatAppearance.BorderSize = 0;
            btnAddEmployee.Click += BtnAddEmployee_Click;
            headerPanel.Controls.Add(btnAddEmployee);

            // Directory panel (second row)
            Panel directoryPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle
            };
            mainLayout.Controls.Add(directoryPanel, 0, 1);

            // Directory layout
            TableLayoutPanel directoryLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 3,
                ColumnCount = 1
            };
            directoryLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            directoryLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            directoryLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            directoryPanel.Controls.Add(directoryLayout);

            // Directory header
            Label lblDirectory = new Label
            {
                Text = "Employee Directory",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = true,
                Margin = new Padding(0, 0, 0, 15)
            };
            directoryLayout.Controls.Add(lblDirectory, 0, 0);

            // Search and filter panel
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60
            };
            directoryLayout.Controls.Add(searchPanel, 0, 1);

            // Search box
            TextBox txtSearch = new TextBox
            {
                Size = new Size(500, 30),
                Location = new Point(0, 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };
            txtSearch.SetBounds(txtSearch.Left, txtSearch.Top, txtSearch.Width, 35);
            searchPanel.Controls.Add(txtSearch);

            // Search icon (can be improved with an actual icon)
            Label lblSearchIcon = new Label
            {
                Text = "🔍",
                AutoSize = true,
                Location = new Point(10, 17)
            };
            txtSearch.Controls.Add(lblSearchIcon);
            txtSearch.Padding = new Padding(25, 5, 5, 5);

            // Filter buttons panel
            Panel filterPanel = new Panel
            {
                Location = new Point(txtSearch.Right + 20, 10),
                Height = 35,
                Width = 300,
                BorderStyle = BorderStyle.FixedSingle
            };
            searchPanel.Controls.Add(filterPanel);

            // Filter buttons
            string[] filterOptions = { "All", "Active", "On Leave" };
            int buttonWidth = filterPanel.Width / filterOptions.Length;

            for (int i = 0; i < filterOptions.Length; i++)
            {
                Button btnFilter = new Button
                {
                    Text = filterOptions[i],
                    FlatStyle = FlatStyle.Flat,
                    Size = new Size(buttonWidth, filterPanel.Height - 2),
                    Location = new Point(i * buttonWidth, 0),
                    BackColor = i == 0 ? Color.White : Color.FromArgb(245, 245, 245),
                    ForeColor = i == 0 ? Color.FromArgb(68, 93, 233) : Color.FromArgb(100, 100, 100)
                };
                btnFilter.FlatAppearance.BorderSize = 0;
                btnFilter.Tag = filterOptions[i];
                btnFilter.Click += BtnFilter_Click;
                filterPanel.Controls.Add(btnFilter);
            }

            // DataGridView for employees
            DataGridView dgvEmployees = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 230),
                Name = "dgvEmployees"
            };

            // DataGridView style
            dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvEmployees.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 100, 100);
            dgvEmployees.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            dgvEmployees.ColumnHeadersDefaultCellStyle.Padding = new Padding(10);
            dgvEmployees.ColumnHeadersHeight = 50;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvEmployees.RowTemplate.Height = 50;
            dgvEmployees.DefaultCellStyle.Padding = new Padding(10);
            dgvEmployees.DefaultCellStyle.SelectionBackColor = Color.FromArgb(245, 245, 245);
            dgvEmployees.DefaultCellStyle.SelectionForeColor = Color.Black;

            directoryLayout.Controls.Add(dgvEmployees, 0, 2);

            // Set up columns for the DataGridView
            var columns = new[]
            {
                new DataGridViewTextBoxColumn { Name = "ID", HeaderText = "ID", DataPropertyName = "EmployeeId", Width = 80 },
                new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", DataPropertyName = "Name", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Department", HeaderText = "Department", DataPropertyName = "DepartmentName", Width = 150 },
                new DataGridViewTextBoxColumn { Name = "Position", HeaderText = "Position", DataPropertyName = "Position", Width = 180 },
                new DataGridViewTextBoxColumn { Name = "DateHired", HeaderText = "Date Hired", DataPropertyName = "HireDate", Width = 120 },
                new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", DataPropertyName = "Status", Width = 100 }
            };

            foreach (var column in columns)
            {
                dgvEmployees.Columns.Add(column);
            }

            // Add action buttons column
            DataGridViewButtonColumn editColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Actions",
                Name = "Edit",
                Text = "✏️",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 50
            };
            dgvEmployees.Columns.Add(editColumn);

            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn
            {
                HeaderText = "",
                Name = "Delete",
                Text = "🗑️",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat,
                Width = 50
            };
            dgvEmployees.Columns.Add(deleteColumn);

            dgvEmployees.CellClick += DgvEmployees_CellClick;
            dgvEmployees.CellFormatting += DgvEmployees_CellFormatting;
        }

        private void BtnFilter_Click(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            // Reset all buttons
            foreach (Control control in clickedButton.Parent.Controls)
            {
                if (control is Button button)
                {
                    button.BackColor = Color.FromArgb(245, 245, 245);
                    button.ForeColor = Color.FromArgb(100, 100, 100);
                }
            }

            // Highlight selected button
            clickedButton.BackColor = Color.White;
            clickedButton.ForeColor = Color.FromArgb(68, 93, 233);

            // Apply filter
            string filter = (string)clickedButton.Tag;
            ApplyFilter(filter);
        }

        private void ApplyFilter(string filter)
        {
            DataGridView dgvEmployees = (DataGridView)Controls.Find("dgvEmployees", true)[0];
            dgvEmployees.DataSource = null;

            var filteredEmployees = filter switch
            {
                "Active" => employees.Where(e => e.Status == EmployeeStatus.Active).ToList(),
                "On Leave" => employees.Where(e => e.Status == EmployeeStatus.OnLeave).ToList(),
                _ => employees.ToList(),
            };

            var employeeData = filteredEmployees.Select(e => new
            {
                e.EmployeeId,
                e.Name,
                DepartmentName = GetDepartmentName(e.DepartmentId),
                e.Position,
                HireDate = e.HireDate.ToString("yyyy-MM-dd"),
                Status = e.Status.ToString()
            }).ToList();

            dgvEmployees.DataSource = employeeData;
            FormatStatusColumn(dgvEmployees);
        }

        private void LoadSampleData()
        {
            // Load departments
            departments = new List<Department>
            {
                new Department { DepartmentId = "D001", Name = "Engineering", Description = "Engineering Department" },
                new Department { DepartmentId = "D002", Name = "Marketing", Description = "Marketing Department" },
                new Department { DepartmentId = "D003", Name = "Finance", Description = "Finance Department" },
                new Department { DepartmentId = "D004", Name = "Human Resources", Description = "HR Department" }
            };

            // Load employees
            employees = new List<Employee>
            {
                new Employee(
                    "P001", "John Doe", "john.doe@example.com", "123-456-7890",
                    new DateTime(1985, 5, 15), "123 Main St",
                    "E001", new DateTime(2020, 1, 15), "Software Engineer",
                    85000, "D001", EmployeeStatus.Active),

                new Employee(
                    "P002", "Jane Smith", "jane.smith@example.com", "234-567-8901",
                    new DateTime(1990, 8, 22), "456 Oak St",
                    "E002", new DateTime(2019, 5, 20), "Marketing Manager",
                    92000, "D002", EmployeeStatus.Active),

                new Employee(
                    "P003", "Robert Johnson", "robert.johnson@example.com", "345-678-9012",
                    new DateTime(1982, 3, 10), "789 Pine St",
                    "E003", new DateTime(2021, 3, 10), "Financial Analyst",
                    78000, "D003", EmployeeStatus.Active),

                new Employee(
                    "P004", "Emily Davis", "emily.davis@example.com", "456-789-0123",
                    new DateTime(1988, 11, 5), "321 Elm St",
                    "E004", new DateTime(2018, 11, 5), "HR Specialist",
                    75000, "D004", EmployeeStatus.OnLeave),

                new Employee(
                    "P005", "Michael Wilson", "michael.wilson@example.com", "567-890-1234",
                    new DateTime(1977, 8, 22), "654 Maple St",
                    "E005", new DateTime(2017, 8, 22), "Senior Developer",
                    105000, "D001", EmployeeStatus.Active)
            };
        }

        private string GetDepartmentName(string departmentId)
        {
            return departments.FirstOrDefault(d => d.DepartmentId == departmentId)?.Name ?? "Unknown";
        }

        private void PopulateEmployeeDataGridView()
        {
            DataGridView dgvEmployees = (DataGridView)Controls.Find("dgvEmployees", true)[0];

            var employeeData = employees.Select(e => new
            {
                e.EmployeeId,
                e.Name,
                DepartmentName = GetDepartmentName(e.DepartmentId),
                e.Position,
                HireDate = e.HireDate.ToString("yyyy-MM-dd"),
                Status = e.Status.ToString()
            }).ToList();

            dgvEmployees.DataSource = employeeData;
            FormatStatusColumn(dgvEmployees);
        }

        private void FormatStatusColumn(DataGridView dgvEmployees)
        {
            // Ensure we have the Status column
            if (dgvEmployees.Columns.Contains("Status"))
            {
                // Store the DataGridView's current data
                var dataSource = dgvEmployees.DataSource;

                // Handle status cell formatting through CellFormatting event
                dgvEmployees.CellFormatting += (sender, e) =>
                {
                    if (e.ColumnIndex == dgvEmployees.Columns["Status"].Index && e.RowIndex >= 0)
                    {
                        if (e.Value != null)
                        {
                            string status = e.Value.ToString();
                            if (status == "Active")
                            {
                                e.CellStyle.ForeColor = Color.Green;
                                e.CellStyle.BackColor = Color.FromArgb(230, 255, 230);
                            }
                            else if (status == "OnLeave")
                            {
                                e.CellStyle.ForeColor = Color.Orange;
                                e.CellStyle.BackColor = Color.FromArgb(255, 250, 230);
                                e.Value = "On Leave";
                            }
                        }
                    }
                };
            }
        }

        private void DgvEmployees_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // Format Status column
            if (dgv.Columns[e.ColumnIndex].Name == "Status" && e.RowIndex >= 0)
            {
                if (e.Value != null)
                {
                    string status = e.Value.ToString();
                    if (status == "Active")
                    {
                        e.CellStyle.ForeColor = Color.Green;
                        e.CellStyle.BackColor = Color.FromArgb(230, 255, 230);
                    }
                    else if (status == "OnLeave")
                    {
                        e.CellStyle.ForeColor = Color.Orange;
                        e.CellStyle.BackColor = Color.FromArgb(255, 250, 230);
                        e.Value = "On Leave";
                    }
                }
            }
        }

        private void DgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            // Check if click is on a button column
            if (e.RowIndex >= 0)
            {
                if (dgv.Columns[e.ColumnIndex].Name == "Edit")
                {
                    // Get the employee ID from the row
                    string employeeId = dgv.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    MessageBox.Show($"Edit employee with ID: {employeeId}");
                    // Here you would open an edit form for the employee
                }
                else if (dgv.Columns[e.ColumnIndex].Name == "Delete")
                {
                    // Get the employee ID from the row
                    string employeeId = dgv.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                    DialogResult result = MessageBox.Show(
                        $"Are you sure you want to delete employee with ID: {employeeId}?",
                        "Confirm Delete",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Delete the employee
                        MessageBox.Show($"Employee with ID: {employeeId} deleted!");
                        // Actually remove the employee from the data source and refresh
                        employees.RemoveAll(e => e.EmployeeId == employeeId);
                        PopulateEmployeeDataGridView();
                    }
                }
            }
        }

        private void BtnAddEmployee_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Add New Employee button clicked!");
            // Here you would open a form to add a new employee
        }
    }

}