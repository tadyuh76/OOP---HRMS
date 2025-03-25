using static System.Net.Mime.MediaTypeNames;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Xml.Linq;

namespace HRManagementSystem
{
    partial class Employee_PayrollView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panelTop = new Panel();
            lblTitle = new Label();
            label1 = new Label();
            cboEmployee = new ComboBox();
            panelButtons = new Panel();
            btnSearch = new Button();
            dgvPayrolls = new DataGridView();
            panelBottom = new Panel();
            lblSalaryRange = new Label();
            lblAverageSalary = new Label();
            lblTotalPayroll = new Label();
            panelTop.SuspendLayout();
            panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPayrolls).BeginInit();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.BackColor = Color.White;
            panelTop.Controls.Add(lblTitle);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Margin = new Padding(4, 6, 4, 6);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(1075, 105);
            panelTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.Location = new Point(472, 29);
            lblTitle.Margin = new Padding(4, 0, 4, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(192, 39);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "My Payslip";
            lblTitle.TextAlign = ContentAlignment.TopCenter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(46, 26);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(104, 28);
            label1.TabIndex = 2;
            label1.Text = "Employee";
            // 
            // cboEmployee
            // 
            cboEmployee.FormattingEnabled = true;
            cboEmployee.Location = new Point(241, 26);
            cboEmployee.Margin = new Padding(4);
            cboEmployee.Name = "cboEmployee";
            cboEmployee.Size = new Size(265, 36);
            cboEmployee.TabIndex = 1;
            // 
            // panelButtons
            // 
            panelButtons.BackColor = Color.White;
            panelButtons.Controls.Add(label1);
            panelButtons.Controls.Add(btnSearch);
            panelButtons.Controls.Add(cboEmployee);
            panelButtons.Dock = DockStyle.Top;
            panelButtons.Location = new Point(0, 105);
            panelButtons.Margin = new Padding(4, 6, 4, 6);
            panelButtons.Name = "panelButtons";
            panelButtons.Size = new Size(1075, 87);
            panelButtons.TabIndex = 1;
            // 
            // btnSearch
            // 
            btnSearch.FlatStyle = FlatStyle.Popup;
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSearch.Location = new Point(874, 23);
            btnSearch.Margin = new Padding(4);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(129, 41);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Search";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // dgvPayrolls
            // 
            dgvPayrolls.AllowUserToAddRows = false;
            dgvPayrolls.AllowUserToDeleteRows = false;
            dgvPayrolls.BackgroundColor = Color.White;
            dgvPayrolls.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPayrolls.Dock = DockStyle.Fill;
            dgvPayrolls.Location = new Point(0, 192);
            dgvPayrolls.Margin = new Padding(4, 6, 4, 6);
            dgvPayrolls.MultiSelect = false;
            dgvPayrolls.Name = "dgvPayrolls";
            dgvPayrolls.ReadOnly = true;
            dgvPayrolls.RowHeadersWidth = 51;
            dgvPayrolls.RowTemplate.Height = 24;
            dgvPayrolls.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPayrolls.Size = new Size(1075, 439);
            dgvPayrolls.TabIndex = 2;
            // 
            // panelBottom
            // 
            panelBottom.BackColor = Color.White;
            panelBottom.Controls.Add(lblSalaryRange);
            panelBottom.Controls.Add(lblAverageSalary);
            panelBottom.Controls.Add(lblTotalPayroll);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 631);
            panelBottom.Margin = new Padding(4, 6, 4, 6);
            panelBottom.Name = "panelBottom";
            panelBottom.Size = new Size(1075, 87);
            panelBottom.TabIndex = 3;
            // 
            // lblSalaryRange
            // 
            lblSalaryRange.AutoSize = true;
            lblSalaryRange.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSalaryRange.Location = new Point(785, 27);
            lblSalaryRange.Margin = new Padding(4, 0, 4, 0);
            lblSalaryRange.Name = "lblSalaryRange";
            lblSalaryRange.Size = new Size(171, 28);
            lblSalaryRange.TabIndex = 2;
            lblSalaryRange.Text = "Min-Max: 0 - 0 $";
            // 
            // lblAverageSalary
            // 
            lblAverageSalary.AutoSize = true;
            lblAverageSalary.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblAverageSalary.ForeColor = Color.Black;
            lblAverageSalary.Location = new Point(427, 27);
            lblAverageSalary.Margin = new Padding(4, 0, 4, 0);
            lblAverageSalary.Name = "lblAverageSalary";
            lblAverageSalary.Size = new Size(131, 28);
            lblAverageSalary.TabIndex = 1;
            lblAverageSalary.Text = "Average: 0 $";
            // 
            // lblTotalPayroll
            // 
            lblTotalPayroll.AutoSize = true;
            lblTotalPayroll.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTotalPayroll.Location = new Point(61, 27);
            lblTotalPayroll.Margin = new Padding(4, 0, 4, 0);
            lblTotalPayroll.Name = "lblTotalPayroll";
            lblTotalPayroll.Size = new Size(100, 28);
            lblTotalPayroll.TabIndex = 0;
            lblTotalPayroll.Text = "Total: 0 $";
            // 
            // Employee_PayrollView
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1075, 718);
            Controls.Add(dgvPayrolls);
            Controls.Add(panelBottom);
            Controls.Add(panelButtons);
            Controls.Add(panelTop);
            Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Margin = new Padding(4, 6, 4, 6);
            Name = "Employee_PayrollView";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "My Payslip";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            panelButtons.ResumeLayout(false);
            panelButtons.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPayrolls).EndInit();
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.DataGridView dgvPayrolls;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblSalaryRange;
        private System.Windows.Forms.Label lblAverageSalary;
        private System.Windows.Forms.Label lblTotalPayroll;
        private Button btnSearch;
        private Label label1;
        private ComboBox cboEmployee;
    }
}
