using HRManagementSystem.Services;
using System.Data;
using System.Text.Json;

namespace HRManagementSystem
{
    public class AttendanceManagement : Form
    {
        private TabControl tabControl;
        private TabPage attendanceTab;
        private TabPage leaveRequestsTab;
        private DataGridView attendanceGridView;
        private DataGridView leaveRequestsGridView;
        private DateTimePicker datePicker;
        private TextBox searchBox;

        // Add services for attendance and leave management
        private AttendanceService attendanceService;
        private LeaveService leaveService;
        private EmployeeService employeeService;

        // For tracking current view month/year
        private int currentMonth;
        private int currentYear;

        // Lists to store the loaded data
        private List<Attendance> attendances;
        private List<LeaveRequest> leaveRequests;

        public AttendanceManagement()
        {
            InitializeComponent();

            // Initialize services using singleton pattern
            attendanceService = AttendanceService.GetInstance();
            leaveService = LeaveService.GetInstance();
            employeeService = EmployeeService.GetInstance();

            // Set current month and year
            currentMonth = DateTime.Now.Month;
            currentYear = DateTime.Now.Year;

            // Set date picker to current month
            datePicker.Value = new DateTime(currentYear, currentMonth, 1);

            // Load data from services
            LoadAttendanceData();
            LoadLeaveData();
        }

        private void InitializeComponent()
        {
            this.BackColor = Color.WhiteSmoke;
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

            // Header panel with title
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60
            };
            mainLayout.Controls.Add(headerPanel, 0, 0);

            // Title label
            Label lblTitle = new Label
            {
                Text = "Attendance Management",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(0, 10)
            };
            headerPanel.Controls.Add(lblTitle);

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

            // Top control panel (with labels, search box, month picker)
            Panel controlPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60
            };
            directoryLayout.Controls.Add(controlPanel, 0, 0);

            // Records section label
            Label lblRecords = new Label
            {
                Text = "Attendance & Leave Records",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(0, 0)
            };
            controlPanel.Controls.Add(lblRecords);

            // Month picker
            datePicker = new DateTimePicker
            {
                Location = new Point(controlPanel.Width - 170, 0),
                Size = new Size(150, 30),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMMM yyyy",
                ShowUpDown = true,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            datePicker.ValueChanged += DatePicker_ValueChanged;
            controlPanel.Controls.Add(datePicker);

            // Search and filter panel
            Panel searchPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 50
            };
            directoryLayout.Controls.Add(searchPanel, 0, 1);

            // Search box
            searchBox = new TextBox
            {
                Size = new Size(500, 35),
                Location = new Point(0, 10),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                PlaceholderText = "🔍 Search records..."
            };
            searchBox.TextChanged += SearchBox_TextChanged;
            searchPanel.Controls.Add(searchBox);

            // Tab Control
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Regular)
            };
            directoryLayout.Controls.Add(tabControl, 0, 2);

            // Attendance Tab
            attendanceTab = new TabPage
            {
                Text = "Attendance",
                BackColor = Color.White
            };
            tabControl.Controls.Add(attendanceTab);

            // Leave Requests Tab
            leaveRequestsTab = new TabPage
            {
                Text = "Leave Requests",
                BackColor = Color.White
            };
            tabControl.Controls.Add(leaveRequestsTab);

            // Attendance Grid
            attendanceGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DefaultCellStyle = { SelectionBackColor = Color.LightSkyBlue, SelectionForeColor = Color.Black }
            };
            attendanceGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            attendanceGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            attendanceGridView.CellClick += AttendanceGridView_CellClick;

            // Set up columns for attendance
            attendanceGridView.Columns.Add("AttendanceId", "Attendance ID");
            attendanceGridView.Columns.Add("EmployeeId", "Employee ID");
            attendanceGridView.Columns.Add("Name", "Name");
            attendanceGridView.Columns.Add("Date", "Date");
            attendanceGridView.Columns.Add("TimeIn", "Time In");
            attendanceGridView.Columns.Add("TimeOut", "Time Out");
            attendanceGridView.Columns.Add("Status", "Status");

            // Add Actions column with buttons
            DataGridViewButtonColumn actionsColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Actions",
                Name = "Actions",
                Text = "⚙",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            attendanceGridView.Columns.Add(actionsColumn);
            attendanceTab.Controls.Add(attendanceGridView);

            // Leave Requests Grid
            leaveRequestsGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                DefaultCellStyle = { SelectionBackColor = Color.LightSkyBlue, SelectionForeColor = Color.Black }
            };
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
            leaveRequestsGridView.Columns.Add("Remarks", "Remarks");

            // Add Actions column with buttons for leave requests
            DataGridViewButtonColumn leaveActionsColumn = new DataGridViewButtonColumn
            {
                HeaderText = "Actions",
                Name = "Actions",
                Text = "⚙",
                UseColumnTextForButtonValue = true,
                FlatStyle = FlatStyle.Flat
            };
            leaveRequestsGridView.Columns.Add(leaveActionsColumn);
            leaveRequestsTab.Controls.Add(leaveRequestsGridView);
        }

        private void LoadAttendanceData()
        {
            try
            {
                // Get attendances for the selected month and year
                attendances = attendanceService.GetMonthlyAttendance(currentYear, currentMonth);
                DisplayAttendanceData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayAttendanceData()
        {
            attendanceGridView.Rows.Clear();

            foreach (var attendance in attendances)
            {
                // Use the EmployeeName directly from the attendance record
                string employeeName = attendance.EmployeeName ?? "Unknown";

                string clockIn = attendance.ClockInTime != DateTime.MinValue
                    ? attendance.ClockInTime.ToString("HH:mm")
                    : "--";

                string clockOut = attendance.ClockOutTime != DateTime.MinValue
                    ? attendance.ClockOutTime.ToString("HH:mm")
                    : "--";

                int rowIndex = attendanceGridView.Rows.Add(
                    attendance.AttendanceId,
                    attendance.EmployeeId,
                    employeeName,
                    attendance.Date.ToString("yyyy-MM-dd"),
                    clockIn,
                    clockOut,
                    GetAttendanceStatusString(attendance.Status)
                );

                // Store the attendance object in the row's Tag for later retrieval
                attendanceGridView.Rows[rowIndex].Tag = attendance;

                // Apply color to status cells
                var statusCell = attendanceGridView.Rows[rowIndex].Cells["Status"];
                switch (attendance.Status)
                {
                    case AttendanceStatus.Present:
                        statusCell.Style.ForeColor = Color.Green;
                        statusCell.Style.BackColor = Color.FromArgb(230, 255, 230);
                        break;
                    case AttendanceStatus.HalfDay:
                        statusCell.Style.ForeColor = Color.Orange;
                        statusCell.Style.BackColor = Color.FromArgb(255, 250, 230);
                        break;
                    case AttendanceStatus.WorkFromHome:
                        statusCell.Style.ForeColor = Color.DarkOrange;
                        statusCell.Style.BackColor = Color.FromArgb(255, 240, 230);
                        break;
                    case AttendanceStatus.Absent:
                        statusCell.Style.ForeColor = Color.Red;
                        statusCell.Style.BackColor = Color.FromArgb(255, 230, 230);
                        break;
                }
            }
        }

        private string GetAttendanceStatusString(AttendanceStatus status)
        {
            return status switch
            {
                AttendanceStatus.Present => "Present",
                AttendanceStatus.Absent => "Absent",
                AttendanceStatus.WorkFromHome => "Work From Home",
                AttendanceStatus.HalfDay => "Half Day",
                _ => "Unknown"
            };
        }

        private void LoadLeaveData()
        {
            try
            {
                // Get all leave requests - may need additional filtering in a real implementation
                leaveRequests = leaveService.GetPendingLeaveRequests();
                DisplayLeaveRequestsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading leave data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayLeaveRequestsData()
        {
            leaveRequestsGridView.Rows.Clear();

            foreach (var request in leaveRequests)
            {
                // Use the EmployeeName directly from the leave details
                string employeeName = request.LeaveDetails.EmployeeName ?? "Unknown";

                int rowIndex = leaveRequestsGridView.Rows.Add(
                    request.RequestId,
                    request.EmployeeId,
                    employeeName,
                    request.LeaveDetails.Type.ToString(),
                    request.LeaveDetails.StartDate.ToString("yyyy-MM-dd"),
                    request.LeaveDetails.EndDate.ToString("yyyy-MM-dd"),
                    request.LeaveDetails.Status.ToString(),
                    request.LeaveDetails.Remarks
                );

                // Store the leave request object in the row's Tag for later retrieval
                leaveRequestsGridView.Rows[rowIndex].Tag = request;

                // Apply color to status cells
                var statusCell = leaveRequestsGridView.Rows[rowIndex].Cells["Status"];
                switch (request.LeaveDetails.Status)
                {
                    case LeaveStatus.Approved:
                        statusCell.Style.ForeColor = Color.Green;
                        statusCell.Style.BackColor = Color.FromArgb(230, 255, 230);
                        break;
                    case LeaveStatus.Rejected:
                        statusCell.Style.ForeColor = Color.Red;
                        statusCell.Style.BackColor = Color.FromArgb(255, 230, 230);
                        break;
                    case LeaveStatus.Pending:
                        statusCell.Style.ForeColor = Color.Orange;
                        statusCell.Style.BackColor = Color.FromArgb(255, 250, 230);
                        break;
                }
            }
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            // Update current month and year
            currentMonth = datePicker.Value.Month;
            currentYear = datePicker.Value.Year;

            // Reload attendance data for the selected month
            LoadAttendanceData();
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchBox.Text.ToLower();

            if (tabControl.SelectedTab == attendanceTab)
            {
                foreach (DataGridViewRow row in attendanceGridView.Rows)
                {
                    bool visible = false;

                    // Check if any cell in the row contains the search text
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

                    // Check if any cell in the row contains the search text
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
                // Get the attendance record from the row's Tag
                Attendance attendance = attendanceGridView.Rows[e.RowIndex].Tag as Attendance;
                if (attendance == null) return;

                // Get employee name directly from attendance record
                string employeeName = attendance.EmployeeName ?? "Unknown";

                // Show context menu with options
                ContextMenuStrip actionMenu = new ContextMenuStrip();

                // If the employee hasn't clocked out yet, add a clock out option
                if (attendance.ClockOutTime == DateTime.MinValue)
                {
                    actionMenu.Items.Add("Record Clock Out", null, (s, args) =>
                    {
                        try
                        {
                            attendanceService.UpdateClockOut(attendance.AttendanceId);
                            LoadAttendanceData(); // Refresh the data
                            MessageBox.Show("Clock out recorded successfully!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error recording clock out: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });
                }

                actionMenu.Items.Add("View Details", null, (s, args) =>
                {
                    MessageBox.Show(
                        $"Attendance Details\n\n" +
                        $"Employee: {employeeName} ({attendance.EmployeeId})\n" +
                        $"Date: {attendance.Date.ToShortDateString()}\n" +
                        $"Clock In: {(attendance.ClockInTime != DateTime.MinValue ? attendance.ClockInTime.ToString("HH:mm") : "Not recorded")}\n" +
                        $"Clock Out: {(attendance.ClockOutTime != DateTime.MinValue ? attendance.ClockOutTime.ToString("HH:mm") : "Not recorded")}\n" +
                        $"Status: {GetAttendanceStatusString(attendance.Status)}",
                        "Attendance Details",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                });

                actionMenu.Show(Cursor.Position);
            }
        }

        private void LeaveRequestsGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the Actions column
            if (e.ColumnIndex == leaveRequestsGridView.Columns["Actions"].Index && e.RowIndex >= 0)
            {
                // Get the leave request from the row's Tag
                LeaveRequest request = leaveRequestsGridView.Rows[e.RowIndex].Tag as LeaveRequest;
                if (request == null) return;

                // Get employee name directly from leave request
                string employeeName = request.LeaveDetails.EmployeeName ?? "Unknown";

                // Show context menu with approve/reject options
                ContextMenuStrip actionMenu = new ContextMenuStrip();

                if (request.LeaveDetails.Status == LeaveStatus.Pending)
                {
                    actionMenu.Items.Add("Approve", null, (s, args) =>
                    {
                        try
                        {
                            // Use an admin ID for approval (in a real system, this would be the logged-in user's ID)
                            string approverId = "ADMIN001";
                            leaveService.ApproveLeaveRequest(request.RequestId, approverId);
                            LoadLeaveData(); // Refresh the data
                            MessageBox.Show("Leave request approved!", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error approving leave request: {ex.Message}", "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    });

                    actionMenu.Items.Add("Reject", null, (s, args) =>
                    {
                        // Create a simple form to get rejection reason
                        Form rejectionForm = new Form
                        {
                            Text = "Reject Leave Request",
                            Size = new Size(400, 200),
                            StartPosition = FormStartPosition.CenterParent,
                            FormBorderStyle = FormBorderStyle.FixedDialog,
                            MaximizeBox = false,
                            MinimizeBox = false
                        };

                        Label lblReason = new Label
                        {
                            Text = "Rejection Reason:",
                            Location = new Point(20, 20),
                            AutoSize = true
                        };
                        rejectionForm.Controls.Add(lblReason);

                        TextBox txtReason = new TextBox
                        {
                            Location = new Point(20, 50),
                            Size = new Size(340, 80),
                            Multiline = true
                        };
                        rejectionForm.Controls.Add(txtReason);

                        Button btnConfirm = new Button
                        {
                            Text = "Confirm Rejection",
                            Location = new Point(240, 140),
                            Size = new Size(120, 30),
                            BackColor = Color.FromArgb(220, 53, 69),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };
                        btnConfirm.FlatAppearance.BorderSize = 0;
                        rejectionForm.Controls.Add(btnConfirm);

                        btnConfirm.Click += (sender, e) =>
                        {
                            try
                            {
                                if (string.IsNullOrWhiteSpace(txtReason.Text))
                                {
                                    MessageBox.Show("Please provide a reason for rejection.", "Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                // Use an admin ID for rejection (in a real system, this would be the logged-in user's ID)
                                string approverId = "ADMIN001";
                                leaveService.RejectLeaveRequest(request.RequestId, approverId, txtReason.Text);
                                LoadLeaveData(); // Refresh the data

                                rejectionForm.Close();
                                MessageBox.Show("Leave request rejected!", "Success",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error rejecting leave request: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        };

                        rejectionForm.ShowDialog();
                    });
                }

                actionMenu.Items.Add("View Details", null, (s, args) =>
                {
                    MessageBox.Show(
                        $"Leave Request Details\n\n" +
                        $"Employee: {employeeName} ({request.EmployeeId})\n" +
                        $"Request Date: {request.RequestDate.ToShortDateString()}\n" +
                        $"Leave Type: {request.LeaveDetails.Type}\n" +
                        $"Period: {request.LeaveDetails.StartDate.ToShortDateString()} to {request.LeaveDetails.EndDate.ToShortDateString()}\n" +
                        $"Status: {request.LeaveDetails.Status}\n" +
                        $"Remarks: {request.LeaveDetails.Remarks}",
                        "Leave Request Details",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                });

                actionMenu.Show(Cursor.Position);
            }
        }
    }
}