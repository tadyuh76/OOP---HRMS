using System;
using System.Windows.Forms;
using System.Drawing;

namespace HRManagementSystem
{
    public class Employee_AttendanceView : Form
    {
        private Button btnClockIn;
        private Button btnClockOut;
        private Button btnRequestLeave;

        public Employee_AttendanceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee View - Attendance & Leave";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            // Button section
            btnClockIn = new Button();
            btnClockIn.Text = "Clock In";
            btnClockIn.BackColor = Color.RoyalBlue;
            btnClockIn.ForeColor = Color.White;
            btnClockIn.FlatStyle = FlatStyle.Flat;
            btnClockIn.FlatAppearance.BorderSize = 0;
            btnClockIn.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClockIn.Size = new Size(200, 50);
            btnClockIn.Location = new Point(250,300);
            btnClockIn.Click += BtnClockIn_Click;
            this.Controls.Add(btnClockIn);

            btnClockOut = new Button();
            btnClockOut.Text = "Clock Out";
            btnClockOut.BackColor = Color.RoyalBlue;
            btnClockOut.ForeColor = Color.White;
            btnClockOut.FlatStyle = FlatStyle.Flat;
            btnClockOut.FlatAppearance.BorderSize = 0;
            btnClockOut.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnClockOut.Size = new Size(200, 50);
            btnClockOut.Location = new Point(500, 300);
            btnClockOut.Click += BtnClockOut_Click;
            this.Controls.Add(btnClockOut);

            btnRequestLeave = new Button();
            btnRequestLeave.Text = "Request Leave";
            btnRequestLeave.BackColor = Color.White;
            btnRequestLeave.ForeColor = Color.Black;
            btnRequestLeave.FlatStyle = FlatStyle.Flat;
            btnRequestLeave.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnRequestLeave.Size = new Size(200, 50);
            btnRequestLeave.Location = new Point(750, 300);
            btnRequestLeave.Click += BtnRequestLeave_Click;
            this.Controls.Add(btnRequestLeave);

            Label title = new Label
            {
                Text = "My Attendance - Leave Requests",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(title);
        }

        private void BtnClockIn_Click(object sender, EventArgs e)
        {
            // Create a custom form for Clock In
            Form clockInForm = new Form
            {
                Text = "Clock In",
                Size = new Size(350, 350),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };

            // Employee ID input
            Label lblEmployeeID = new Label
            {
                Text = "Employee ID:",
                Location = new Point(20, 20),
                Size = new Size(100, 25)
            };
            TextBox txtEmployeeID = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(150, 25)
            };

            // Employee Name input
            Label lblEmployeeName = new Label
            {
                Text = "Employee Name:",
                Location = new Point(20, 60),
                Size = new Size(100, 25)
            };
            TextBox txtEmployeeName = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(150, 25)
            };

            // Attendance Status input
            Label lblAttendanceStatus = new Label
            {
                Text = "Attendance Status:",
                Location = new Point(20, 100),
                Size = new Size(120, 25)
            };
            ComboBox cbAttendanceStatus = new ComboBox
            {
                Location = new Point(150, 100),
                Size = new Size(150, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Add attendance status options
            cbAttendanceStatus.Items.AddRange(new string[] 
            { 
                "Present", 
                "Absent", 
                "HalfDay", 
                "WorkFromHome" 
            });

            // Submit button
            Button btnSubmit = new Button
            {
                Text = "Submit",
                Location = new Point(100, 250),
                Size = new Size(100, 30)
            };
            btnSubmit.Click += (s, ev) => 
            {
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) || 
                    string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
                    cbAttendanceStatus.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all fields.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show(
                    $"Clock In successful for {txtEmployeeName.Text} (ID: {txtEmployeeID.Text})\n" +
                    $"Attendance Status: {cbAttendanceStatus.SelectedItem}", 
                    "Success", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                clockInForm.Close();
            };

            // Add controls to form
            clockInForm.Controls.AddRange(new Control[] 
            { 
                lblEmployeeID, txtEmployeeID, 
                lblEmployeeName, txtEmployeeName,
                lblAttendanceStatus, cbAttendanceStatus,
                btnSubmit 
            });

            clockInForm.ShowDialog();
        }

        private void BtnClockOut_Click(object sender, EventArgs e)
        {
            // Create a custom form for Clock Out (similar to Clock In)
            Form clockOutForm = new Form
            {
                Text = "Clock Out",
                Size = new Size(350, 250),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };

            // Employee ID input
            Label lblEmployeeID = new Label
            {
                Text = "Employee ID:",
                Location = new Point(20, 20),
                Size = new Size(100, 25)
            };
            TextBox txtEmployeeID = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(150, 25)
            };

            // Employee Name input
            Label lblEmployeeName = new Label
            {
                Text = "Employee Name:",
                Location = new Point(20, 60),
                Size = new Size(100, 25)
            };
            TextBox txtEmployeeName = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(150, 25)
            };

            // Submit button
            Button btnSubmit = new Button
            {
                Text = "Submit",
                Location = new Point(100, 150),
                Size = new Size(100, 30)
            };
            btnSubmit.Click += (s, ev) => 
            {
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) || 
                    string.IsNullOrWhiteSpace(txtEmployeeName.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show($"Clock Out successful for {txtEmployeeName.Text} (ID: {txtEmployeeID.Text})", 
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clockOutForm.Close();
            };

            // Add controls to form
            clockOutForm.Controls.AddRange(new Control[] 
            { 
                lblEmployeeID, txtEmployeeID, 
                lblEmployeeName, txtEmployeeName, 
                btnSubmit 
            });

            clockOutForm.ShowDialog();
        }

        private void BtnRequestLeave_Click(object sender, EventArgs e)
        {
            // Create a custom form for Leave Request
            Form leaveRequestForm = new Form
            {
                Text = "Leave Request",
                Size = new Size(400, 400),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false
            };

            // Employee ID input
            Label lblEmployeeID = new Label
            {
                Text = "Employee ID:",
                Location = new Point(20, 20),
                Size = new Size(100, 25)
            };
            TextBox txtEmployeeID = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(200, 25)
            };

            // Employee Name input
            Label lblEmployeeName = new Label
            {
                Text = "Employee Name:",
                Location = new Point(20, 60),
                Size = new Size(100, 25)
            };
            TextBox txtEmployeeName = new TextBox
            {
                Location = new Point(150, 60),
                Size = new Size(200, 25)
            };

            // Leave Type dropdown
            Label lblLeaveType = new Label
            {
                Text = "Leave Type:",
                Location = new Point(20, 100),
                Size = new Size(100, 25)
            };
            ComboBox cbLeaveType = new ComboBox
            {
                Location = new Point(150, 100),
                Size = new Size(200, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            // Add leave type options
            cbLeaveType.Items.AddRange(new string[] 
            { 
                "Annual", 
                "Sick", 
                "Maternity", 
                "Paternity", 
                "Unpaid", 
                "Bereavement" 
            });

            // Start Date input
            Label lblStartDate = new Label
            {
                Text = "Start Date:",
                Location = new Point(20, 140),
                Size = new Size(100, 25)
            };
            DateTimePicker dtStartDate = new DateTimePicker
            {
                Location = new Point(150, 140),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short
            };

            // End Date input
            Label lblEndDate = new Label
            {
                Text = "End Date:",
                Location = new Point(20, 180),
                Size = new Size(100, 25)
            };
            DateTimePicker dtEndDate = new DateTimePicker
            {
                Location = new Point(150, 180),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short
            };

            // Submit button
            Button btnSubmit = new Button
            {
                Text = "Submit Leave Request",
                Location = new Point(100, 250),
                Size = new Size(200, 30)
            };
            btnSubmit.Click += (s, ev) => 
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(txtEmployeeID.Text) || 
                    string.IsNullOrWhiteSpace(txtEmployeeName.Text) ||
                    cbLeaveType.SelectedItem == null)
                {
                    MessageBox.Show("Please fill in all required fields.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Success message
                MessageBox.Show(
                    $"Leave Request Submitted:\n" +
                    $"Employee Name: {txtEmployeeName.Text} (ID: {txtEmployeeID.Text})\n" +
                    $"Leave Type: {cbLeaveType.SelectedItem}\n" +
                    $"Start Date: {dtStartDate.Value.ToShortDateString()}\n" +
                    $"End Date: {dtEndDate.Value.ToShortDateString()}", 
                    "Leave Request", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Information);
                
                leaveRequestForm.Close();
            };

            // Add controls to form
            leaveRequestForm.Controls.AddRange(new Control[] 
            { 
                lblEmployeeID, txtEmployeeID, 
                lblEmployeeName, txtEmployeeName,
                lblLeaveType, cbLeaveType,
                lblStartDate, dtStartDate,
                lblEndDate, dtEndDate,
                btnSubmit 
            });

            leaveRequestForm.ShowDialog();
        }
    }
}