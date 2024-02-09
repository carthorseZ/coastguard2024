namespace Admintools
{
    partial class Admin
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
            this.TideImportBut = new System.Windows.Forms.Button();
            this.PortImportButton = new System.Windows.Forms.Button();
            this.Test = new System.Windows.Forms.Button();
            this.Backup = new System.Windows.Forms.Button();
            this.Restore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TideImportBut
            // 
            this.TideImportBut.Location = new System.Drawing.Point(40, 22);
            this.TideImportBut.Name = "TideImportBut";
            this.TideImportBut.Size = new System.Drawing.Size(202, 23);
            this.TideImportBut.TabIndex = 0;
            this.TideImportBut.Text = "Tide Import";
            this.TideImportBut.UseVisualStyleBackColor = true;
            this.TideImportBut.Click += new System.EventHandler(this.TideImport_Click);
            // 
            // PortImportButton
            // 
            this.PortImportButton.Location = new System.Drawing.Point(40, 51);
            this.PortImportButton.Name = "PortImportButton";
            this.PortImportButton.Size = new System.Drawing.Size(202, 23);
            this.PortImportButton.TabIndex = 1;
            this.PortImportButton.Text = "Port Import";
            this.PortImportButton.UseVisualStyleBackColor = true;
            this.PortImportButton.Click += new System.EventHandler(this.PortImport_Click);
            // 
            // Test
            // 
            this.Test.Location = new System.Drawing.Point(40, 80);
            this.Test.Name = "Test";
            this.Test.Size = new System.Drawing.Size(202, 23);
            this.Test.TabIndex = 2;
            this.Test.Text = "Testing";
            this.Test.UseVisualStyleBackColor = true;
            this.Test.Click += new System.EventHandler(this.Test_Click);
            // 
            // Backup
            // 
            this.Backup.Location = new System.Drawing.Point(40, 109);
            this.Backup.Name = "Backup";
            this.Backup.Size = new System.Drawing.Size(202, 23);
            this.Backup.TabIndex = 3;
            this.Backup.Text = "Backup Database";
            this.Backup.UseVisualStyleBackColor = true;
            this.Backup.Click += new System.EventHandler(this.Backup_Click);
            // 
            // Restore
            // 
            this.Restore.Location = new System.Drawing.Point(40, 138);
            this.Restore.Name = "Restore";
            this.Restore.Size = new System.Drawing.Size(202, 23);
            this.Restore.TabIndex = 4;
            this.Restore.Text = "Restore Database";
            this.Restore.UseVisualStyleBackColor = true;
            this.Restore.Click += new System.EventHandler(this.Restore_Click);
            // 
            // Admin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 179);
            this.Controls.Add(this.Restore);
            this.Controls.Add(this.Backup);
            this.Controls.Add(this.Test);
            this.Controls.Add(this.PortImportButton);
            this.Controls.Add(this.TideImportBut);
            this.Name = "Admin";
            this.Text = "Coastguard Admintools";
            this.Load += new System.EventHandler(this.Admin_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TideImportBut;
        private System.Windows.Forms.Button PortImportButton;
        private System.Windows.Forms.Button Test;
        private System.Windows.Forms.Button Backup;
        private System.Windows.Forms.Button Restore;
    }
}

