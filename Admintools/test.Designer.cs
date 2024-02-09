namespace Admintools
{
    partial class test
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
            this.Skipper = new System.Windows.Forms.ComboBox();
            this.crew = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Skipper
            // 
            this.Skipper.FormattingEnabled = true;
            this.Skipper.Location = new System.Drawing.Point(30, 40);
            this.Skipper.Name = "Skipper";
            this.Skipper.Size = new System.Drawing.Size(225, 21);
            this.Skipper.TabIndex = 0;
            this.Skipper.SelectedIndexChanged += new System.EventHandler(this.Skipper_SelectedIndexChanged);
            // 
            // crew
            // 
            this.crew.FormattingEnabled = true;
            this.crew.Location = new System.Drawing.Point(30, 68);
            this.crew.Name = "crew";
            this.crew.Size = new System.Drawing.Size(225, 21);
            this.crew.TabIndex = 1;
            this.crew.SelectedIndexChanged += new System.EventHandler(this.crew_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Get Trips";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // test
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.crew);
            this.Controls.Add(this.Skipper);
            this.Name = "test";
            this.Text = "test";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox Skipper;
        private System.Windows.Forms.ComboBox crew;
        private System.Windows.Forms.Button button1;
    }
}