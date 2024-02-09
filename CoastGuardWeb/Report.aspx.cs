using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBInterface;
using System.Data;
using cgset = CoastGuardWeb.Properties.Settings;
namespace CoastGuardWeb.Reporting
{

    public partial class Report : System.Web.UI.Page
    {
        private static readonly Utils.Logger Logger = new Utils.Logger(typeof(Report));
        private CrystalDecisions.CrystalReports.Engine.ReportDocument _reportDoc;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Debug("Report Page Load sid[{0}] Filename [{1}]", Request.QueryString["SessionId"], Request.QueryString["filename"]);
                if (string.IsNullOrEmpty(Request.QueryString["SessionId"]))
                {
                    Response.Redirect("../login.htm");
                }
                if (!string.IsNullOrEmpty(Request.QueryString["filename"]))
                {
                    var fname = Request.QueryString["filename"];
                    if (!fname.ToLower().EndsWith(".rpt"))
                    {
                        fname = fname + ".rpt";
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["SelectionCriteria"]))
                    {
                        var selCriteria = Request.QueryString["SelectionCriteria"];
                        Logger.Debug("{0} Calling Report {1} with criteria [{2}]", Request.QueryString["SessionId"], Request.QueryString["filename"], selCriteria);
                        //generateReport(fname, selCriteria);
                        generateReportUsingDataSet(fname, selCriteria);
                    }
                    else
                    {
                        //generateReport(fname);
                        if (!string.IsNullOrEmpty(Request.QueryString["Views"]))
                        {
                            var myViews = Request.QueryString["Views"];
                            Logger.Debug("{0} Calling Report {1} with criteria [{2}]", Request.QueryString["SessionId"], Request.QueryString["filename"],"1=1");
                            generateReportUsingDataSet(fname, "1=1", myViews);
                        }
                        else
                        {
                            generateReportUsingDataSet(fname, "1=1");
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }

        }
        public override void Dispose()
        {
            if (_reportDoc != null)
            {
                _reportDoc.Close();
                _reportDoc.Dispose();
            }
            //if (reportViewer != null)
            //{
            //    reportViewer.Dispose();
            //}
        }


        private void generateReport(string fname)
        {
            generateReport(fname, "");
        }
        private void generateReport(string fname, string RecordSelectFormula)
        {

            Logger.Debug("");
            try
            {
                CrystalDecisions.Shared.TableLogOnInfo tInfo = new CrystalDecisions.Shared.TableLogOnInfo();

                //tInfo.ConnectionInfo.DatabaseName = "coastguard";
                tInfo.ConnectionInfo.ServerName = CoastGuardWeb.Properties.Settings.Default.DatabaseHostName;
                tInfo.ConnectionInfo.Password = CoastGuardWeb.Properties.Settings.Default.DatabasePassword;
                tInfo.ConnectionInfo.DatabaseName = CoastGuardWeb.Properties.Settings.Default.DatabaseName;
                tInfo.ConnectionInfo.UserID = CoastGuardWeb.Properties.Settings.Default.DatabaseUserID;
                //tInfo.ConnectionInfo.AllowCustomConnection = true;
                //tInfo.ConnectionInfo.Type = CrystalDecisions.Shared.ConnectionInfoType.SQL;
                Logger.Info("DatabaseName [{0}] ServerName[{1}] Userid[{2}] Password[{3}]", tInfo.ConnectionInfo.ServerName, tInfo.ConnectionInfo.DatabaseName, tInfo.ConnectionInfo.UserID, tInfo.ConnectionInfo.EncodedPassword);
                //tInfo.ConnectionInfo.Password = "(0astGuard";
                //tInfo.ConnectionInfo.ServerName = "localhost";
                //tInfo.ConnectionInfo.UserID = "coastguard";
                _reportDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                Logger.Debug("Opening Report Location [{0}]", fname);
                fname = @"Reporting\Reports\" + fname;
                string reportLoc = Server.MapPath(fname);
                var reportFileInfo = new System.IO.FileInfo(reportLoc);

                Logger.Debug("Report Server Location [{0}]", reportLoc);
                if (reportFileInfo.Exists)
                {
                    _reportDoc.Load(reportLoc);
                }
                else
                {
                    Logger.Debug("Unable to find file [{0}]", fname);
                }

                foreach (CrystalDecisions.CrystalReports.Engine.Table t in _reportDoc.Database.Tables)
                {
                    //t.ApplyLogOnInfo(tInfo);
                 //   _reportDoc.Database.Tables[0].ApplyLogOnInfo(tInfo);
                }

                if (!string.IsNullOrEmpty(RecordSelectFormula))
                {
                    _reportDoc.RecordSelectionFormula = RecordSelectFormula;
                }

                reportViewer.ReportSource = _reportDoc;
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
        }
        private void generateReportUsingDataSet(string fname, string where)
        {
            generateReportUsingDataSet(fname, where,"vTripReport;ActivityPassangers");
        }

        private void generateReportUsingDataSet(string fname, string where, string vName)
        {
            var myQueries = new List<string>();
            //var vName = "vTripReport;ActivityPassangers";
            var sql = string.Format("select * from {0} where {1}", vName, where);
            foreach (string name in vName.Split(';'))
            {
                sql = string.Format("select * from {0} where {1}", name, where);
                myQueries.Add(sql);
            }

            Logger.Debug("");
            try
            {
                var connString = string.Format("Data Source={0};Initial Catalog=CoastGuard;Persist Security Info=True;User ID=coastguard;Password=(0astGuard",cgset.Default.DatabaseHostName);
                var tableid = 0;
                using (DBIInterface DBI = new DBIInterface(connString))
                {
                    DataSet DS = DBI.RunReport(myQueries);
                    var tempname = @"Reporting\Reports\" + vName+".xml";
                    string reportTempLoc = Server.MapPath(tempname);
                    //var reportFileInfo = new System.IO.FileInfo(reportTempLoc);
                    try
                    {
                        DS.WriteXml(reportTempLoc,XmlWriteMode.WriteSchema);
                    }
                    catch { }

                    if (DS != null && DS.Tables.Count > 0)
                    {
                        if (DS.Tables[0].Rows.Count > 0)
                        {

                            _reportDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                            Logger.Debug("Opening Report Location [{0}]", fname);
                            fname = @"Reporting\Reports\" + fname;
                            string reportLoc = Server.MapPath(fname);
                            var reportFileInfo = new System.IO.FileInfo(reportLoc);
                            if (reportFileInfo.Exists)
                            {
                                _reportDoc.Load(reportLoc);
                            }
                            else
                            {
                                Logger.Debug("Unable to find file [{0}]", fname);
                            }

                            //foreach (CrystalDecisions.CrystalReports.Engine.Table t in _reportDoc.Database.Tables)
                            //{
                                //t.ApplyLogOnInfo(tInfo);
                                //   _reportDoc.Database.Tables[0].ApplyLogOnInfo(tInfo);
                            //}
           
                            Logger.Debug("Report Loaded");
                            _reportDoc.SetDataSource(DS);
                          
                            Logger.Debug("Setting Datasource for sub reports");
                            Logger.Debug("Report Server Location [{0}]", reportLoc);
                           
                            reportViewer.ReportSource = _reportDoc;
                            foreach (CrystalDecisions.CrystalReports.Engine.ReportDocument s in _reportDoc.Subreports) {
                                tableid ++;
                                s.SetDataSource(DS.Tables[tableid]);
                            }
                           reportViewer.EnableParameterPrompt = true;
                            
                            this.reportViewer.DataBind();
                            //this.reportViewer.RefreshReport();

                            //_reportDoc.ReadRecords();
                            //readyToGenerateReport = true;
                            //Logger.Debug(string.Format("Report loaded: \"{0}\". Report path: {1}", report.Name, report.ReportFile));
                        }
                        else
                        {
                            Logger.Debug("No records found.");
                            //errMessage = "There is no report to display. Please try again with different date and time settings.";
                        }
                    }
                    else
                    {
                        Logger.Debug("No tables found.");
                        //errMessage = "There is no report to display. Please try again with different date and time settings.";
                    }


                    //foreach (CrystalDecisions.CrystalReports.Engine.Table t in _reportDoc.Database.Tables)
                    //{
                    //    t.ApplyLogOnInfo(tInfo);
                        //   _reportDoc.Database.Tables[0].ApplyLogOnInfo(tInfo);
                    //}

                    //if (!string.IsNullOrEmpty(RecordSelectFormula))
                    //{
                    //    _reportDoc.RecordSelectionFormula = RecordSelectFormula;
                    //}

                    reportViewer.ReportSource = _reportDoc;
                    // clear record selection formula
                    // This should be controled on a per report basis
                   // _reportDoc.RecordSelectionFormula = "";
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
        
        }
    }
}
