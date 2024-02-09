using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;
using DBInterface;
using System.DirectoryServices;
namespace Admintools
{
    
    public partial class RestoreDatabase : Form
    {
        private DBIInterface DBI; 
        public RestoreDatabase(string connstring)
        {
            DBI = new DBIInterface(connstring);
            InitializeComponent();
            getAppPools();
        }


        private void getAppPools()
        {
            string machine = "localhost";
            using (DirectoryEntry appPoolsEntry = new DirectoryEntry(string.Format("IIS://{0}/W3SVC/AppPools", machine)))
            {
                foreach (DirectoryEntry childEntry in appPoolsEntry.Children)
                {
                    this.AppPools.Items.Add(childEntry.Name);
                 
                }
            }
            

        }


        private void Browse_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "Database Backup files (*.bak)|*.bak";



            //Set the starting directory and the title. 

            openFileDialog1.InitialDirectory = "C:";
            openFileDialog1.Title = "Select a Backup Destination";


            openFileDialog1.ShowDialog();
            this.restoreFName.Text = openFileDialog1.FileName;
        }

        private void StartRestore_Click(object sender, EventArgs e)
        {
            DBI.RestoreDatabase(this.restoreFName.Text);
            MessageBox.Show("Restore completed");
            this.Close();
        }

        private void restoreFName_TextChanged(object sender, EventArgs e)
        {
            this.StartRestore.Enabled = false;
            try
            {
                var FI = new System.IO.FileInfo(this.restoreFName.Text);
                if (FI.Exists)
                {
                    this.StartRestore.Enabled = true;
                }
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void AppPools_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
