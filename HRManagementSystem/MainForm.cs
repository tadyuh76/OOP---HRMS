namespace HRManagementSystem
{
    public partial class MainForm : Form
    {
        private Form activePanel = null!;

        public MainForm()
        {
            InitializeComponent();
            CustomizeDesign();
            SetActiveTab(btnDashboard); // Set Dashboard as default active tab
            OpenChildForm(CreateDashboard()); // Create and open initial dashboard
        }

        // Create a dashboard with event subscription properly attached
        private DashboardOverview CreateDashboard()
        {
            var dashboard = new DashboardOverview();
            dashboard.ModuleAccessRequested += Dashboard_ModuleAccessRequested;
            return dashboard;
        }


        // Event handler for dashboard module access requests
        private void Dashboard_ModuleAccessRequested(object sender, ModuleAccessEventArgs e)
        {
            // Open the appropriate form based on module name
            switch (e.ModuleName)
            {
                case "Employees Management":
                    SetActiveTab(btnEmployees);
                    OpenChildForm(new EmployeeManagement());
                    break;
                case "Departments Management":
                    SetActiveTab(btnDepartments);
                    OpenChildForm(new DepartmentManagement());
                    break;
                case "Leave & Attendance":
                    SetActiveTab(btnAttendance);
                    OpenChildForm(new AttendanceManagement());
                    break;
                case "Payroll":
                    SetActiveTab(btnPayroll);
                    OpenChildForm(new PayrollManagement());
                    break;
                case "Performance":
                    SetActiveTab(btnPerformance);
                    OpenChildForm(new PerformanceManagement());
                    break;
                case "Reports":
                    SetActiveTab(btnReports);
                    OpenChildForm(new Reports());
                    break;
            }
        }

        private void CustomizeDesign()
        {
            this.BackColor = Color.WhiteSmoke;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = true;
            this.Text = "Human Resource Management System";
            this.Size = new Size(1200, 800);
        }

        // Unsubscribe from events when removing a form
        public void OpenChildForm(Form childForm)
        {
            if (activePanel != null)
            {
                // Unsubscribe from events if the active panel is a dashboard
                if (activePanel is DashboardOverview dashboard)
                {
                    dashboard.ModuleAccessRequested -= Dashboard_ModuleAccessRequested;
                }

                panelContent.Controls.Remove(activePanel);
                activePanel.Dispose(); // Important: properly dispose the form
            }

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            panelContent.Controls.Add(childForm);
            panelContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            activePanel = childForm;
        }


        private void SetActiveTab(Button selectedButton)
        {
            // Reset all buttons
            foreach (Control ctrl in panelNavigation.Controls)
            {
                if (ctrl is Button && ctrl != btnLogo && ctrl != btnRoleSwitcher)
                {
                    Button btn = (Button)ctrl;
                    btn.BackColor = ColorPalette.navBackground;
                    btn.ForeColor = Color.Black;
                    btn.FlatAppearance.BorderSize = 0;
                }
            }

            // Highlight selected button
            selectedButton.BackColor = ColorPalette.primaryColor;
            selectedButton.ForeColor = Color.White;
            selectedButton.FlatAppearance.BorderSize = 0;
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(CreateDashboard());
        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(new EmployeeManagement());
        }

        private void btnDepartments_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(new DepartmentManagement());
        }

        private void btnAttendance_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(new AttendanceManagement());
        }

        private void btnPayroll_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(new PayrollManagement());
        }

        private void btnPerformance_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(new PerformanceManagement());
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            SetActiveTab((Button)sender);
            OpenChildForm(new Reports());
        }

        private void btnRoleSwitcher_Click(object sender, EventArgs e)
        {
            ContextMenuStrip roleMenu = new ContextMenuStrip();
            roleMenu.Items.Add("Admin", null, (s, args) => SwitchRole("Admin"));
            roleMenu.Items.Add("Manager", null, (s, args) => SwitchRole("Manager"));
            roleMenu.Items.Add("User", null, (s, args) => SwitchRole("User"));

            roleMenu.Show(btnRoleSwitcher, new Point(0, btnRoleSwitcher.Height));
        }

        private void SwitchRole(string role)
        {
            btnRoleSwitcher.Text = role + " ▼";
            // In a real app, you would change permissions/access based on the role
            MessageBox.Show("Switched to " + role + " role", "Role Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
