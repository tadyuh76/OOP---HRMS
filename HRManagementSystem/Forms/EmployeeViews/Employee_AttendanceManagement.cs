using System;
using System.Windows.Forms;
using System.Drawing;
using HRManagementSystem.Services;

namespace HRManagementSystem
{
    public class Employee_AttendanceView : Form
    {
        private Button btnClockIn;
        private Button btnClockOut;
        private Button btnRequestLeave;
        private DataGridView attendanceGridView;
        private DataGridView leaveRequestsGridView;
        private TabControl tabControl;
        private Label lblEmployeeInfo;

        // Services
        private AttendanceService attendanceService;
        private LeaveService leaveService;

        // Employee information
        private string employeeId;
        private string employeeName;

        public Employee_AttendanceView(string employeeId = "EMP001")
        {
            this.employeeId = employeeId;

            // Initialize services
            attendanceService = AttendanceService.GetInstance();
            leaveService = LeaveService.GetInstance();
            
            // Get employee name from attendance or leave records
            // Try to get from attendance records first
            var attendances = attendanceService.GetEmployeeAttendance(
                employeeId, DateTime.Now.Year, DateTime.Now.Month);
            
            if (attendances.Count > 0)
            {
                employeeName = attendances[0].EmployeeName;
            }
            else
            {
                // If no attendance records, try leaves
                var leaves = leaveService.GetEmployeeLeaves(employeeId);
                if (leaves.Count > 0)
                {
                    employeeName = leaves[0].EmployeeName;
                }
                else
                {
                    // Fallback if no data found
                    employeeName = $"Employee {employeeId}";
                }
            }

            InitializeComponent();
            LoadEmployeeData();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee View - Attendance & Leave";
            this.Size = new Size(1000, 700);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // Main layout
            TableLayoutPanel mainLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 3,
                ColumnCount = 1
            };
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.Controls.Add(mainLayout);

            // Header with employee info
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 60
            };
            mainLayout.Controls.Add(headerPanel, 0, 0);

            lblEmployeeInfo = new Label
            {
                Text = $"Employee: {employeeName} (ID: {employeeId})",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 10)
            };
            headerPanel.Controls.Add(lblEmployeeInfo);

            // Panel for action buttons
            Panel actionPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Height = 80
            };
            mainLayout.Controls.Add(actionPanel, 0, 1);

            // Clock In button
            btnClockIn = new Button
            {
                Text = "Clock In",
                BackColor = Color.FromArgb(68, 93, 233),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 45),
                Location = new Point(0, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnClockIn.FlatAppearance.BorderSize = 0;
            btnClockIn.Click += BtnClockIn_Click;
            actionPanel.Controls.Add(btnClockIn);

            // Clock Out button
            btnClockOut = new Button
            {
                Text = "Clock Out",
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 45),
                Location = new Point(170, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnClockOut.FlatAppearance.BorderSize = 0;
            btnClockOut.Click += BtnClockOut_Click;
            actionPanel.Controls.Add(btnClockOut);

            // Request Leave button
            btnRequestLeave = new Button
            {
                Text = "Request Leave",
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(150, 45),
                Location = new Point(340, 15),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRequestLeave.FlatAppearance.BorderSize = 0;
            btnRequestLeave.Click += BtnRequestLeave_Click;
            actionPanel.Controls.Add(btnRequestLeave);

            // Tab control for attendance and leave history
            tabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10)
            };

            // Attendance history tab
            TabPage attendanceTab = new TabPage
            {
                Text = "My Attendance History",
                BackColor = Color.White
            };

            // Leave requests tab
            TabPage leaveRequestsTab = new TabPage
            {
                Text = "My Leave Requests",
                BackColor = Color.White
            };

            tabControl.Controls.Add(attendanceTab);
            tabControl.Controls.Add(leaveRequestsTab);
            mainLayout.Controls.Add(tabControl, 0, 2);

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

            // Set up columns for attendance
            attendanceGridView.Columns.Add("AttendanceId", "ID");
            attendanceGridView.Columns.Add("Date", "Date");
            attendanceGridView.Columns.Add("TimeIn", "Time In");
            attendanceGridView.Columns.Add("TimeOut", "Time Out");
            attendanceGridView.Columns.Add("Status", "Status");
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

            // Set up columns for leave requests
            leaveRequestsGridView.Columns.Add("RequestId", "Request ID");
            leaveRequestsGridView.Columns.Add("Type", "Leave Type");
            leaveRequestsGridView.Columns.Add("StartDate", "Start Date");
            leaveRequestsGridView.Columns.Add("EndDate", "End Date");
            leaveRequestsGridView.Columns.Add("Status", "Status");
            leaveRequestsGridView.Columns.Add("Remarks", "Remarks");
            leaveRequestsTab.Controls.Add(leaveRequestsGridView);
        }

        private void LoadEmployeeData()
        {
            // Load attendance data for current employee
            try
            {
                // Get current month's attendance
                var attendances = attendanceService.GetEmployeeAttendance(
                    employeeId, DateTime.Now.Year, DateTime.Now.Month);

                // Display in grid
                attendanceGridView.Rows.Clear();
                foreach (var attendance in attendances)
                {
                    string clockIn = attendance.ClockInTime != DateTime.MinValue
                        ? attendance.ClockInTime.ToString("HH:mm")
                        : "--";

                    string clockOut = attendance.ClockOutTime != DateTime.MinValue
                        ? attendance.ClockOutTime.ToString("HH:mm")
                        : "--";

                    int rowIndex = attendanceGridView.Rows.Add(
                        attendance.AttendanceId,
                        attendance.Date.ToString("yyyy-MM-dd"),
                        clockIn,
                        clockOut,
                        GetAttendanceStatusString(attendance.Status)
                    );

                    // Store attendance in row's Tag
                    attendanceGridView.Rows[rowIndex].Tag = attendance;

                    // Apply color to status cell
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

                // Check if employee already clocked in today
                var todayAttendance = attendances.Find(a => a.Date.Date == DateTime.Today);
                if (todayAttendance != null)
                {
                    btnClockIn.Enabled = false;
                    btnClockIn.BackColor = Color.Gray;

                    if (todayAttendance.ClockOutTime == DateTime.MinValue)
                    {
                        btnClockOut.Enabled = true;
                    }
                    else
                    {
                        btnClockOut.Enabled = false;
                        btnClockOut.BackColor = Color.Gray;
                    }
                }
                else
                {
                    btnClockIn.Enabled = true;
                    btnClockOut.Enabled = false;
                    btnClockOut.BackColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Load leave requests for current employee
            try
            {
                var leaves = leaveService.GetEmployeeLeaves(employeeId);

                // Filter out the leave records that have associated leave requests
                leaveRequestsGridView.Rows.Clear();
                foreach (var leave in leaves)
                {
                    int rowIndex = leaveRequestsGridView.Rows.Add(
                        leave.LeaveId,
                        leave.Type.ToString(),
                        leave.StartDate.ToString("yyyy-MM-dd"),
                        leave.EndDate.ToString("yyyy-MM-dd"),
                        leave.Status.ToString(),
                        leave.Remarks
                    );

                    // Store leave in row's Tag
                    leaveRequestsGridView.Rows[rowIndex].Tag = leave;

                    // Apply color to status cell
                    var statusCell = leaveRequestsGridView.Rows[rowIndex].Cells["Status"];
                    switch (leave.Status)
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading leave data: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void BtnClockIn_Click(object sender, EventArgs e)
        {
            try
            {
                // Record clock in for the current employee
                var attendance = attendanceService.RecordAttendance(employeeId, employeeName, AttendanceStatus.Present);

                MessageBox.Show($"You have clocked in successfully at {DateTime.Now.ToString("HH:mm")}.",
                    "Clock In", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh the data
                LoadEmployeeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clocking in: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClockOut_Click(object sender, EventArgs e)
        {
            try
            {
                // Find today's attendance record to update
                var todayAttendances = attendanceService.GetEmployeeAttendance(
                    employeeId, DateTime.Now.Year, DateTime.Now.Month);

                var todayAttendance = todayAttendances.Find(a => a.Date.Date == DateTime.Today);

                if (todayAttendance != null)
                {
                    // Record clock out
                    attendanceService.UpdateClockOut(todayAttendance.AttendanceId);

                    MessageBox.Show($"You have clocked out successfully at {DateTime.Now.ToString("HH:mm")}.",
                        "Clock Out", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the data
                    LoadEmployeeData();
                }
                else
                {
                    MessageBox.Show("No clock-in record found for today. Please clock in first.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clocking out: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRequestLeave_Click(object sender, EventArgs e)
        {
            // Create leave request form
            Form leaveRequestForm = new Form
            {
                Text = "Request Leave",
                Size = new Size(450, 350),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            TableLayoutPanel layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                RowCount = 5,
                ColumnCount = 2
            };
            leaveRequestForm.Controls.Add(layout);

            // Employee ID is pre-filled and read-only
            Label lblEmployeeInfo = new Label { Text = $"Employee: {employeeName}", AutoSize = true };
            layout.Controls.Add(lblEmployeeInfo, 0, 0);
            layout.SetColumnSpan(lblEmployeeInfo, 2);

            Label lblType = new Label { Text = "Leave Type:", AutoSize = true };
            layout.Controls.Add(lblType, 0, 1);

            ComboBox cmbType = new ComboBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbType.Items.AddRange(Enum.GetNames(typeof(LeaveType)));
            cmbType.SelectedIndex = 0;
            layout.Controls.Add(cmbType, 1, 1);

            Label lblStartDate = new Label { Text = "Start Date:", AutoSize = true };
            layout.Controls.Add(lblStartDate, 0, 2);

            DateTimePicker dtpStartDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today.AddDays(1) // Leave must be at least 1 day in advance
            };
            layout.Controls.Add(dtpStartDate, 1, 2);

            Label lblEndDate = new Label { Text = "End Date:", AutoSize = true };
            layout.Controls.Add(lblEndDate, 0, 3);

            DateTimePicker dtpEndDate = new DateTimePicker
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                Format = DateTimePickerFormat.Short,
                MinDate = DateTime.Today.AddDays(1)
            };
            layout.Controls.Add(dtpEndDate, 1, 3);

            Button btnSubmit = new Button
            {
                Text = "Submit Request",
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(68, 93, 233),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSubmit.FlatAppearance.BorderSize = 0;
            layout.Controls.Add(btnSubmit, 1, 4);

            btnSubmit.Click += (s, args) =>
            {
                try
                {
                    if (dtpStartDate.Value > dtpEndDate.Value)
                    {
                        MessageBox.Show("Start date must be before or equal to end date.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Parse the leave type
                    LeaveType leaveType = (LeaveType)Enum.Parse(typeof(LeaveType), cmbType.SelectedItem.ToString());

                    // Submit leave request using the service
                    var request = leaveService.SubmitLeaveRequest(
                        employeeId,
                        dtpStartDate.Value,
                        dtpEndDate.Value,
                        leaveType,
                        "Employee request"
                    );

                    MessageBox.Show("Leave request submitted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    leaveRequestForm.Close();

                    // Refresh data to show the new request
                    LoadEmployeeData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error submitting leave request: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            leaveRequestForm.ShowDialog();
        }
    }
}