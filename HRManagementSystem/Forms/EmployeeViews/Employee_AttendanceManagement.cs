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
        private Label lblWorkingHours;
        private DateTimePicker datePicker;
        private RadioButton rbtnMonthly;
        private RadioButton rbtnDaily;
        private bool isMonthlyView = true;

        // Services
        private AttendanceService attendanceService;
        private LeaveService leaveService;

        // Employee information
        private string employeeId;
        private string employeeName;

        // Company working hours - get from the service
        private TimeSpan workStartTime;
        private TimeSpan workEndTime;

        public Employee_AttendanceView(string employeeId = "EMP001")
        {
            this.employeeId = employeeId;

            // Initialize services
            attendanceService = AttendanceService.GetInstance();
            leaveService = LeaveService.GetInstance();

            // Get working hours
            workStartTime = AttendanceService.GetWorkStartTime();
            workEndTime = AttendanceService.GetWorkEndTime();

            // Get employee name from attendance or leave records
            var attendances = attendanceService.GetEmployeeAttendance(
                employeeId, DateTime.Now.Year, DateTime.Now.Month);

            if (attendances.Count > 0)
            {
                employeeName = attendances[0].EmployeeName;
            }
            else
            {
                var leaves = leaveService.GetEmployeeLeaves(employeeId);
                if (leaves.Count > 0)
                {
                    employeeName = leaves[0].EmployeeName;
                }
                else
                {
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
                Height = 100 // Increased height for view options
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

            // Working hours label
            lblWorkingHours = new Label
            {
                Text = $"Official Working Hours: {workStartTime.ToString(@"hh\:mm")} - {workEndTime.ToString(@"hh\:mm")}",
                Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point),
                AutoSize = true,
                Location = new Point(0, 45)
            };
            headerPanel.Controls.Add(lblWorkingHours);

            // View options panel
            Panel viewOptionsPanel = new Panel
            {
                Location = new Point(0, 70),
                Size = new Size(350, 25),
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(viewOptionsPanel);

            // Monthly radio button
            rbtnMonthly = new RadioButton
            {
                Text = "Monthly View",
                Checked = true,
                Location = new Point(0, 0),
                AutoSize = true
            };
            rbtnMonthly.CheckedChanged += ViewTypeRadioButton_CheckedChanged;
            viewOptionsPanel.Controls.Add(rbtnMonthly);

            // Daily radio button
            rbtnDaily = new RadioButton
            {
                Text = "Daily View",
                Checked = false,
                Location = new Point(120, 0),
                AutoSize = true
            };
            rbtnDaily.CheckedChanged += ViewTypeRadioButton_CheckedChanged;
            viewOptionsPanel.Controls.Add(rbtnDaily);

            // Date picker
            datePicker = new DateTimePicker
            {
                Location = new Point(240, 0),
                Size = new Size(110, 25),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MMMM yyyy", // Default to month view
                ShowUpDown = true
            };
            datePicker.ValueChanged += DatePicker_ValueChanged;
            viewOptionsPanel.Controls.Add(datePicker);

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

        private void ViewTypeRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton != null && radioButton.Checked)
            {
                isMonthlyView = (radioButton == rbtnMonthly);

                // Update DateTimePicker format based on selected view
                if (isMonthlyView)
                {
                    datePicker.Format = DateTimePickerFormat.Custom;
                    datePicker.CustomFormat = "MMMM yyyy";
                    datePicker.ShowUpDown = true;
                }
                else
                {
                    datePicker.Format = DateTimePickerFormat.Short;
                    datePicker.ShowUpDown = false;
                }

                // Reload data based on the new view type
                LoadEmployeeData();
            }
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            try
            {
                List<Attendance> attendances;
                DateTime selectedDate = datePicker.Value;

                if (isMonthlyView)
                {
                    // Get current month's attendance
                    attendances = attendanceService.GetEmployeeAttendance(
                        employeeId, selectedDate.Year, selectedDate.Month);
                }
                else
                {
                    // Get selected day's attendance
                    attendances = attendanceService.GetEmployeeDailyAttendance(
                        employeeId, selectedDate);

                    // If viewing a day in the past or today, and no attendance record exists,
                    // and there's no approved leave for that day, show as absent
                    if (attendances.Count == 0 && selectedDate.Date <= DateTime.Today)
                    {
                        // Check if employee was on approved leave
                        var leavesOnDate = leaveService.GetEmployeeDailyLeaves(employeeId, selectedDate)
                            .Where(l => l.Status == LeaveStatus.Approved).ToList();

                        if (leavesOnDate.Count == 0)
                        {
                            // Add a virtual attendance record showing absence
                            Attendance absentAttendance = new Attendance
                            {
                                AttendanceId = $"ABSENT-{employeeId}-{selectedDate.ToString("yyyyMMdd")}",
                                EmployeeId = employeeId,
                                EmployeeName = employeeName,
                                Date = selectedDate,
                                ClockInTime = DateTime.MinValue,
                                ClockOutTime = DateTime.MinValue,
                                Status = AttendanceStatus.Present, // This doesn't matter, we'll override in display
                                IsAbsentRecord = true // New property to explicitly mark as absent
                            };
                            attendances.Add(absentAttendance);
                        }
                    }
                }

                attendanceGridView.Rows.Clear();
                foreach (var attendance in attendances)
                {
                    // Determine if this is an absent employee record
                    bool isAbsent = attendance.IsAbsentRecord ||
                                   attendance.AttendanceId.StartsWith("ABSENT") ||
                                   (!isMonthlyView && attendance.ClockInTime == DateTime.MinValue &&
                                    attendance.ClockOutTime == DateTime.MinValue);

                    // Always show "Absent" for absent records
                    string status = isAbsent ? "Absent" : GetAttendanceStatusString(attendance.Status);

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
                        status
                    );

                    attendanceGridView.Rows[rowIndex].Tag = attendance;

                    var statusCell = attendanceGridView.Rows[rowIndex].Cells["Status"];
                    if (isAbsent)
                    {
                        // Absent styling
                        statusCell.Style.ForeColor = Color.White;
                        statusCell.Style.BackColor = Color.FromArgb(220, 53, 69); // Red background

                        // Color the entire row with light red background
                        foreach (DataGridViewCell cell in attendanceGridView.Rows[rowIndex].Cells)
                        {
                            if (cell.ColumnIndex != statusCell.ColumnIndex)
                            {
                                cell.Style.BackColor = Color.FromArgb(255, 240, 240); // Light red
                            }
                        }
                    }
                    else
                    {
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
                            case AttendanceStatus.Late:
                                statusCell.Style.ForeColor = Color.DarkOrange;
                                statusCell.Style.BackColor = Color.FromArgb(255, 235, 200);
                                break;
                        }
                    }
                }

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

            try
            {
                List<LeaveRequest> leaveRequests;
                DateTime selectedDate = datePicker.Value;

                if (isMonthlyView)
                {
                    // Get leave requests for the selected month
                    leaveRequests = leaveService.GetEmployeeMonthlyLeaves(
                        employeeId, selectedDate.Year, selectedDate.Month);
                }
                else
                {
                    // Get leave requests for the selected day
                    leaveRequests = leaveService.GetEmployeeDailyLeaves(
                        employeeId, selectedDate);
                }

                leaveRequestsGridView.Rows.Clear();
                foreach (var request in leaveRequests)
                {
                    if (request == null) continue;

                    string leaveTypeStr = request.Type.ToString();
                    string statusStr = request.Status.ToString();

                    int rowIndex = leaveRequestsGridView.Rows.Add(
                        request.RequestId,
                        leaveTypeStr,
                        request.StartDate.ToString("yyyy-MM-dd"),
                        request.EndDate.ToString("yyyy-MM-dd"),
                        statusStr,
                        request.Remarks
                    );

                    leaveRequestsGridView.Rows[rowIndex].Tag = request;

                    var statusCell = leaveRequestsGridView.Rows[rowIndex].Cells["Status"];
                    switch (request.Status)
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
                        case LeaveStatus.Cancelled:
                            statusCell.Style.ForeColor = Color.Gray;
                            statusCell.Style.BackColor = Color.FromArgb(240, 240, 240);
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
                AttendanceStatus.HalfDay => "Half Day",
                AttendanceStatus.WorkFromHome => "Work From Home",
                AttendanceStatus.Late => "Late",
                _ => "Unknown"
            };
        }

        private void BtnClockIn_Click(object sender, EventArgs e)
        {
            try
            {
                var attendance = attendanceService.RecordAttendance(employeeId, employeeName, AttendanceStatus.Present);

                string statusMessage = attendance.Status == AttendanceStatus.Late
                    ? $"You have clocked in late at {DateTime.Now.ToString("HH:mm")}."
                    : $"You have clocked in successfully at {DateTime.Now.ToString("HH:mm")}.";

                MessageBox.Show(statusMessage, "Clock In",
                    MessageBoxButtons.OK,
                    attendance.Status == AttendanceStatus.Late ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

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
                var todayAttendances = attendanceService.GetEmployeeAttendance(
                    employeeId, DateTime.Now.Year, DateTime.Now.Month);

                var todayAttendance = todayAttendances.Find(a => a.Date.Date == DateTime.Today);

                if (todayAttendance != null)
                {
                    attendanceService.UpdateClockOut(todayAttendance.AttendanceId);

                    MessageBox.Show($"You have clocked out successfully at {DateTime.Now.ToString("HH:mm")}.",
                        "Clock Out", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                RowCount = 6,
                ColumnCount = 2
            };
            leaveRequestForm.Controls.Add(layout);

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
                MinDate = DateTime.Today.AddDays(1)
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

            Label lblRemarks = new Label { Text = "Remarks:", AutoSize = true };
            layout.Controls.Add(lblRemarks, 0, 4);

            TextBox txtRemarks = new TextBox
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(5),
                Multiline = true,
                Height = 60
            };
            layout.Controls.Add(txtRemarks, 1, 4);

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
            layout.Controls.Add(btnSubmit, 1, 5);

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

                    LeaveType leaveType = (LeaveType)Enum.Parse(typeof(LeaveType), cmbType.SelectedItem.ToString());

                    // Make sure we have the employee's full details
                    EmployeeService employeeService = EmployeeService.GetInstance();
                    Employee employee = null;

                    // Find employee using EmployeeId (which is the identifier we have)
                    var allEmployees = employeeService.GetAll();
                    employee = allEmployees.FirstOrDefault(e => e.EmployeeId == employeeId);

                    if (employee == null)
                    {
                        MessageBox.Show("Could not locate your employee record. Please contact HR.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    var request = leaveService.SubmitLeaveRequest(
                        employeeId,
                        dtpStartDate.Value,
                        dtpEndDate.Value,
                        leaveType,
                        txtRemarks.Text
                    );

                    MessageBox.Show("Leave request submitted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    leaveRequestForm.Close();

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