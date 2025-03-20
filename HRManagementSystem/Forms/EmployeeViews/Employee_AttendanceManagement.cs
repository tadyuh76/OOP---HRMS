namespace HRManagementSystem
{
    public class Employee_AttendanceView : Form
    {
        public Employee_AttendanceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee View - Attendance & Leave";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = "My Attendance & Leave Requests",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label info = new Label
            {
                Text = "This is the employee view of the attendance module.\nEmployees can view their attendance records and submit leave requests.",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 70),
                AutoSize = true
            };

            this.Controls.Add(title);
            this.Controls.Add(info);
        }
    }
}
