namespace HRManagementSystem
{
    public class Employee_ProfileView : Form
    {
        public Employee_ProfileView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee View - My Profile";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = "Employee Profile View",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label info = new Label
            {
                Text = "This is the employee view of the profile management module.\nEmployees can view and update their own information here.",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 70),
                AutoSize = true
            };

            this.Controls.Add(title);
            this.Controls.Add(info);
        }
    }
}
