using System.Text.Json;
using System.Drawing.Drawing2D;

namespace HRManagementSystem
{
    public class Employee_ProfileView : Form
    {
        // UI Controls
        private Label lblName, lblEmail, lblPhone, lblAddress, lblDOB, lblAge;
        private Label lblEmployeeID, lblPosition, lblDepartment, lblHireDate, lblYearsOfService, lblSalary;
        private TextBox txtPhone, txtEmail, txtAddress;
        private Button btnSave, btnCancel;
        private Panel personalInfoPanel, employmentInfoPanel, headerPanel;
        private PictureBox profilePicture;
        private Label lblUserName, lblUserTitle;
        private Panel mainContentPanel;
        private TableLayoutPanel buttonsPanel;

        // Color scheme
        private readonly Color primaryColor = Color.FromArgb(60, 141, 188);  // Blue
        private readonly Color accentColor = Color.FromArgb(0, 166, 90);     // Green
        private readonly Color lightGrayColor = Color.FromArgb(245, 245, 245);
        private readonly Color darkTextColor = Color.FromArgb(73, 80, 87);
        private readonly Color lightTextColor = Color.FromArgb(108, 117, 125);
        private readonly Color panelBackColor = Color.White;
        private readonly Color editFieldBackColor = Color.FromArgb(248, 249, 250);

        // Data fields
        private Employee currentEmployee;
        private string employeeDataPath = @"c:\Users\tadyuh\Coding Projects\hrms\HRManagementSystem\Data\Employees.json";

        // Assuming the logged-in employee ID is passed to the constructor
        public Employee_ProfileView(string employeeId = "EMP001") // Default for testing
        {
            InitializeComponent();
            LoadEmployeeData(employeeId);
        }

        private void InitializeComponent()
        {
            // Form properties
            Text = "Employee Profile";
            Size = new Size(900, 680);
            BackColor = lightGrayColor;
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            Padding = new Padding(15);

            // Create main content panel with shadow effect
            mainContentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = panelBackColor,
                Padding = new Padding(0),
                Margin = new Padding(0)
            };

            // Create header panel
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = primaryColor,
                Padding = new Padding(20)
            };

            // Profile picture (placeholder)
            profilePicture = new PictureBox
            {
                Size = new Size(80, 80),
                Location = new Point(20, 20),
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // Create a circle for the profile picture
            GraphicsPath gp = new();
            gp.AddEllipse(0, 0, profilePicture.Width - 1, profilePicture.Height - 1);
            profilePicture.Region = new Region(gp);

            // User name in header
            lblUserName = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 16),
                ForeColor = Color.White,
                Location = new Point(110, 30)
            };

            // User title in header
            lblUserTitle = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.FromArgb(220, 220, 220),
                Location = new Point(110, 65)
            };

            headerPanel.Controls.Add(profilePicture);
            headerPanel.Controls.Add(lblUserName);
            headerPanel.Controls.Add(lblUserTitle);

            // Content layout
            TableLayoutPanel contentLayout = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(20, 20, 20, 15),
                BackColor = panelBackColor
            };

            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            contentLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // Create Personal Information panel
            personalInfoPanel = CreateRoundedPanel();
            personalInfoPanel.Dock = DockStyle.Fill;
            personalInfoPanel.Padding = new Padding(15);
            personalInfoPanel.Margin = new Padding(5, 10, 10, 10);

            Label personalInfoTitle = new()
            {
                Text = "Personal Information",
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = darkTextColor,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };

            TableLayoutPanel personalInfoTable = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(0, 10, 0, 0)
            };

            personalInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            personalInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));

            for (int i = 0; i < 6; i++)
            {
                personalInfoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }

            // Labels
            lblName = CreateStyledLabel("Name:");
            lblEmail = CreateStyledLabel("Email:");
            lblPhone = CreateStyledLabel("Phone:");
            lblAddress = CreateStyledLabel("Address:");
            lblDOB = CreateStyledLabel("Date of Birth:");
            lblAge = CreateStyledLabel("Age:");

            // Text boxes with modern style
            txtEmail = CreateStyledTextBox();
            txtPhone = CreateStyledTextBox();
            txtAddress = CreateStyledTextBox();

            // Add field labels
            personalInfoTable.Controls.Add(lblName, 0, 0);
            personalInfoTable.Controls.Add(lblEmail, 0, 1);
            personalInfoTable.Controls.Add(lblPhone, 0, 2);
            personalInfoTable.Controls.Add(lblAddress, 0, 3);
            personalInfoTable.Controls.Add(lblDOB, 0, 4);
            personalInfoTable.Controls.Add(lblAge, 0, 5);

            // Add name value (read-only)
            Label lblNameValue = CreateValueLabel();
            personalInfoTable.Controls.Add(lblNameValue, 1, 0);

            // Add editable fields
            personalInfoTable.Controls.Add(txtEmail, 1, 1);
            personalInfoTable.Controls.Add(txtPhone, 1, 2);
            personalInfoTable.Controls.Add(txtAddress, 1, 3);

            // Add read-only fields
            Label lblDOBValue = CreateValueLabel();
            Label lblAgeValue = CreateValueLabel();
            personalInfoTable.Controls.Add(lblDOBValue, 1, 4);
            personalInfoTable.Controls.Add(lblAgeValue, 1, 5);

            personalInfoPanel.Controls.Add(personalInfoTable);
            personalInfoPanel.Controls.Add(personalInfoTitle);

            // Create Employment Information panel
            employmentInfoPanel = CreateRoundedPanel();
            employmentInfoPanel.Dock = DockStyle.Fill;
            employmentInfoPanel.Padding = new Padding(15);
            employmentInfoPanel.Margin = new Padding(10, 10, 5, 10);

            Label employmentInfoTitle = new()
            {
                Text = "Employment Information",
                Font = new Font("Segoe UI Semibold", 12),
                ForeColor = darkTextColor,
                Dock = DockStyle.Top,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft
            };

            TableLayoutPanel employmentInfoTable = new()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(0, 10, 0, 0)
            };

            employmentInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            employmentInfoTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));

            for (int i = 0; i < 6; i++)
            {
                employmentInfoTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }

            // Labels
            lblEmployeeID = CreateStyledLabel("Employee ID:");
            lblPosition = CreateStyledLabel("Position:");
            lblDepartment = CreateStyledLabel("Department:");
            lblHireDate = CreateStyledLabel("Hire Date:");
            lblYearsOfService = CreateStyledLabel("Years of Service:");
            lblSalary = CreateStyledLabel("Base Salary:");

            // Values (all read-only)
            Label lblEmployeeIDValue = CreateValueLabel();
            Label lblPositionValue = CreateValueLabel();
            Label lblDepartmentValue = CreateValueLabel();
            Label lblHireDateValue = CreateValueLabel();
            Label lblYearsOfServiceValue = CreateValueLabel();
            Label lblSalaryValue = CreateValueLabel();

            // Add to table
            employmentInfoTable.Controls.Add(lblEmployeeID, 0, 0);
            employmentInfoTable.Controls.Add(lblPosition, 0, 1);
            employmentInfoTable.Controls.Add(lblDepartment, 0, 2);
            employmentInfoTable.Controls.Add(lblHireDate, 0, 3);
            employmentInfoTable.Controls.Add(lblYearsOfService, 0, 4);
            employmentInfoTable.Controls.Add(lblSalary, 0, 5);

            employmentInfoTable.Controls.Add(lblEmployeeIDValue, 1, 0);
            employmentInfoTable.Controls.Add(lblPositionValue, 1, 1);
            employmentInfoTable.Controls.Add(lblDepartmentValue, 1, 2);
            employmentInfoTable.Controls.Add(lblHireDateValue, 1, 3);
            employmentInfoTable.Controls.Add(lblYearsOfServiceValue, 1, 4);
            employmentInfoTable.Controls.Add(lblSalaryValue, 1, 5);

            employmentInfoPanel.Controls.Add(employmentInfoTable);
            employmentInfoPanel.Controls.Add(employmentInfoTitle);

            // Add panels to content layout
            contentLayout.Controls.Add(personalInfoPanel, 0, 0);
            contentLayout.Controls.Add(employmentInfoPanel, 1, 0);

            // Create buttons panel
            buttonsPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = panelBackColor,
                Padding = new Padding(20, 10, 20, 10)
            };

            // Create buttons
            btnSave = CreateStyledButton("Save Changes", accentColor, Color.White);
            btnSave.Click += BtnSave_Click;

            btnCancel = CreateStyledButton("Cancel", Color.FromArgb(220, 220, 220), darkTextColor);
            btnCancel.Click += BtnCancel_Click;

            // Add buttons to panel
            buttonsPanel.ColumnCount = 3;
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
            buttonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));

            buttonsPanel.Controls.Add(new Label(), 0, 0); // Empty spacer
            buttonsPanel.Controls.Add(btnCancel, 1, 0);
            buttonsPanel.Controls.Add(btnSave, 2, 0);

            // Assemble the form
            mainContentPanel.Controls.Add(contentLayout);
            mainContentPanel.Controls.Add(headerPanel);
            mainContentPanel.Controls.Add(buttonsPanel);

            Controls.Add(mainContentPanel);

            // Store references to value labels for data loading
            lblNameValue.Name = "lblNameValue";
            lblDOBValue.Name = "lblDOBValue";
            lblAgeValue.Name = "lblAgeValue";
            lblEmployeeIDValue.Name = "lblEmployeeIDValue";
            lblPositionValue.Name = "lblPositionValue";
            lblDepartmentValue.Name = "lblDepartmentValue";
            lblHireDateValue.Name = "lblHireDateValue";
            lblYearsOfServiceValue.Name = "lblYearsOfServiceValue";
            lblSalaryValue.Name = "lblSalaryValue";
        }

        private Panel CreateRoundedPanel()
        {
            Panel panel = new()
            {
                BackColor = panelBackColor,
                BorderStyle = BorderStyle.None
            };

            // Add shadow effect (simulated with border)
            panel.Paint += (sender, e) =>
            {
                Rectangle rect = new(0, 0, panel.Width - 1, panel.Height - 1);
                using (Pen pen = new(Color.FromArgb(230, 230, 230), 1))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            };

            return panel;
        }

        private Label CreateStyledLabel(string text)
        {
            return new Label
            {
                Text = text,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = lightTextColor,
                Margin = new Padding(0, 0, 10, 0)
            };
        }

        private Label CreateValueLabel()
        {
            return new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Font = new Font("Segoe UI", 10),
                ForeColor = darkTextColor
            };
        }

        private TextBox CreateStyledTextBox()
        {
            TextBox textBox = new()
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = editFieldBackColor,
                Margin = new Padding(0, 8, 0, 8)
            };

            return textBox;
        }

        private Button CreateStyledButton(string text, Color backColor, Color foreColor)
        {
            Button button = new()
            {
                Text = text,
                BackColor = backColor,
                ForeColor = foreColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10),
                Size = new Size(120, 35),
                Dock = DockStyle.Fill,
                Margin = new Padding(5, 0, 5, 0),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void LoadEmployeeData(string employeeId)
        {
            try
            {
                string jsonData = File.ReadAllText(employeeDataPath);
                List<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(jsonData);

                currentEmployee = employees.FirstOrDefault(e => e.EmployeeId == employeeId);

                if (currentEmployee != null)
                {
                    // Header information
                    lblUserName.Text = currentEmployee.Name;
                    lblUserTitle.Text = currentEmployee.Position;

                    // Try to load a profile picture based on employee ID
                    string picturePath = $@"c:\Users\tadyuh\Coding Projects\hrms\HRManagementSystem\Resources\ProfilePictures\{currentEmployee.EmployeeId}.png";
                    if (File.Exists(picturePath))
                    {
                        profilePicture.Image = Image.FromFile(picturePath);
                    }
                    else
                    {
                        // Create an initial-based avatar
                        Bitmap avatarBitmap = CreateInitialsAvatar(currentEmployee.Name);
                        profilePicture.Image = avatarBitmap;
                    }

                    // Populate personal information
                    ((Label)personalInfoPanel.Controls.Find("lblNameValue", true)[0]).Text = currentEmployee.Name;
                    txtEmail.Text = currentEmployee.Email;
                    txtPhone.Text = currentEmployee.Phone;
                    txtAddress.Text = currentEmployee.Address;
                    ((Label)personalInfoPanel.Controls.Find("lblDOBValue", true)[0]).Text = currentEmployee.DateOfBirth.ToShortDateString();
                    ((Label)personalInfoPanel.Controls.Find("lblAgeValue", true)[0]).Text = currentEmployee.CalculateAge() + " years";

                    // Populate employment information
                    ((Label)employmentInfoPanel.Controls.Find("lblEmployeeIDValue", true)[0]).Text = currentEmployee.EmployeeId;
                    ((Label)employmentInfoPanel.Controls.Find("lblPositionValue", true)[0]).Text = currentEmployee.Position;
                    ((Label)employmentInfoPanel.Controls.Find("lblDepartmentValue", true)[0]).Text = currentEmployee.DepartmentId;
                    ((Label)employmentInfoPanel.Controls.Find("lblHireDateValue", true)[0]).Text = currentEmployee.HireDate.ToShortDateString();
                    ((Label)employmentInfoPanel.Controls.Find("lblYearsOfServiceValue", true)[0]).Text = currentEmployee.CalculateYearsOfService() + " years";
                    ((Label)employmentInfoPanel.Controls.Find("lblSalaryValue", true)[0]).Text = "$" + currentEmployee.BaseSalary.ToString("N2");
                }
                else
                {
                    MessageBox.Show("Employee not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employee data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Bitmap CreateInitialsAvatar(string name)
        {
            // Create a bitmap for the avatar
            Bitmap avatarBitmap = new(80, 80);

            // Get initials from name
            string[] nameParts = name.Split(' ');
            string initials = "";

            if (nameParts.Length > 0)
                initials += nameParts[0][0];
            if (nameParts.Length > 1)
                initials += nameParts[nameParts.Length - 1][0];

            initials = initials.ToUpper();

            // Create graphics object
            using (Graphics g = Graphics.FromImage(avatarBitmap))
            {
                // Fill background
                g.SmoothingMode = SmoothingMode.AntiAlias;
                using (SolidBrush brush = new(Color.FromArgb(60, 60, 60)))
                {
                    g.FillEllipse(brush, 0, 0, 79, 79);
                }

                // Draw initials
                using (Font font = new("Arial", 24, FontStyle.Bold))
                {
                    // Measure string to center it
                    SizeF textSize = g.MeasureString(initials, font);
                    float x = (80 - textSize.Width) / 2;
                    float y = (80 - textSize.Height) / 2;

                    // Draw initials
                    using (SolidBrush textBrush = new(Color.White))
                    {
                        g.DrawString(initials, font, textBrush, x, y);
                    }
                }
            }

            return avatarBitmap;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (currentEmployee != null)
            {
                try
                {
                    // Update employee information
                    currentEmployee.Email = txtEmail.Text;
                    currentEmployee.Phone = txtPhone.Text;
                    currentEmployee.Address = txtAddress.Text;

                    // Load all employees
                    string jsonData = File.ReadAllText(employeeDataPath);
                    List<Employee> employees = JsonSerializer.Deserialize<List<Employee>>(jsonData);

                    // Find and update the current employee in the list
                    int index = employees.FindIndex(e => e.EmployeeId == currentEmployee.EmployeeId);
                    if (index >= 0)
                    {
                        employees[index] = currentEmployee;

                        // Save back to file
                        JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
                        string updatedJson = JsonSerializer.Serialize(employees, options);
                        File.WriteAllText(employeeDataPath, updatedJson);

                        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving changes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            // Reload the original data
            LoadEmployeeData(currentEmployee.EmployeeId);
        }
    }
}
