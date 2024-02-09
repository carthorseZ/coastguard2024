namespace Admintools
{
    partial class PortImport
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
            this.Import = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.filename = new System.Windows.Forms.TextBox();
            this.fd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.updateLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Import
            // 
            this.Import.Location = new System.Drawing.Point(12, 32);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(268, 23);
            this.Import.TabIndex = 4;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File ...";
            // 
            // filename
            // 
            this.filename.Location = new System.Drawing.Point(69, 6);
            this.filename.Name = "filename";
            this.filename.Size = new System.Drawing.Size(171, 20);
            this.filename.TabIndex = 6;
            // 
            // fd
            // 
            this.fd.Location = new System.Drawing.Point(249, 4);
            this.fd.Name = "fd";
            this.fd.Size = new System.Drawing.Size(31, 23);
            this.fd.TabIndex = 7;
            this.fd.Text = "...";
            this.fd.UseVisualStyleBackColor = true;
            this.fd.Click += new System.EventHandler(this.fd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Currently Updating:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // updateLabel
            // 
            this.updateLabel.AutoSize = true;
            this.updateLabel.Location = new System.Drawing.Point(109, 68);
            this.updateLabel.Name = "updateLabel";
            this.updateLabel.Size = new System.Drawing.Size(0, 13);
            this.updateLabel.TabIndex = 10;
            // 
            // TideImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 90);
            this.Controls.Add(this.updateLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.fd);
            this.Controls.Add(this.filename);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Import);
            this.Name = "TideImport";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Import;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox filename;
        private System.Windows.Forms.Button fd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label updateLabel;
    }
}