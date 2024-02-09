using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Admintools
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();
            if (Properties.Settings.Default.UseIntegratedSecurity)
            {
                dbconnstring = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;",Properties.Settings.Default.DatabaseServer,Properties.Settings.Default.Database);
            }
            else
            {
                dbconnstring = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", Properties.Settings.Default.DatabaseServer, Properties.Settings.Default.Database,Properties.Settings.Default.UserName,Properties.Settings.Default.Password);
            }
        }
        public string dbconnstring;
        private void TideImport_Click(object sender, EventArgs e)
        {
            TideImport TI = new TideImport();
            TI.Show();
        }

        private void PortImport_Click(object sender, EventArgs e)
        {
            PortImport PI = new PortImport();
            PI.Show();
        }

        private void Test_Click(object sender, EventArgs e)
        {
            test t = new test();
            t.Show();
        }

        private void Admin_Load(object sender, EventArgs e)
        {

        }

        private void Backup_Click(object sender, EventArgs e)
        {
            BackupDatabase bdb = new BackupDatabase(dbconnstring);
            bdb.Show();
        }

        private void Restore_Click(object sender, EventArgs e)
        {
            string connstr;
            if (Properties.Settings.Default.UseIntegratedSecurity)
            {
                connstr = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;", Properties.Settings.Default.DatabaseServer, "master");
            }
            else
            {
                connstr = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};", Properties.Settings.Default.DatabaseServer, "master", Properties.Settings.Default.UserName, Properties.Settings.Default.Password);
            }
            RestoreDatabase rdb = new RestoreDatabase(connstr);
            rdb.Show();
            
        }



    }
}
