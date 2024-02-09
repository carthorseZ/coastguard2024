namespace Admintools
{
    partial class RestoreDatabase
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
            this.restoreFName = new System.Windows.Forms.TextBox();
            this.Browse = new System.Windows.Forms.Button();
            this.StartRestore = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.AppPools = new System.Windows.Forms.ComboBox();
            this.Stop = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // restoreFName
            // 
            this.restoreFName.Enabled = false;
            this.restoreFName.Location = new System.Drawing.Point(8, 65);
            this.restoreFName.Name = "restoreFName";
            this.restoreFName.Size = new System.Drawing.Size(181, 20);
            this.restoreFName.TabIndex = 0;
            this.restoreFName.TextChanged += new System.EventHandler(this.restoreFName_TextChanged);
            // 
            // Browse
            // 
            this.Browse.Location = new System.Drawing.Point(200, 62);
            this.Browse.Name = "Browse";
            this.Browse.Size = new System.Drawing.Size(75, 23);
            this.Browse.TabIndex = 1;
            this.Browse.Text = "Browse";
            this.Browse.UseVisualStyleBackColor = true;
            this.Browse.Click += new System.EventHandler(this.Browse_Click);
            // 
            // StartRestore
            // 
            this.StartRestore.Enabled = false;
            this.StartRestore.Location = new System.Drawing.Point(8, 91);
            this.StartRestore.Name = "StartRestore";
            this.StartRestore.Size = new System.Drawing.Size(267, 23);
            this.StartRestore.TabIndex = 2;
            this.StartRestore.Text = "Start Restore";
            this.StartRestore.UseVisualStyleBackColor = true;
            this.StartRestore.Click += new System.EventHandler(this.StartRestore_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // AppPools
            // 
            this.AppPools.FormattingEnabled = true;
            this.AppPools.Location = new System.Drawing.Point(4, 16);
            this.AppPools.Name = "AppPools";
            this.AppPools.Size = new System.Drawing.Size(176, 21);
            this.AppPools.TabIndex = 3;
            this.AppPools.SelectedIndexChanged += new System.EventHandler(this.AppPools_SelectedIndexChanged);
            // 
            // Stop
            // 
            this.Stop.Location = new System.Drawing.Point(192, 14);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(75, 23);
            this.Stop.TabIndex = 4;
            this.Stop.Text = "Stop";
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AppPools);
            this.groupBox1.Controls.Add(this.Stop);
            this.groupBox1.Location = new System.Drawing.Point(8, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 56);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Application Pools";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // RestoreDatabase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 126);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.StartRestore);
            this.Controls.Add(this.Browse);
            this.Controls.Add(this.restoreFName);
            this.Name = "RestoreDatabase";
            this.Text = "Restore Database";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox restoreFName;
        private System.Windows.Forms.Button Browse;
        private System.Windows.Forms.Button StartRestore;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox AppPools;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}