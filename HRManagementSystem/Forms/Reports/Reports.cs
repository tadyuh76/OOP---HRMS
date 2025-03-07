namespace HRManagementSystem
{
    public class Reports : Form
    {
        public Reports()
        {
            this.BackColor = Color.WhiteSmoke;
            this.Dock = DockStyle.Fill;

            Label lblTitle = new Label();
            lblTitle.Text = "HR Reports & Analytics";
            lblTitle.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTitle.Location = new Point(20, 20);
            lblTitle.AutoSize = true;

            this.Controls.Add(lblTitle);

            Label lblDescription = new Label();
            lblDescription.Text = "Generate HR reports and analytics";
            lblDescription.Font = new Font("Segoe UI", 10);
            lblDescription.Location = new Point(20, 60);
            lblDescription.AutoSize = true;

            this.Controls.Add(lblDescription);
        }
    }
}
