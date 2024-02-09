using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using DBInterface;
namespace Admintools
{
    public partial class test : Form
    {
  //      Utils.Logger Logger = new Utils.Logger(typeof(test));
  //      List<CoastGuardMember> Skippers;
  //      List<CoastGuardMember> CrewList;
        public test()
        {
            /*
            InitializeComponent();
            using (DBIInterface DBI = new DBIInterface()){
                
                Skippers = DBI.getSkippers();
                CrewList = DBI.getCrew();
            }
            Skipper.Items.AddRange((from s in Skippers select s.First_Name +' '+s.Last_Name).ToArray());
            crew.Items.AddRange((from s in CrewList select s.First_Name + ' ' + s.Last_Name).ToArray());
             */
        }
        
        private void Skipper_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void crew_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //BackEnd.CGBE BE = new BackEnd.CGBE();

            string[] test = { "test" };
            //var t = BE.GetActivities(0, DateTime.Now.AddDays(-30), DateTime.Now, "", 50);

        //    var temp = "";
        
        }

    }
}
