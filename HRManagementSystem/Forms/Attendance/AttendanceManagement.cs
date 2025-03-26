namespace HRManagementSystem
{
    public class AttendanceManagement : Form
    {

        private TabControl tabControl;
        private TabPage attendanceTab;
        private TabPage leaveRequestsTab;
        private DataGridView attendanceGridView;
        private DataGridView leaveRequestsGridView;
        private Button btnClockInOut;
        private Button btnRequestLeave;
        private DateTimePicker datePicker;
        private TextBox searchBox;

        public AttendanceManagement()
        {
            InitializeComponent();
            LoadDummyData();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.WhiteSmoke;
            this.Dock = DockStyle.Fill;
            this.Size = new Size(1000, 1000);
            this.Padding = new Padding(100);

            // Title Section
            Label lblTitle = new Label();
            lblTitle.Text = "Attendance - Leave Management";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            // Month Picker
            datePicker = new DateTimePicker();
            datePicker.Location = new Point(660, 92);
            datePicker.Size = new Size(170, 40);
            datePicker.Format = DateTimePickerFormat.Custom;
            datePicker.CustomFormat = "MMMM yyyy";
            datePicker.ShowUpDown = true;
            this.Controls.Add(datePicker);

            // // Label showing selected month (as a checkbox-like display)
            // Label lblMonth = new Label();
            // lblMonth.Text = "☑ March 2023";
            // lblMonth.Font = new Font("Segoe UI", 9);
            // lblMonth.Location = new Point(720, 62);
            // lblMonth.AutoSize = true;
            // this.Controls.Add(lblMonth);

            // // Button section
            // btnClockInOut = new Button();
            // btnClockInOut.Text = "Clock In/Out";
            // btnClockInOut.BackColor = Color.RoyalBlue;
            // btnClockInOut.ForeColor = Color.White;
            // btnClockInOut.FlatStyle = FlatStyle.Flat;
            // btnClockInOut.FlatAppearance.BorderSize = 0;
            // btnClockInOut.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            // btnClockInOut.Size = new Size(120, 35);
            // btnClockInOut.Location = new Point(660, 92);
            // btnClockInOut.Click += BtnClockInOut_Click;
            // this.Controls.Add(btnClockInOut);

            // btnRequestLeave = new Button();
            // btnRequestLeave.Text = "+ Request Leave";
            // btnRequestLeave.BackColor = Color.White;
            // btnRequestLeave.ForeColor = Color.Black;
            // btnRequestLeave.FlatStyle = FlatStyle.Flat;
            // btnRequestLeave.Font = new Font("Segoe UI", 9);
            // btnRequestLeave.Size = new Size(120, 35);
            // btnRequestLeave.Location = new Point(790, 92);
            // btnRequestLeave.Click += BtnRequestLeave_Click;
            // this.Controls.Add(btnRequestLeave);

            // Label for Records Section
            Label lblRecords = new Label();
            lblRecords.Text = "Attendance & Leave Records";
            lblRecords.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            lblRecords.Location = new Point(20, 95);
            lblRecords.AutoSize = true;
            this.Controls.Add(lblRecords);

            // Search Box
            searchBox = new TextBox();
            searchBox.Location = new Point(20, 130);
            searchBox.Size = new Size(900, 50);
            searchBox.PlaceholderText = "🔍 Search records...";
            searchBox.BorderStyle = BorderStyle.FixedSingle;
            searchBox.TextChanged += SearchBox_TextChanged;
            this.Controls.Add(searchBox);

            // Tab Control
            tabControl = new TabControl();
            tabControl.Location = new Point(20, 165);
            tabControl.Size = new Size(860, 400);
            tabControl.Font = new Font("Segoe UI", 10);

            // Attendance Tab
            attendanceTab = new TabPage();
            attendanceTab.Text = "Attendance";
            attendanceTab.BackColor = Color.White;
            tabControl.Controls.Add(attendanceTab);

            // Leave Requests Tab
            leaveRequestsTab = new TabPage();
            leaveRequestsTab.Text = "Leave Requests";
            leaveRequestsTab.BackColor = Color.White;
            tabControl.Controls.Add(leaveRequestsTab);

            this.Controls.Add(tabControl);

            // Attendance Grid
            attendanceGridView = new DataGridView();
            attendanceGridView.Location = new Point(10, 10);
            attendanceGridView.Size = new Size(840, 300);
            attendanceGridView.BackgroundColor = Color.White;
            attendanceGridView.BorderStyle = BorderStyle.None;
            attendanceGridView.RowHeadersVisible = false;
            attendanceGridView.AllowUserToAddRows = false;
            attendanceGridView.ReadOnly = true;
            attendanceGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            attendanceGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            attendanceGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            attendanceGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            attendanceGridView.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            attendanceGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            attendanceGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            attendanceGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            attendanceGridView.CellClick += AttendanceGridView_CellClick;

            // Set up columns
            attendanceGridView.Columns.Add("EmployeeId", "Employee ID");
            attendanceGridView.Columns.Add("Name", "Name");
            attendanceGridView.Columns.Add("Date", "Date");
            attendanceGridView.Columns.Add("TimeIn", "Time In");
            attendanceGridView.Columns.Add("TimeOut", "Time Out");
            attendanceGridView.Columns.Add("Status", "Status");

            // Add an Actions column with buttons
            DataGridViewButtonColumn actionsColumn = new DataGridViewButtonColumn();
            actionsColumn.HeaderText = "Actions";
            actionsColumn.Name = "Actions";
            actionsColumn.Text = "⚙";
            actionsColumn.UseColumnTextForButtonValue = true;
            attendanceGridView.Columns.Add(actionsColumn);

            attendanceTab.Controls.Add(attendanceGridView);

            // Leave Requests Grid
            leaveRequestsGridView = new DataGridView();
            leaveRequestsGridView.Location = new Point(10, 10);
            leaveRequestsGridView.Size = new Size(830, 350);
            leaveRequestsGridView.BackgroundColor = Color.White;
            leaveRequestsGridView.BorderStyle = BorderStyle.None;
            leaveRequestsGridView.RowHeadersVisible = false;
            leaveRequestsGridView.AllowUserToAddRows = false;
            leaveRequestsGridView.ReadOnly = true;
            leaveRequestsGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            leaveRequestsGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            leaveRequestsGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            leaveRequestsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            leaveRequestsGridView.DefaultCellStyle.SelectionBackColor = Color.LightSkyBlue;
            leaveRequestsGridView.DefaultCellStyle.SelectionForeColor = Color.Black;
            leaveRequestsGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            leaveRequestsGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            leaveRequestsGridView.CellClick += LeaveRequestsGridView_CellClick;

            // Set up columns for leave requests
            leaveRequestsGridView.Columns.Add("RequestId", "Request ID");
            leaveRequestsGridView.Columns.Add("EmployeeId", "Employee ID");
            leaveRequestsGridView.Columns.Add("Name", "Name");
            leaveRequestsGridView.Columns.Add("Type", "Leave Type");
            leaveRequestsGridView.Columns.Add("StartDate", "Start Date");
            leaveRequestsGridView.Columns.Add("EndDate", "End Date");
            leaveRequestsGridView.Columns.Add("Status", "Status");

            // Add an Actions column with buttons for leave requests
            DataGridViewButtonColumn leaveActionsColumn = new DataGridViewButtonColumn();
            leaveActionsColumn.HeaderText = "Actions";
            leaveActionsColumn.Name = "Actions";
            leaveActionsColumn.Text = "⚙";
            leaveActionsColumn.UseColumnTextForButtonValue = true;
            leaveRequestsGridView.Columns.Add(leaveActionsColumn);

            leaveRequestsTab.Controls.Add(leaveRequestsGridView);
        }

        private void LoadDummyData()
        {
            // Load dummy data for attendance
            attendanceGridView.Rows.Add("E001", "John Doe", "2023-03-01", "08:00", "17:30", "Present");
            attendanceGridView.Rows.Add("E002", "Jane Smith", "2023-03-01", "08:45", "17:15", "Present");
            attendanceGridView.Rows.Add("E003", "Robert Johnson", "2023-03-01", "09:10", "17:45", "Present");
            attendanceGridView.Rows.Add("E004", "Emily Davis", "2023-03-01", "--", "--", "Absent");
            attendanceGridView.Rows.Add("E005", "Michael Wilson", "2023-03-01", "08:30", "16:45", "Present");

            // Apply color coding to status cells
            foreach (DataGridViewRow row in attendanceGridView.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "Present")
                {
                    row.Cells["Status"].Style.ForeColor = Color.Green;
                }
                else if (row.Cells["Status"].Value.ToString() == "Absent")
                {
                    row.Cells["Status"].Style.ForeColor = Color.Red;
                }
            }

            // Load dummy data for leave requests
            leaveRequestsGridView.Rows.Add("LR001", "E001", "John Doe", "Annual", "2023-03-15", "2023-03-20", "Pending");
            leaveRequestsGridView.Rows.Add("LR002", "E003", "Robert Johnson", "Sick", "2023-03-08", "2023-03-09", "Approved");
            leaveRequestsGridView.Rows.Add("LR003", "E002", "Jane Smith", "Personal", "2023-03-22", "2023-03-24", "Rejected");

            // Apply color coding to status cells
            foreach (DataGridViewRow row in leaveRequestsGridView.Rows)
            {
                if (row.Cells["Status"].Value.ToString() == "Approved")
                {
                    row.Cells["Status"].Style.ForeColor = Color.Green;
                }
                else if (row.Cells["Status"].Value.ToString() == "Rejected")
                {
                    row.Cells["Status"].Style.ForeColor = Color.Red;
                }
                else if (row.Cells["Status"].Value.ToString() == "Pending")
                {
                    row.Cells["Status"].Style.ForeColor = Color.Orange;
                }
            }
        }

        private void BtnClockInOut_Click(object sender, EventArgs e)
        {
            // Open clock in/out dialog
            MessageBox.Show("Clock In/Out functionality will be implemented here.", "Clock In/Out", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnRequestLeave_Click(object sender, EventArgs e)
        {
            // Open leave request dialog
            MessageBox.Show("Leave Request form will be implemented here.", "Request Leave", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            // Implement search functionality
            string searchText = searchBox.Text.ToLower();

            if (tabControl.SelectedTab == attendanceTab)
            {
                foreach (DataGridViewRow row in attendanceGridView.Rows)
                {
                    bool visible = false;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            visible = true;
                            break;
                        }
                    }

                    row.Visible = visible;
                }
            }
            else if (tabControl.SelectedTab == leaveRequestsTab)
            {
                foreach (DataGridViewRow row in leaveRequestsGridView.Rows)
                {
                    bool visible = false;

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchText))
                        {
                            visible = true;
                            break;
                        }
                    }

                    row.Visible = visible;
                }
            }
        }

        private void AttendanceGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the Actions column
            if (e.ColumnIndex == attendanceGridView.Columns["Actions"].Index && e.RowIndex >= 0)
            {
                string employeeId = attendanceGridView.Rows[e.RowIndex].Cells["EmployeeId"].Value.ToString();
                string name = attendanceGridView.Rows[e.RowIndex].Cells["Name"].Value.ToString();

                // Show actions menu for this attendance record
                MessageBox.Show($"Actions for {name} ({employeeId})", "Attendance Actions", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void LeaveRequestsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the Actions column
            if (e.ColumnIndex == leaveRequestsGridView.Columns["Actions"].Index && e.RowIndex >= 0)
            {
                string requestId = leaveRequestsGridView.Rows[e.RowIndex].Cells["RequestId"].Value.ToString();
                string name = leaveRequestsGridView.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                string status = leaveRequestsGridView.Rows[e.RowIndex].Cells["Status"].Value.ToString();

                // Show context menu with approve/reject options
                ContextMenuStrip actionMenu = new ContextMenuStrip();

                if (status == "Pending")
                {
                    actionMenu.Items.Add("Approve", null, (s, args) => 
                    {
                        leaveRequestsGridView.Rows[e.RowIndex].Cells["Status"].Value = "Approved";
                        leaveRequestsGridView.Rows[e.RowIndex].Cells["Status"].Style.ForeColor = Color.Green;
                    });

                    actionMenu.Items.Add("Reject", null, (s, args) => 
                    {
                        leaveRequestsGridView.Rows[e.RowIndex].Cells["Status"].Value = "Rejected";
                        leaveRequestsGridView.Rows[e.RowIndex].Cells["Status"].Style.ForeColor = Color.Red;
                    });
                }

                actionMenu.Items.Add("View Details", null, (s, args) => 
                {
                    MessageBox.Show($"Leave request details for {name}", "Leave Request Details", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                });

                actionMenu.Show(Cursor.Position);
            }
        }
    }
}