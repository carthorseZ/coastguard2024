namespace Admintools
{
    partial class BackupDatabase
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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.backupfile = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.StartBackup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // backupfile
            // 
            this.backupfile.Enabled = false;
            this.backupfile.Location = new System.Drawing.Point(34, 29);
            this.backupfile.Name = "backupfile";
            this.backupfile.Size = new System.Drawing.Size(145, 20);
            this.backupfile.TabIndex = 0;
            this.backupfile.TextChanged += new System.EventHandler(this.backupfile_TextChanged);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(185, 29);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // StartBackup
            // 
            this.StartBackup.Enabled = false;
            this.StartBackup.Location = new System.Drawing.Point(34, 56);
            this.StartBackup.Name = "StartBackup";
            this.StartBackup.Size = new System.Drawing.Size(226, 23);
            this.StartBackup.TabIndex = 2;
            this.StartBackup.Text = "Backup";
            this.StartBackup.UseVisualStyleBackColor = true;
            this.StartBackup.Click += new System.EventHandler(this.StartBackup_Click);
            // 
            // BackupDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 98);
            this.Controls.Add(this.StartBackup);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.backupfile);
            this.Name = "BackupDatabase";
            this.Text = "BackupDatabase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox backupfile;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button StartBackup;
    }
}