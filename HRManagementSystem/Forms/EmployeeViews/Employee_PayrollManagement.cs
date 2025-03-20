namespace HRManagementSystem
{
    public class Employee_PayrollView : Form
    {
        public Employee_PayrollView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Employee View - My Payroll";
            this.Size = new Size(800, 600);
            this.BackColor = Color.White;

            Label title = new Label
            {
                Text = "My Salary Information",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label info = new Label
            {
                Text = "This is the employee view of the payroll module.\nEmployees can view their salary slips and payment history.",
                Font = new Font("Segoe UI", 12),
                Location = new Point(20, 70),
                AutoSize = true
            };

            this.Controls.Add(title);
            this.Controls.Add(info);
        }
    }
}
