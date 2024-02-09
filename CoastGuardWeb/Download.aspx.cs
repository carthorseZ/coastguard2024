using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Threading;
using System.DirectoryServices;
using System.Reflection;
namespace CoastGuardWeb
{
    public partial class Download : System.Web.UI.Page
    {

    private static readonly Utils.Logger Logger = new Utils.Logger(typeof(Download));
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            string filename = Request["file"].ToString();
            var myPath = CoastGuardWeb.Properties.Settings.Default.UploadDir;
            if (!string.IsNullOrEmpty(Request["path"].ToString()))
            {
                myPath = Request["path"].ToString();
            }



            fileDownload(filename, System.Web.Hosting.HostingEnvironment.MapPath("~")+@"\"+myPath +@"\" + filename);
        }
        catch (Exception ex)
        {
            Logger.ErrorEx(ex, "");
        }
    }


    private static string SelectMimEType(FileInfo inFile)
    {
        switch (inFile.Extension.ToLower())
                {
                    case ".pdf":
                        return "application/pdf";
                        //break;
                    case ".docx":
                        return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        //break;
                    case ".doc":
                        return "application/msword";
                        //break;

                    default:
                   return "application/octet-stream";
                    //break;
                }
    }
    private static string IISMimeType(FileInfo inFile)
    {
        string mime = "application/octetstream";
        string ext = inFile.Extension.ToLower();
        string ServerName = "LocalHost";
        // Define the path to the metabase
        string MetabasePath = "IIS://" + ServerName + "/MimeMap";

        try
        {
            DirectoryEntry MimeMap = new DirectoryEntry(MetabasePath);
            // Get the Mime Types as a collection
            PropertyValueCollection pvc = MimeMap.Properties["MimeMap"];
            Type t = pvc[0].GetType(); 
            foreach (object property in pvc) { 
                BindingFlags f = BindingFlags.GetProperty;
                if (t.InvokeMember("Extension", f, null, property, null) as String == ext)
                {
                    
                    mime = t.InvokeMember("MimeType", f, null, property, null) as String;
                }
            } 
            
        }
        catch (Exception ex)
        {
            Logger.ErrorEx(ex, "");
        }
        return mime;
    }

    private static string RegistryMimeType(FileInfo inFile)
    {
        string mime = "application/octetstream";
        string ext = inFile.Extension.ToLower();
        Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
        if (rk != null && rk.GetValue("Content Type") != null)
            mime = rk.GetValue("Content Type").ToString();
        return mime;
    }

    private void fileDownload(string fileName, string fileUrl)
    {
        Page.Response.Clear();
        bool success = ResponseFile(Page.Request, Page.Response, fileName, fileUrl, 1024000);
        if (!success)
            Response.Write("Downloading Error!");
        //Page.Response.End();
        HttpContext.Current.ApplicationInstance.CompleteRequest();

    }
    
        
    public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
    {
        try
        {
            Logger.Debug("Downloading file [{0}]", _fullPath);
            FileInfo myUploadFileInfo = new FileInfo(_fullPath);
            FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                    _Response.ContentType = IISMimeType(myUploadFileInfo);
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
                _Response.AddHeader("Content-Disposition", "inline;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));
                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                for (int i = 0; i < maxCount; i++)
                {
                    if (_Response.IsClientConnected)
                    {
                        _Response.BinaryWrite(br.ReadBytes(pack));
                        Thread.Sleep(sleep);
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
            catch (ThreadAbortException tae)
            {
                Logger.Info("Load canceled due to [{0}] Inner [{1}]",tae.Message,tae.InnerException,tae.ExceptionState);
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
    }
}
