namespace HRManagementSystem
{
    public class Employee_DepartmentView : Form
    {
        public Employee_DepartmentView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee View - Department Information";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = "Department Information",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label info = new Label
            {
                Text = "This is the employee view of the department module.\nEmployees can view information about their department and colleagues.",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 70),
                AutoSize = true
            };

            this.Controls.Add(title);
            this.Controls.Add(info);
        }
    }
}
