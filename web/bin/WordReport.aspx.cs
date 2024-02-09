using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBInterface;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using cgset = CoastGuardWeb.Properties.Settings;
namespace CoastGuardWeb.Reporting
{

    public partial class WordReport : System.Web.UI.Page
    {
        private static readonly Utils.Logger Logger = new Utils.Logger(typeof(WordReport));
       

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Logger.Debug("Word Report Page Load sid[{0}] Filename [{1}]", Request.QueryString["SessionId"], Request.QueryString["filename"]);
                if (string.IsNullOrEmpty(Request.QueryString["SessionId"]))
                {
                    Response.Redirect("../login.htm");
                }
                if (!string.IsNullOrEmpty(Request.QueryString["filename"]))
                {
                    var fname = Request.QueryString["filename"];
                    if (!fname.ToLower().EndsWith(".docx"))
                    {
                        fname = fname + ".docx";
                    }
                    if (!string.IsNullOrEmpty(Request.QueryString["LogNo"]))
                    {
                        generateReport(fname,Request.QueryString["LogNo"]);
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

            //if (reportViewer != null)
            //{
            //    reportViewer.Dispose();
            //}
        }


        private void generateReport(string fname)
        {
            generateReport(fname, "-1");
        }
        private void generateReport(string fname, string LogNo)
        {

            Logger.Debug("");
            try
            {
                //Logger.Info("DatabaseName [{0}] ServerName[{1}] Userid[{2}] Password[{3}]", tInfo.ConnectionInfo.ServerName, tInfo.ConnectionInfo.DatabaseName, tInfo.ConnectionInfo.UserID, tInfo.ConnectionInfo.EncodedPassword);

                Logger.Debug("Opening Template Location [{0}]", fname);
                fname = @"Reporting\Templates\" + fname;
                string templateLoc = System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\" + (fname);
                var templateFileInfo = new System.IO.FileInfo(templateLoc);

                Logger.Debug("Report Server Location [{0}]", templateLoc);
                if (templateFileInfo.Exists)
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["LogNo"]))
                    {
                        var selCriteria = Request.QueryString["LogNo"];
                        Logger.Debug("{0} Calling Report {1} with criteria [{2}]", Request.QueryString["SessionId"], Request.QueryString["filename"], selCriteria);
                        CreateDocumentFromTemplate(templateLoc,fname,Convert.ToInt32(selCriteria)); 
                    }
                    
                }
                else
                {
                    Logger.Debug("Unable to find file [{0}]", fname);
                }




            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
        }

        internal void CreateDocumentFromTemplate(string TemplateFile,string outputName,int LogID)
        {
            //byte[] buffer = File.ReadAllBytes(Server.MapPath(TemplateFile));
            //DirectoryInfo TemplatePath = new DirectoryInfo(@"c:\Templates");
            //DirectoryInfo DocumentPath = new DirectoryInfo(@"c:\Documents");
            byte[] buffer = File.ReadAllBytes(TemplateFile);
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(buffer, 0, buffer.Length);


            buffer = null;



            using (WordprocessingDocument package =
    WordprocessingDocument.Open(memoryStream, true))
            {

                MainDocumentPart mainPart = package.MainDocumentPart;
                mainPart.DeleteParts(mainPart.CustomXmlParts);
                CustomXmlPart customXML = mainPart.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                //                string myXml = @"<Activities>
                //  <Activity id=""48155346-DEE9-4C83-99D7-082FA0EAB1DC"" Date=""2010-02-14T00:00:00"" DepartTime=""2011-02-27T11:30:00"" ReturnTime=""2011-02-27T14:30:00"" POB=""10"" Weather=""Cloud"" />
                //</Activities>";
                //                using (StreamWriter ts = new StreamWriter(customXML.GetStream()))
                //               {
                //                   ts.Write(myXml);
                //                }
                GetDataFromSQLServer(customXML.GetStream(), string.Format("Exec dbo.XMLActivities {0}",LogID));
            }
            fileDownload(memoryStream, outputName);
            //File.WriteAllBytes(DocumentPath.FullName + @"\" + DocumentName, memoryStream.ToArray());
        }
        private void fileDownload(Stream fileStream,string outputFName)
        {
            Page.Response.Clear();
            bool success = ResponseFile(Page.Request, Page.Response, fileStream, 1024000,outputFName);
            if (!success)
                Response.Write("Downloading Error!");
            //Page.Response.End();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, Stream myFile, long _speed,String OutputFname)
        {
            try
            {
                
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes
                    int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    using (new Utils.LoggingScope("IISMimeType"))
                    {
                        //_Response.ContentType = IISMimeType(myUploadFileInfo);
                        _Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    }
                    Logger.Debug("Response Content Type from IIS [{0}]", _Response.ContentType);
                    //using (new Utils.LoggingScope("Registry"))
                    //{
                    //    _Response.ContentType = (RegistryMimeType(myUploadFileInfo));
                    //}
                    //Logger.Debug("Request Content Type from Registry [{0}]", _Response.ContentType);
                    //using (new Utils.LoggingScope("IISSelectType"))
                    // {
                    //     _Response.ContentType = SelectMimEType(myUploadFileInfo);
                    // }
                    // Logger.Debug("Request Content Type from Select [{0}]", _Response.ContentType);


                    //_Response.ContentType = "application/pdf";
                    //_Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));
                    _Response.AddHeader("Content-Disposition", "inline;filename=" + HttpUtility.UrlEncode(OutputFname, System.Text.Encoding.UTF8));
                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            //Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                    _Response.Flush();
                    _Response.Clear();
                    //_Response.End();
                }

                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    


        private void GetDataFromSQLServer(Stream stream, string SQL)
        {
            //Connect to a Microsoft SQL Server database and get data
            String source = string.Format("Data Source={0};Initial Catalog={1};Integrated Security=SSPI;",Properties.Settings.Default.DatabaseHostName,Properties.Settings.Default.DatabaseName);
            string SqlStatement = string.Format(SQL);

            using (SqlConnection conn = new SqlConnection(source))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(SqlStatement, conn);
                var result = cmd.ExecuteXmlReader();
                result.Read();
                string data = @"<?xml version=""1.0"" standalone=""no""?>";
                //data += @"<root xmlns=""urn:Activities"">";
                data +=@"<Activities";
                data += @"xmlns=""http://tempuri.org/Activity.xsd""";
                data +=@"xmlns:mstns=""http://tempuri.org/Activity.xsd""";
                data +=@"xmlns:xs=""http://www.w3.org/2001/XMLSchema"">";

                
                while (!result.EOF)
                {
                    data += result.ReadOuterXml() + "\r";
                    result.Read();
                }
                data += "</root>";
                using (StreamWriter ts = new StreamWriter(stream))
                {
                    ts.Write(data);
                }
                conn.Close();
            }
        }
        
        
    }
}
