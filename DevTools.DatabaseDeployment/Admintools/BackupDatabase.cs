using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Utils;
using DBInterface;

namespace Admintools
{
    public partial class BackupDatabase : Form
    {
        private DBIInterface DBI;
        public BackupDatabase(string connstring)
        {
            InitializeComponent();
            DBI = new DBIInterface(connstring);
        }

        private void Browse_Click(object sender, EventArgs e)
        {

            //Now set the file type 

            saveFileDialog1.Filter = "Database Backup files (*.bak)|*.bak";



            //Set the starting directory and the title. 

            saveFileDialog1.InitialDirectory = "C:";
            saveFileDialog1.Title = "Select a Backup Destination"; 


            saveFileDialog1.ShowDialog();
            this.backupfile.Text = saveFileDialog1.FileName;

        }

        private void backupfile_TextChanged(object sender, EventArgs e)
        {
            this.StartBackup.Enabled = false;
            try
            {
                var FI = new System.IO.FileInfo(this.backupfile.Text);
                if (FI.Directory.Exists)
                {
                    this.StartBackup.Enabled = true;
                }
            }
            catch
            {
                
            }
        }

        private void StartBackup_Click(object sender, EventArgs e)
        {
            DBI.BackupDatase(this.backupfile.Text);
            MessageBox.Show("Back up completed");
            this.Close();
        }



    }
}
