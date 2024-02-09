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
namespace Admintools
{
    
    public partial class TideImport : Form
    {
        private static readonly Logger Logger = new Logger(typeof(TideImport));
        private DBIInterface DBI = new DBIInterface();
        public TideImport()
        {
            InitializeComponent();
            try
            {
                foreach (ddPair d in DBI.getDropDown("primaryport"))
                {
                    port.Items.Add(d);
                }
                //Year.Items.Add(System.DateTime.Now.Year - 2);
                //Year.Items.Add(System.DateTime.Now.Year - 1);
                //Year.Items.Add(System.DateTime.Now.Year);
                //Year.Items.Add(System.DateTime.Now.Year +1);
                //Year.Items.Add(System.DateTime.Now.Year +2);

            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "TideImport Top Level");
            }
        }

        
        private void fd_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter.Insert(0, "*.csv");
           
            //ofd.AddExtension("csv");
            ofd.ShowDialog();
            filename.Text = ofd.FileName;
        }

        private void port_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Import_Click(object sender, EventArgs e)
        {
            UpdateNextPrev.Enabled = false;
            Import.Enabled = false;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(filename.Text))
            {
                
                while (!sr.EndOfStream) {
                    string s = sr.ReadLine();
                    var ts =s.Split(',');
                    if (ts.Length==10||ts.Length==12){

                    DBI.addTideTimes(port.Text,ts);
                    this.updateLabel.Text = string.Format("{0}/{1}/{2}", ts[0], ts[2], ts[3]);
                    this.Refresh();
                    }
                }
            };
            
            UpdateNextPrev.Enabled = true;
            Import.Enabled = true;
            MessageBox.Show("Import Completed");
        }

        private void UpdateNextPrev_Click(object sender, EventArgs e)
        {
            UpdateNextPrev.Enabled = false;
            Import.Enabled = false;
            //DBI.updateNextPrev(updateLabel,this);
            UpdateNextPrev.Enabled = true;
            Import.Enabled = true;
            MessageBox.Show("Update Completed");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }




    }
}
