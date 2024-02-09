using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BackEnd;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using mshtml;
namespace CoastGuardWeb
{
    public partial class Upload : System.Web.UI.Page
    {
        private static readonly Utils.Logger Logger = new Utils.Logger(typeof(Upload));

 

        protected void Page_Load(object sender, EventArgs e)
        {

            if (this.IsPostBack)
            {
                

                // Uploading file
                
                UploadInfo uploadInfo = this.Session["UploadInfo"] as UploadInfo;
                if (uploadInfo == null)
                {
                    
                    //  let the parent page know we have processed the uplaod
                    //const string js = "window.('error', 'Unable to upload file.  Please refresh the page and try again');";
                    //ScriptManager.RegisterStartupScript(this, typeof(Upload), "progress", js, true);
                }
                else
                {
                    //  let the webservice know that we are not yet ready
                    uploadInfo.IsReady = false;
                    
                    //  do some basic validation
                    if (this.FileUpload.PostedFile != null && this.FileUpload.PostedFile.ContentLength > 0
                        
                        && this.FileUpload.PostedFile.ContentLength < (CoastGuardWeb.Properties.Settings.Default.MaxFileUploadSize*1024) ){


                            var myPath = CoastGuardWeb.Properties.Settings.Default.UploadDir;
                            if (!String.IsNullOrEmpty(Request.QueryString["path"]))
                            {
                                myPath = Request.QueryString["path"];
                            }
                        //  build the local path
                            string path = System.Web.Hosting.HostingEnvironment.MapPath("~") +"\\"+ (myPath);
                        string fileName = Path.GetFileName(this.FileUpload.PostedFile.FileName);
                        if (uploadInfo.Complete && uploadInfo.FileName.StartsWith(fileName)) return; 
                        //  build the strucutre and stuff it into session
                        uploadInfo.ContentLength = this.FileUpload.PostedFile.ContentLength;
                        uploadInfo.FileName = fileName;
                        uploadInfo.UploadedLength = 0;

                        //  todo: execute any special logic if the file already exists ...

                        //  let the polling process know that we are done initializing ...
                        uploadInfo.IsReady = true;

                        //  DEMOWARE: set the buffer size to something larger.
                        //  the smaller the buffer the longer it will take to
                        //  download, but the more precise your progress bar will be.
                        //  to large of a value and the progress bar will make real large jumps
                        int bufferSize = 1*1024;
                        byte[] buffer = new byte[bufferSize];
                        using (DBInterface.DBIInterface DBI = new DBInterface.DBIInterface())
                        {
                            //  write the byte to disk
                            var myUploadStatus = new DBInterface.FileUploadStatus();
                            myUploadStatus.FileName = uploadInfo.FileName;
                            myUploadStatus.BytesUploaded = 0;
                            myUploadStatus.DateUploadStart = System.DateTime.Now;
                            myUploadStatus.TotalBytes = uploadInfo.ContentLength;
                            DBI.ModifyFileUploadStatus(myUploadStatus);
                            using (FileStream fs = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                            {
                                //  aslong was we haven't written everything ...
                                while (uploadInfo.UploadedLength < uploadInfo.ContentLength)
                                {
                                    //  fill the buffer from the input stream
                                    int bytes = this.FileUpload.PostedFile.InputStream.Read(buffer, 0, bufferSize);
                                    //  write the bytes to the file stream
                                    fs.Write(buffer, 0, bytes);
                                    //  update the number the webservice is polling on
                                    uploadInfo.UploadedLength += bytes;
                                    myUploadStatus.BytesUploaded += bytes;
                                    DBI.ModifyFileUploadStatus(myUploadStatus);
                                    DBI.AddEvent(0, "FileUpload", uploadInfo);


                                }
                                uploadInfo.Complete = true;
                            }
                        }
                  
                        //  let the parent page know we have processed the uplaod
                        
                    }
                    else
                    {
                        //  DEMOWARE: I want to restrict uploads on the demo site to 250 bytes
                        
                    }

                    //  let the webservice know that we are not yet ready
                    uploadInfo.IsReady = false;
                }
            }
            else
            {
                this.Session["UploadInfo"] = new UploadInfo { IsReady = false, Complete= false};
            }        
        

        }

    }
}
