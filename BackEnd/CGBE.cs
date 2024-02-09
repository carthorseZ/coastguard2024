using System;
using System.Collections.Generic;
using System.Web.Script.Serialization; 
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Reflection;
using DBInterface;
using Utils;
namespace BackEnd
{
    // NOTE: If you change the class name "CGBE" here, you must also update the reference to "CGBE" in App.config.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ErrorHandlerBehaviour]
    public class CGBE : ICGBE
    {
      private static readonly Logger Logger = new Logger(typeof(CGBE));
 
        [WebInvoke(
            Method = "POST",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
      
        public Int32 Login(string Username,string Password,string Unit)
        {
            Logger.Debug("BackEnd Login, Username[{0}],Password[{1}] Unit{2}", Username,Password,Unit);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Info("Connecting to database using connection string [{0}]", Properties.Settings.Default.ConnectionString);
                CoastGuardMember temp = DBI.getMemberInfo(Username, Password,Unit);
                if (temp == null)
                {
                    return -1;
                }
                else
                {
                    var t = DBI.newSession(temp);
                    return t;
                }
            }
        }
        [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public Session GetSession(int SessionId)
        {
            Logger.Debug("Getting Session, {0}", SessionId);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                 return DBI.getSessionInfo(SessionId) ;
                
                    
                
            }
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<WCFEvent> GetEvents(int SessionId)
        {
            Logger.Debug("Get Session, {0}", SessionId);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                //return DBI.getData<WCFEvent>("WCFEvents",string.Format("session={0}",SessionId),null);
                return DBI.getEvents(SessionId);


            }
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]

        public EngineStartHours GetEngineStartHours(int SessionId,Guid Vessel,int LogNo)
        {
            Logger.Debug("Get Session, {0}", SessionId);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                //return DBI.getData<WCFEvent>("WCFEvents",string.Format("session={0}",SessionId),null);
                return DBI.getEngineStartHours(SessionId,Vessel,LogNo);


            }
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<String> GetReports(int SessionId)
        {
            Logger.Debug("Getting Reports");
            
            var retval = new List<String>();
            try
            {                
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~") + @"\Reporting\Reports";
                Logger.Debug(" Checking [{0}] for reports", path);
                System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(path);
                foreach (System.IO.FileInfo fi in DI.GetFiles("*.rpt"))
                {
                    var name = fi.Name;
                    if (fi.Extension == ".rpt")
                    {
                        name = name.Substring(0, name.Length - 4);
                    }
                    retval.Add(name);
                }

                Logger.Debug("Get Session, {0}", SessionId);
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<String> GetBackups(int SessionId)
        {
            Logger.Debug("Getting backups");

            var retval = new List<String>();
            try
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~") + @"\Backups";
                Logger.Debug(" Checking [{0}] for Backups", path);
                System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(path);
                foreach (System.IO.FileInfo fi in DI.GetFiles("*.bak"))
                {
                    var name = fi.Name;
                    if (fi.Extension == ".bak")
                    {
                        name = name.Substring(0, name.Length - 4);
                    }
                    retval.Add(name);
                }

                Logger.Debug("Get Session, {0}", SessionId);
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
            return retval;
        }
        [WebInvoke(
       Method = "POST",
       BodyStyle = WebMessageBodyStyle.Wrapped,
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]
      
        public List<AlertInfo> GetAlerts(int SessionID)
        {
            Logger.Debug("Getting Alerts");

            var retval = new List<AlertInfo>();
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    Logger.Debug("[{0}] GetAlerts Calling DBI Get Alerts", SessionID);
                    foreach (Alert a in DBI.getAlerts(DBI.getUnit(SessionID)))
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(a.Tables) && !string.IsNullOrEmpty(a.Where))
                            {
                                if (string.IsNullOrEmpty(a.Fields)) a.Fields = "*";
                                var sql = string.Format("select {1} from {0} where {2}", a.Tables, a.Fields, a.Where);
                                Logger.Debug("Calling alert sql [{0}]",sql);
                                System.Data.DataSet ds = DBI.ProcessAlert(sql);
                                if (ds.Tables.Count > 0)
                                {
                                    foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                                    {
                                        AlertInfo ai = new AlertInfo();
                                        ai.AlertName = a.Name;
                                        ai.Level = DBI.getMessageName(a.Level);
                                        Logger.Debug("{1} Alert Text [{0}]", a.Text, SessionID);
                                        string[] tempArr= new string[dr.ItemArray.Length];
                                        var i = 0;
                                        foreach (Object o in dr.ItemArray)
                                        {
                                            tempArr[i] = o.ToString().Trim();
                                            i++;
                                        }
                                        
                                        ai.Text = string.Format(a.Text,tempArr);
                                        Logger.Debug("{1} Alert [{0}]", ai,SessionID);
                                        retval.Add(ai);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.ErrorEx(ex, "{1} Processing alert {0}", a.Name, SessionID);
                        }
                    }
                    //var retval = new List<Course>();
                    Logger.Debug("[{0}] GetAlerts result count ({1})", SessionID, retval.Count);
                    return retval;
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
            return retval;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean Logoff(int SessionId)
        {
            Logger.Debug("Logoff, {0}", SessionId);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                return DBI.Logoff(SessionId);
            }
        }
        
        [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public List<MemberInfo> GetSkippers(int SessionID)
        {
            List<MemberInfo> retval= new List<MemberInfo>();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetSkippers Calling DBI Get Skippers", SessionID);
                try
                {
                    var temp = DBI.getSkippers(SessionID);
                    foreach (CoastGuardMember cgm in temp) {
                        retval.Add(new MemberInfo(cgm));
                    }
                     
                }
                catch (Exception ex)
                {
                    Logger.Debug("An error has occured in getSkippers");
                    Logger.ErrorEx(ex, "Get Skippers");
                    retval = new List<MemberInfo>();
                }
                Logger.Debug("[{0}] GetSkippers result count ({1})", SessionID, retval.Count);
                return retval;
            }
            
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        
        public List<MemberInfo> GetCrew(int SessionID)
        {
            List<MemberInfo> retval = new List<MemberInfo>();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetSkippers Calling DBI Get Skippers", SessionID);
                try
                {
                    var temp = DBI.getCrew(SessionID);
                    foreach (CoastGuardMember cgm in temp)
                    {
                        var mi = new MemberInfo(cgm);
                        mi.updateSettings(DBI.getMemberSetting(mi.id));
                        retval.Add(mi);
                    }

                }
                catch (Exception ex)
                {
                    Logger.Debug("An error has occured in getSkippers");
                    Logger.ErrorEx(ex, "Get Skippers");
                    retval = new List<MemberInfo>();
                }
                Logger.Debug("[{0}] GetSkippers result count ({1})", SessionID, retval.Count);
                return retval;
            }
            
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<Course> GetCourses(int SessionID)
        {
            Logger.Debug("[{0}] GetCourses", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetCourses Calling DBI Get Cources", SessionID);
                var retval = DBI.getCourses(SessionID);
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetCourses result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }

     

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<Drill> GetDrills(int SessionID)
        {
            Logger.Debug("[{0}] GetDrills", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetDrills Calling DBI Get Cources", SessionID);
                var retval = DBI.getDrills(DBI.getUnit(SessionID));
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetDrills result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]

        public List<CoastguardRescueVessel> GetVesselList(int SessionID)
        {
            Logger.Debug("[{0}] GetVesselList", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetVesselList Calling DBI GettVesselList", SessionID);
                var retval = DBI.getVessels(DBI.getUnit(SessionID));
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetDrills result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<ActivityType> GetActivityTypes(int SessionID)
        {
            Logger.Debug("[{0}] GetActivityTypes", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetActivity Types Calling DBI Get ActivityTypes", SessionID);
                var retval = DBI.getActivityTypes(DBI.getUnit(SessionID));
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetActivityTypes result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
   
        public List<Procedure> GetProcedures(int SessionID)
        {
            Logger.Debug("[{0}] GetProcedures", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetProcedures Calling DBI Get Cources", SessionID);
                var retval = DBI.getProcedures(SessionID);
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetProcedures result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
       
        public List<ProcedureInfo> GetActivityProcedures(int SessionID)
        {
            Logger.Debug("[{0}] GetActivityProcedures", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetProcedures Calling DBI Get Cources", SessionID);
                var retval = new List<ProcedureInfo>();
                var tempList = DBI.getActivityProcedures(DBI.getUnit(SessionID));
                foreach (ProcedureLastCompleted plc in tempList)
                {
                    ProcedureInfo pi = new ProcedureInfo();
                    pi.Comments = "";
                    pi.CompletedBy = new Guid();
                    //pi.DateCompleted = DateTime.MinValue;
                    pi.DateCompleted = DateTime.Now;
                    pi.Name = plc.Name;
                    pi.Type = plc.Type;
                    pi.id = Guid.NewGuid();
                    pi.ProcedureId = plc.id;
                    retval.Add(pi);

                }
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetActivityProcedures result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
        
        public MenuInfo GetMenu(int SessionID,string MenuName)
        {
            Logger.Debug("[{0}] GetMenuInfo", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetMenuInfo Calling DBI Get MenuInfo", SessionID);
                var retval = new MenuInfo();
                var t = DBI.getWhere("Custom_Menu", string.Format("Name == {0}",MenuName));
                var menu = DBI.getData<Custom_Menu>("Custom_Menu", t.where, t.objects).FirstOrDefault();
                retval.Name = MenuName;
                if (menu != null)
                {
                    var t2 = DBI.getWhere("Custom_MenuOption", string.Format("menu == {0}",menu.id));
                    var t3 = DBI.getData<Custom_MenuOption>("Custom_MenuOptions", t2.where, t2.objects);
                    retval.Options = (from o in t3 orderby o.sortorder select o).ToList();
                }
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetMenuInfo Completed", SessionID);
                return retval;
            }
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<Alert> GetAlertDetails(int SessionID)
        {
            Logger.Debug("[{0}] GetAlertDetails", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetAlertDetails Calling DBI Get Alerts", SessionID);
                var unit = DBI.getUnit( SessionID);
                var retval = DBI.getAlerts(unit);
                foreach (Alert a in retval)
                {
                    Logger.Debug("{0} Alert [{1}]", SessionID,a);
                }
                //var retval = new List<Course>();
                Logger.Debug("[{0}] Get Alert Details result count ({1}) ", SessionID, retval.Count);
                return retval;
            }
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<Scenario> GetScenarios(int SessionID)
        {
            Logger.Debug("[{0}] GetScenarios", SessionID);
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                Logger.Debug("[{0}] GetScenarios Calling DBI Get Cources", SessionID);
                var retval = DBI.getScenarios(DBI.getUnit(SessionID));
                //var retval = new List<Course>();
                Logger.Debug("[{0}] GetScenarios result count ({1})", SessionID, retval.Count);
                return retval;
            }
        }
        [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public EventMessage LogMessage(int SessionID,int Level,string message)
        {

            Logger.Debug("Javascript Massage:{0}", message);
            var retval = new EventMessage();
            
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                //return DBI.getData<WCFEvent>("WCFEvents",string.Format("session={0}",SessionId),null);
                retval.Events =  DBI.getEvents(SessionID);


            }
            return retval;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]

        public Boolean UpdateStatistics(int SessionID)
        {

            Logger.Info("Calling Update Statistics for sessionid [{0}]", SessionID);
            var retval = false;

            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {
                //return DBI.getData<WCFEvent>("WCFEvents",string.Format("session={0}",SessionId),null);
                retval = DBI.UpdateStatistics(DBI.getUnit(SessionID));


            }
            return retval;
        }
        
        
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public ActivityID AddActivity(int SessionID, ActivityInfo Activity)
        {
            ActivityID retval = null ;
            Logger.Info("t=[{0}]",Activity.date);
            Logger.Info("Adding Trip for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    retval = DBI.addModifyActivity(Activity.getDBIActivity(SessionID));


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex,"AddTrip");
            }
            return retval;
        }





        [WebInvoke(
        Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
       
        public Boolean BackupDataBase(int SessionID, string fname)
        {
            Logger.Info("Backing Up Database to {0}",fname );
            
            //Logger.Info("Value:{0}", t.AirTemp);
            if (string.IsNullOrEmpty(fname)) return false;
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var retval = DBI.BackupDatabase( System.Web.Hosting.HostingEnvironment.MapPath("~") + string.Format("\\backups\\{0}.bak",fname));
                    

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "AddTrip");
            }
            return true;
        }

        [WebInvoke(
               Method = "POST",
       BodyStyle = WebMessageBodyStyle.Wrapped,
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]

        public Boolean RestoreDataBase(int SessionID, string fname)
        {
            Logger.Info("restoring Database from {0}", fname);

            //Logger.Info("Value:{0}", t.AirTemp);
            if (string.IsNullOrEmpty(fname)) return false;
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var retval = DBI.RestoreDatabase(System.Web.Hosting.HostingEnvironment.MapPath("~") + string.Format("\\backups\\{0}.bak", fname));


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Restore database");
            }
            return true;
        }


        [WebInvoke(
        Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyCoastGuardMember(int SessionID, MemberInfo Member)
        {
            Logger.Info("Member=({0}) XML[{1}]",Member,Member.ToJSV());
            Logger.Info("Adding/Modifying Member for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var member = Member.getDBIMember();
                    if (member.Unit == new Guid()) member.Unit = DBI.getUnit(SessionID);
                    var retval = DBI.ModifyMember(member);
                    DBI.ModifyMemberSetting(Member.getDBIMemberSetting());

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex,"AddTrip");
            }
            return true;
        }

        [WebInvoke(
        Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]

        public Boolean ModifyCoastguardRescueVessel(int SessionID, CoastguardRescueVesselDetail crv)
        {
            
            Logger.Info("Adding/Modifying CRV {1} for session id {0}", SessionID,crv);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    DBI.ModifyCoastguardRescueVessel(crv);

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Adding/Modifying CRV");
            }
            return true;
        }



        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyCourse(int SessionID, Course course)
        {
            Logger.Info("Course=({0})", course);
            Logger.Info("Adding/Modifying Course for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    if (course.Unit == new Guid()) course.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyCourse(course);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Course");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyDrill(int SessionID, Drill drill)
        {
            Logger.Info("Drill=({0})", drill);
            Logger.Info("Adding/Modifying drill for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    if (drill.Unit == new Guid()) drill.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyDrill(drill);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify drill");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyAttachment(int SessionID, Attachment attach)
        {
            Logger.Info("Attachment=({0})", attach);
            Logger.Info("Adding/Modifying attachment for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    //if (attach.Unit == new Guid()) attach.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyAttachment(attach);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify attachment");
            }
            return true;
        }
        private String ListToString(List<string> inList)
        {
            var retval = "[";
            var delim = "";
            foreach (string s in inList)
            {
                retval += delim + s;
                delim = "|";
            }
            retval += "]";
            return retval;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyTable(int SessionID, string TableName, List<string> Fields)
        {
            
            Logger.Info("Adding/Modifying Table {1} for session id {0} Object[{2}]", SessionID,TableName,ListToString(Fields));
            bool retval = false;
            //Logger.Info("Value:{0}", t.AirTemp);
            
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var unit = DBI.getUnit(SessionID).HasValue?DBI.getUnit(SessionID).Value:new Guid();
                    //JavaScriptSerializer ser = new JavaScriptSerializer();
                    switch (TableName.ToLower())
                    {
                        case "activitytype":
                            ActivityType at =new ActivityType();
                            try
                            {
                                at.id = new Guid(Fields[0]);
                            at.Name = Fields[1];
                            at.IsWaterBased = Convert.ToBoolean(Fields[2]);
                            at.Deleted = Convert.ToBoolean(Fields[3]);
                            at.Unit = unit;
                            }
                            catch (Exception)
                            {
                                at.id = Guid.NewGuid();
                                at.Name = "";
                                at.IsWaterBased = null;
                                at.Deleted = null;
                                at.Unit = unit;
                            }

                            retval =  DBI.ModifyActivityType(at);
                            break;
                        case "roletype":
                            RoleType rt = new RoleType();
                            rt.id = new Guid(Fields[0]);

                            rt.Name = Fields[1];
                            rt.Unit = unit;
                            rt.Deleted = Convert.ToBoolean(Fields[2]);
                            retval = DBI.ModifyRoleType(rt);
                            break;
                        case "destination":
                            Destination d = new Destination();
                            try
                            {
                                d.id = new Guid(Fields[0]);

                                d.Name = Fields[1];
                                d.Latitude = DBI.DBIToInt(Fields[2]);
                                d.Longitude = DBI.DBIToInt(Fields[3]);
                                d.Deleted = Convert.ToBoolean(Fields[4]);
                                d.Unit = unit;
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorEx(ex, "");
                            }
                            retval = DBI.ModifyDestination(d);
                            
                            break;
                        case "attachment":
                            Attachment a = new Attachment();
                            a.id = new Guid(Fields[0]);

                            a.Name = Fields[1];
                            retval = DBI.ModifyAttachment(a);

                            break;
                        case "engine":
                            Engine e = new Engine();
                            e.id = new Guid(Fields[0]);
                            e.SerialNumber = Fields[1];
                            int hours = 0;
                            if (int.TryParse(Fields[2], out hours))
                            {
                                e.Hours = hours;
                            }
                            bool isActive = false;
                            if (bool.TryParse(Fields[3], out isActive))
                            {
                                e.Active = isActive;
                            }
                            e.unit = unit;
                            return DBI.ModifyEngine(e);
                            break;

                    }
                    


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "{0} Modifying Table[{1}]",SessionID,TableName);
            }
            return retval;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyActivityType(int SessionID, ActivityType activityType)
        {
            Logger.Info("ActivityType=({0})", activityType);
            Logger.Info("Adding/Modifying ActivityType for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    if (activityType.Unit == new Guid()) activityType.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyActivityType(activityType);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify ActivityType");
            }
            return true;
        }
        
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyProcedure(int SessionID, Procedure Procedure)
        {
            Logger.Info("Procedure=({0})", Procedure);
            Logger.Info("Adding/Modifying Procedure for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    if (Procedure.Unit == new Guid()) Procedure.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyProcedure(Procedure);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Procedure");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyProcedureInfo(int SessionID, ProcedureInfo inProcedureInfo)
        {
            Logger.Info("Modify Procedure Info");
            Logger.Info("ProcedureInfo=({0})", inProcedureInfo);
            Logger.Info("Adding/Modifying ProcedureInfo for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    //if (drill.Unit == new Guid()) drill.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyProcedureLog(inProcedureInfo.getProcedureLog());


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Procedure");
            }
            return true;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyActivityPassanger(int SessionID, ActivityPassangerInfo inPassangerInfo)
        {
            Logger.Info("Modify ModifyActivityPassanger");
            Logger.Info("PassangerInfo=({0})", inPassangerInfo);
            Logger.Info("Adding/Modifying Passanger for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    return DBI.ModifyActivityPassanger(inPassangerInfo.getActivityPassanger());


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Procedure");
            }
            return true;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyActivityDrill(int SessionID, ActivityDrillInfo inDrillInfo)
        {
            Logger.Info("Adding/Modifying DrillInfo for session id {0}", SessionID);
            Logger.Info("DrillInfo=({0})", inDrillInfo);
            
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    return DBI.ModifyActivityDrill(inDrillInfo.getActivityDrill());


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Procedure");
            }
            return true;
        }
        [WebInvoke(
        Method = "POST",
        BodyStyle = WebMessageBodyStyle.Wrapped,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyActivityScenario(int SessionID, ActivityScenarioInfo inScenarioInfo)
        {
            Logger.Info("Adding/Modifying ScenarioInfo for session id {0}", SessionID);
            Logger.Info("DrillInfo=({0})", inScenarioInfo);

            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    return DBI.ModifyActivityScenario(inScenarioInfo.getActivityScenario());


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Scenario");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyActivityDestination(int SessionID, ActivityDestinationInfo inDestinationInfo)
        {
            Logger.Info("Adding/Modifying DestinationInfo for session id {0}", SessionID);
            Logger.Info("DestinationInfo=({0})", inDestinationInfo);

            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    return DBI.ModifyActivityDestination(inDestinationInfo.getActivityDestination());


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Destination");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyAlert(int SessionID, Alert alert)
        {
            Logger.Info("Alert=({0})", alert);
            Logger.Info("Adding/Modifying Alert for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    if (alert.Unit == new Guid()) alert.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyAlert(alert);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Alert");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Boolean ModifyScenario(int SessionID, Scenario scenario)
        {
            Logger.Info("Scenario=({0})", scenario);
            Logger.Info("Adding/Modifying Scenario for session id {0}", SessionID);
            //Logger.Info("Value:{0}", t.AirTemp);
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    if (scenario.Unit == new Guid()) scenario.Unit = DBI.getUnit(SessionID);
                    return DBI.ModifyScenario(scenario);


                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modify Scenario");
            }
            return true;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public ActivityInfo NewActivity(int SessionID)
        {
            Logger.Info("{0} Getting new Trip", SessionID);
            var retval = new ActivityInfo(SessionID);
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            Logger.Info("{0} Creating Activity, Guid=[{1}]",SessionID,retval.id);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public MemberInfo NewUser(int SessionID)
        {
            Logger.Info("{0} Getting new Trip", SessionID);
            var retval = new MemberInfo();
            
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Course NewCourse(int SessionID)
        {
            Logger.Info("{0} Getting new Course", SessionID);
            var retval = new Course();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    retval.Unit = DBI.getUnit(SessionID);


                }
            
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Drill NewDrill(int SessionID)
        {
            Logger.Info("{0} Getting new Drill", SessionID);
            var retval = new Drill();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                retval.Unit = DBI.getUnit(SessionID);


            }
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public ActivityType NewActivityType(int SessionID)
        {
            Logger.Info("{0} Getting new ActivityType", SessionID);
            var retval = new ActivityType();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                retval.Unit = DBI.getUnit(SessionID);


            }
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public JSONObject NewObject(int SessionID,string ObjectName)
        {
            try
            {
                Logger.Info("{0} Getting new Object Name [{1}]", SessionID, ObjectName);
                var retval = new JSONObject();
                retval.Name = ObjectName;
                var MinValueUtc = DateTime.MinValue.ToUniversalTime().AddYears(1);
                //retval.id.CompareTo(
                //var test = retval.id.CompareTo(new Guid());
                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
                using (DBIInterface DBI = new DBIInterface())
                {
                   Type t;
                   switch (ObjectName.ToLower())
                   {
                       case "activitydestinationinfo":
                           t = typeof(ActivityDestinationInfo);
                           break;
                       case "activitypassangerinfo":
                           t = typeof(ActivityPassangerInfo);
                           break;
                       case "activitydrillinfo":
                           t = typeof(ActivityDrillInfo);
                           break;
                       case "activityscenarioinfo":
                           t = typeof(ActivityScenarioInfo);
                           break;
                       case "crv":
                           t = typeof(CoastguardRescueVessel);
                           break;
                       default:
                           t = DBI.GetEntityTypeFromTableName(ObjectName);

                           if (t == null)
                           {
                               t = Type.GetType("DBInterface." + ObjectName);
                               if (t == null)
                               {
                                   return null;
                               }
                           }
                           break;
                   }
                    var tempval = Activator.CreateInstance(t);
                    
                    switch (ObjectName.ToLower())
                    {
                        case "attachment":
                            var attach = (Attachment)tempval;
                            Logger.Debug("updating attachment dates");
                            if (attach.DateCreated < MinValueUtc)
                            {
                                attach.DateCreated = MinValueUtc;
                            }
                            if (attach.DateUpdated < MinValueUtc)
                            {
                                attach.DateUpdated = MinValueUtc;
                            }
                            tempval = attach;
                            break;
                        default:
                            break;
                    }
                    /// Serialize to JSON
                    //var tempval = new RoleType();
                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(tempval.GetType());
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    serializer.WriteObject(ms, tempval);
                    string json = Encoding.Default.GetString(ms.ToArray());
                    retval.ObjectStr = json;

                }
                return retval;
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
                return null;
            }
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public List<JSONObject> GetObjects(int SessionID, string ObjectName, string where)
        {
            //throw new Exception("test");


            Logger.Info("{0} Getting new Object [{1}] Where [{2}]", SessionID,ObjectName,where);
            var retval = new List<JSONObject>();

            if (where == null)
            {
                Logger.Debug("{0} No where clause defined for object [{1}] returning empty list",SessionID,ObjectName,where);
                return retval;
            }
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var unit = DBI.getUnit(SessionID);
                    switch (ObjectName.ToLower()) {
                        case "activitydrillinfo":
                            var aditempTable = DBI.getITable("ActivityDrills",where);
                            var myActivity = new Guid(where);
                            
                            var drills = (from ActivityDrill a in aditempTable where a.Activity == myActivity select a);
                            //foreach (ActivityDrill o in DBI.getData<ActivityDrill>("ActivityDrills"))
                            foreach (ActivityDrill o in drills)
                            {
                                Logger.Debug("{0} Processing ActivityDrills", SessionID);


                                //retval.id.CompareTo(
                                //var test = retval.id.CompareTo(new Guid());
                                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);


                                /// Serialize to JSON
                                try
                                {
                                    var tempval = new JSONObject();
                                    var adi = new ActivityDrillInfo((ActivityDrill)o);
                                    tempval.Name = ObjectName;

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(adi.GetType());
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    serializer.WriteObject(ms, adi);
                                    string json = Encoding.Default.GetString(ms.ToArray());
                                    tempval.ObjectStr = json;
                                    retval.Add(tempval);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                            break;
                        case "activitydestinationinfo":
                            var additempTable = DBI.getITable("ActivityDestinations", where);
                            myActivity = new Guid(where);
                            var destinations = (from ActivityDestination a in additempTable where a.Activity == myActivity select a);
                            //foreach (ActivityDrill o in DBI.getData<ActivityDrill>("ActivityDrills"))
                            foreach (ActivityDestination o in destinations)
                            {
                                Logger.Debug("{0} Processing ActivityDestinations", SessionID);


                                //retval.id.CompareTo(
                                //var test = retval.id.CompareTo(new Guid());
                                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);


                                /// Serialize to JSON
                                try
                                {
                                    var tempval = new JSONObject();
                                    var adi = new ActivityDestinationInfo((ActivityDestination)o);
                                    tempval.Name = ObjectName;

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(adi.GetType());
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    serializer.WriteObject(ms, adi);
                                    string json = Encoding.Default.GetString(ms.ToArray());
                                    tempval.ObjectStr = json;
                                    retval.Add(tempval);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                            break;
                        case "activitypassangerinfo":
                            var acptempTable = DBI.getITable("ActivityPassangers", where);
                            myActivity = new Guid(where);
                            var passangers = (from ActivityPassanger a in acptempTable where a.Activity == myActivity select a);
                            //foreach (ActivityDrill o in DBI.getData<ActivityDrill>("ActivityDrills"))
                            foreach (ActivityPassanger o in passangers)
                            {
                                Logger.Debug("{0} Processing ActivityPassangers", SessionID);


                                //retval.id.CompareTo(
                                //var test = retval.id.CompareTo(new Guid());
                                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);


                                /// Serialize to JSON
                                try
                                {
                                    var tempval = new JSONObject();
                                    var adi = new ActivityPassangerInfo((ActivityPassanger)o);
                                    tempval.Name = ObjectName;

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(adi.GetType());
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    serializer.WriteObject(ms, adi);
                                    string json = Encoding.Default.GetString(ms.ToArray());
                                    tempval.ObjectStr = json;
                                    retval.Add(tempval);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                            break;
                        case "activityscenarioinfo":
                            var adstempTable = DBI.getITable("ActivityScenarios", where);
                            var myActivityS = new Guid(where);
                            var scenarios = (from ActivityScenario a in adstempTable where a.Activity == myActivityS select a);
                            //foreach (ActivityDrill o in DBI.getData<ActivityDrill>("ActivityDrills"))
                            foreach (ActivityScenario o in scenarios)
                            {
                                Logger.Debug("{0} Processing ActivityScenarios", SessionID);


                                //retval.id.CompareTo(
                                //var test = retval.id.CompareTo(new Guid());
                                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);


                                /// Serialize to JSON
                                try
                                {
                                    var tempval = new JSONObject();
                                    var adi = new ActivityScenarioInfo((ActivityScenario)o);
                                    tempval.Name = ObjectName;

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(adi.GetType());
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    serializer.WriteObject(ms, adi);
                                    string json = Encoding.Default.GetString(ms.ToArray());
                                    tempval.ObjectStr = json;
                                    retval.Add(tempval);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                            break;
                        case "destinations2":
                            var desttempTable = DBI.getITable(ObjectName, where);
                            //myActivity = new Guid(where);
                            
                            //foreach (ActivityDrill o in DBI.getData<ActivityDrill>("ActivityDrills"))
                            foreach (var o in desttempTable)
                            {
                                Logger.Debug("{0} Processing ActivityScenarios", SessionID);


                                //retval.id.CompareTo(
                                //var test = retval.id.CompareTo(new Guid());
                                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);


                                /// Serialize to JSON
                                try
                                {
                                    var tempval = new JSONObject();
                                    var adi = new ActivityScenarioInfo((ActivityScenario)o);
                                    tempval.Name = ObjectName;

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(adi.GetType());
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    serializer.WriteObject(ms, adi);
                                    string json = Encoding.Default.GetString(ms.ToArray());
                                    tempval.ObjectStr = json;
                                    retval.Add(tempval);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                            break;
                        case "crv": case "crvs":
                            //var crvtempTable = DBI.getITable("CoastguardRescueVessel", where);
                            //ObjectName = "CoastguardRescueVessel";
                            //foreach (ActivityDrill o in DBI.getData<ActivityDrill>("ActivityDrills"))


                            foreach (var crv  in DBI.getCRVs())
                            {
                                Logger.Debug("{0} Processing CoastguardRescueVessels", SessionID);


                                //retval.id.CompareTo(
                                //var test = retval.id.CompareTo(new Guid());
                                //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);


                                /// Serialize to JSON
                                try
                                {
                                    var tempval = new JSONObject();
                                    //var adi = new ActivityScenarioInfo((ActivityScenario)o);
                                    tempval.Name = ObjectName;

                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(crv.GetType());
                                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    serializer.WriteObject(ms, crv);
                                    string json = Encoding.Default.GetString(ms.ToArray());
                                    tempval.ObjectStr = json;
                                    retval.Add(tempval);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                            break;

                        default:

                    var tempTable = DBI.getITable(ObjectName);
                    

                    try
                    {
                    foreach (var o in tempTable)
                        {
                            Logger.Debug("Processing table [{0}]", tempTable.ToString());


                            //retval.id.CompareTo(
                            //var test = retval.id.CompareTo(new Guid());
                            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);

                            //object o = myEnumerator.Current;
                            /// Serialize to JSON
                            if (o != null)
                            {
                                Logger.Debug("Processing Object of Type {0} name {1}", o.GetType(), ObjectName);
                                try
                                {
                                    var tempval = new JSONObject();
                                    tempval.Name = ObjectName;
                                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(o.GetType());

                                    //System.IO.MemoryStream ms = new System.IO.MemoryStream();
                                    //Logger.Debug("write object {0}",ObjectName);
                                    //serializer.WriteObject(ms, o);
                                    //Logger.Debug("get JSON {0}", ObjectName);
                                    //string json = Encoding.Default.GetString(ms.ToArray());
                                    //Logger.Debug("assign to tempval {0}", ObjectName);
                                    //Newtonsoft.Json.
                                    tempval.ObjectStr = Newtonsoft.Json.JsonConvert.SerializeObject(o);
                                    Logger.Debug("ADDING TO LIST {0}", ObjectName);
                                    retval.Add(tempval);
                                    Logger.Debug("returning {0}", ObjectName);
                                }
                                catch (Exception ex)
                                {
                                    Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex, "{1} Getting Table Data for Object {0}", ObjectName, SessionID);
                    }
                    finally
                    {
                        tempTable = null;
                    }
                    break;
                }
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "{0} Get Objects [{1}]",SessionID,ObjectName);
            }
            
            Logger.Debug("{0} Returning {1} {2}s",SessionID,retval.Count,ObjectName);
            return retval;
        }

        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public JSONObjectAsStringList NewObjectAsStringList(int SessionID, string ObjectName)
        {
            Logger.Info("{0} Getting new Object", SessionID);
            var retval = new JSONObjectAsStringList();
            Guid unit;
            retval.Name = ObjectName;
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                unit = DBI.getUnit(SessionID).HasValue?DBI.getUnit(SessionID).Value:new Guid();


            }
            
            retval.ObjectStr = new List<string>();
            switch (ObjectName)
            {
                case "ActivityType":
                        ActivityType c = new ActivityType();
                        c.Unit = unit;
                        //Logger.Debug("processing Member");
                        
                        retval.ObjectStr.Add(c.id.ToString());
                        retval.ObjectStr.Add("");
                        retval.ObjectStr.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                        break;
                case "RoleType":
                        RoleType rt = new RoleType();
                        //Logger.Debug("processing Member");
                        rt.Unit = unit;
                        retval.ObjectStr.Add(rt.id.ToString());
                        retval.ObjectStr.Add("");
                        retval.ObjectStr.Add((rt.Deleted.HasValue ? rt.Deleted.Value : false).ToString());
                        break;

                case "Alert":
                        Alert a = new Alert();
                        a.Unit = unit;
                        retval.ObjectStr.Add(a.id.ToString());
                        retval.ObjectStr.Add("");
                        
                        retval.ObjectStr.Add((a.Deleted.HasValue ? a.Deleted.Value : false).ToString());
                        
                    
                    break;
                case "Destination":
                    Destination d = new Destination();
                    retval.ObjectStr.Add(d.id.ToString());
                    retval.ObjectStr.Add("");
                    retval.ObjectStr.Add("0");
                    retval.ObjectStr.Add("0");
                    retval.ObjectStr.Add((d.Deleted.HasValue ? d.Deleted.Value : false).ToString());


                    break;
            }
           


            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Procedure NewProcedure(int SessionID)
        {
            Logger.Info("{0} Getting new Procedure", SessionID);
            var retval = new Procedure();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                retval.Unit = DBI.getUnit(SessionID);


            }
            
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Alert NewAlert(int SessionID)
        {
            Logger.Info("{0} Getting new Alert", SessionID);
            var retval = new Alert();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                retval.Unit = DBI.getUnit(SessionID);


            }
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }






        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public Scenario NewScenario(int SessionID)
        {
            Logger.Info("{0} Getting new Scenario", SessionID);
            var retval = new Scenario();
            using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                retval.Unit = DBI.getUnit(SessionID);


            }
            //retval.id.CompareTo(
            //var test = retval.id.CompareTo(new Guid());
            //Logger.Error("{0} Creating Trip, Guid=[{1}] temp=[{2}]", SessionID, retval.id, test);
            return retval;
        }
        [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public List<PortTideTime> getTideHeights(int SessionID,DateTime dt,List<string> Ports)
        {
            Logger.Info("{0} Getting Tide Heights ",SessionID);
            List<PortTideTime> retval = null;
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    Logger.Info("{0} Calling DBI Get Tide Heights for Date[{1}],Ports[{2}]", SessionID,dt,Ports);
                    try
                    {
                        retval = DBI.getTideHeights(DBI.DateOnly(dt), Ports);
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex,"{0} Calling DBI Get Tide Heights for Date[{1}],Ports[{2}]", SessionID, dt, Ports);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex,"Get Tide Hegits");
            }
            Logger.Debug("{1} Get Tide Hights returning [{0}]", retval.Count,SessionID);
            return retval;
        }
        [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]
      
        public ActivityInfo getActivity(int SessionID,Guid tripid)        {
            Logger.Info("{0} Getting Activity ", SessionID);
            ActivityInfo retval = null;
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    Logger.Info("{0} Calling DBI Get activity for [{1}]",SessionID,tripid);
                    try
                    {
                        retval = new ActivityInfo(DBI.getActivity(tripid));
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex, "{0} Calling DBI Get activity for id[{1}]", SessionID, tripid);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Get Trip");
            }
            
            
            return retval;
        }
        
        [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public FindResult GetActivities(int SessionID,DateTime StartDt,DateTime EndDt,string Filter,int MaxCount)
        {
            Logger.Info("{0} Getting Tide Heights ",SessionID);
            FindResult retval = new FindResult();
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var unit = DBI.getUnit(SessionID);
                    Logger.Info("{0} Calling DBI Get Trips from Date[{1}-{2}] filter [{3}] max count[{4}] unit [{5}]", SessionID,StartDt,EndDt,Filter,MaxCount,unit);
                    try
                    {
                       var temp = DBI.getActivities(StartDt,EndDt,Filter,MaxCount,unit);
                       List<List<string>> l1 = new List<List<string>>();
                        foreach (Activity t in temp) {
                            List<string> l2 = new List<string>();
                            CoastGuardMember skip = null;
                            ActivityType act = null;
                            if (t.ActivityType.HasValue)
                            {
                                act = DBI.getActivityType(t.ActivityType.Value);
                            }
                            if (t.Skipper.HasValue)
                            {
                                skip = DBI.getMemberInfo(t.Skipper.Value,SessionID);
                            }
                            


                            l2.Add(t.id.ToString());
                            l2.Add(t.LogNo.ToString());
                            l2.Add(t.Date.ToShortDateString());
                            if (act == null)
                            {
                                l2.Add("");
                            }
                            else
                            {
                                l2.Add(act.Name);
                            }



                            l2.Add(string.Format("{0:HH:mm}", t.DepartTime.Value));
                            l2.Add(string.Format("{0:HH:mm}", t.ReturnTime.Value));
                            try
                            {
                                if (skip == null)
                                {
                                    l2.Add("");
                                }
                                else
                                {
                                    l2.Add(skip.First_Name + ' ' + skip.Last_Name);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorEx(ex, "");
                            }
                            l2.Add(t.AirTemp.Value.ToString());
                            l2.Add(t.WaterTemp.Value.ToString());
                            l2.Add(t.BarometicPresure.ToString());
                            l1.Add(l2);
                        }
                        retval.titles = new List<String>();
                        retval.titles.Add("ID");
                        retval.titles.Add("Log No");
                        retval.titles.Add("Date");
                        retval.titles.Add("Type");
                        retval.titles.Add("Depart Time");
                        retval.titles.Add("Return Time");
                        retval.titles.Add("Skipper");
                        retval.titles.Add("Air Temp");
                        retval.titles.Add("Water Temp");
                        retval.titles.Add("Presure");
                        //retval.data = (from t in temp select t.id.ToString()).ToList();
                       retval.data = l1;
                       //retval.titles = new JQDataTableTitl te[retval.data.Count()];
                       //retval.titles[0]=new JQDataTableTitle("Tes");
                      
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex,"{0} Calling DBI Get Trips from Date[{1}-{2}] filter [{3}] max count[{4}]", SessionID,StartDt,EndDt,Filter,MaxCount);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex,"Get Trips Hegits");
            }
            //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
            return retval;
        }

        [WebInvoke(
       Method = "POST",
       BodyStyle = WebMessageBodyStyle.Wrapped,
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json)]

        public FindResult GetRecentActivities(int SessionID, int MaxCount)
        {
            Logger.Info("{0} Getting Tide Heights ", SessionID);
            FindResult retval = new FindResult();
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {
                    var unit = DBI.getUnit(SessionID).HasValue?DBI.getUnit(SessionID).Value:new Guid();
                    Logger.Info("{0} Calling DBI Get Recent Trips  max count[{1}]", SessionID,  MaxCount);
                    try
                    {
                        var temp = DBI.getActivities( MaxCount,unit);
                        List<List<string>> l1 = new List<List<string>>();
                        foreach (Activity t in temp)
                        {
                            List<string> l2 = new List<string>();
                            CoastGuardMember skip = null;
                            ActivityType act = null;
                            if (t.ActivityType.HasValue)
                            {
                                act = DBI.getActivityType(t.ActivityType.Value);
                            }
                            if (t.Skipper.HasValue)
                            {
                                skip = DBI.getMemberInfo(t.Skipper.Value,SessionID);
                            }



                            l2.Add(t.id.ToString());
                            l2.Add(t.LogNo.ToString());
                            l2.Add(t.Date.ToShortDateString());
                            if (act == null)
                            {
                                l2.Add("");
                            }
                            else
                            {
                                l2.Add(act.Name);
                            }

                            l2.Add(string.Format("{0:HH:mm}",t.DepartTime.Value));
                            l2.Add(string.Format("{0:HH:mm}", t.ReturnTime.Value));
                            try
                            {
                                if (skip == null)
                                {
                                    l2.Add("");
                                }
                                else
                                {
                                    l2.Add(skip.First_Name + ' ' + skip.Last_Name);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorEx(ex, "");
                            }
                            l2.Add(t.AirTemp.Value.ToString());
                            l2.Add(t.WaterTemp.Value.ToString());
                            l2.Add(t.BarometicPresure.ToString());
                            l1.Add(l2);
                        }
                        retval.titles = new List<String>();
                        retval.titles.Add("ID");
                        retval.titles.Add("Log No");
                        retval.titles.Add("Date");
                        retval.titles.Add("Type");
                        retval.titles.Add("Depart Time");
                        retval.titles.Add("Return Time");
                        retval.titles.Add("Skipper");
                        retval.titles.Add("Air Temp");
                        retval.titles.Add("Water Temp");
                        retval.titles.Add("Presure");
                        //retval.data = (from t in temp select t.id.ToString()).ToList();
                        retval.data = l1;
                        //retval.titles = new JQDataTableTitl te[retval.data.Count()];
                        //retval.titles[0]=new JQDataTableTitle("Tes");

                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex, "{0} Calling DBI Get Recent Trips max count[{4}]", SessionID, MaxCount);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Get Trips Hegits");
            }
            //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
            return retval;
        }
    
    
      [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public FindResult GetUsers(int SessionID,string Filter,int MaxCount)
        {
            Logger.Info("{0} Getting Users ",SessionID);
            FindResult retval = new FindResult();
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    Logger.Info("{0} Calling DBI Get Users filter [{1}] max count[{2}]", SessionID,Filter,MaxCount);
                    try
                    {
                       var temp = DBI.getUsers(Filter,MaxCount,DBI.getUnit(SessionID).HasValue?DBI.getUnit(SessionID).Value:new Guid());
                       Logger.Debug("{0} Found {1} Users", SessionID, temp.Count);
                       List<List<string>> l1 = new List<List<string>>();
                        foreach (CrewStatisticsTotal cgm in temp) {
                            //Logger.Debug("processing Member");
                            List<string> l2 = new List<string>();
                            l2.Add(cgm.id.ToString());
                            l2.Add(cgm.UserId);
                            l2.Add(cgm.First_Name);
                            l2.Add(cgm.Last_Name);
                            l2.Add(cgm.Mobile);
                            l2.Add(cgm.Home);
                            l2.Add(cgm.Work);
                            l2.Add(cgm.email);
                            l2.Add((cgm.TotalHours.HasValue)?Math.Round(cgm.TotalHours.Value,2).ToString():"0");
                            l2.Add((cgm.Skipper.HasValue)?cgm.Skipper.Value.ToString():"False");
                            l2.Add((cgm.Operational.HasValue) ? cgm.Operational.Value.ToString() : "False");
                            l2.Add((cgm.Trainee.HasValue) ? cgm.Trainee.Value.ToString() : "False");
                            l2.Add((cgm.Senior.HasValue) ? cgm.Senior.Value.ToString() : "False");
                            l2.Add((cgm.Deleted.HasValue) ? cgm.Deleted.Value.ToString() : "False");
                            l1.Add(l2);
                        }
                        retval.titles = new List<String>();
                        retval.titles.Add("ID");
                        retval.titles.Add("User ID");
                        retval.titles.Add("First Name");
                        retval.titles.Add("Last Name");
                        retval.titles.Add("Mobile");
                        retval.titles.Add("Home");
                        retval.titles.Add("Work");
                        retval.titles.Add("Email");
                        retval.titles.Add("Total Hours");
                        retval.titles.Add("Skipper");
                        retval.titles.Add("Operational");
                        retval.titles.Add("Trainee");
                        retval.titles.Add("Senior");
                        retval.titles.Add("Deleted");
                        retval.data = l1;                      
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex,"{0} Calling DBI Get Users  filter [{1}] max count[{2}]", SessionID,Filter,MaxCount);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex,"Get Trips Hegits");
            }
            //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
            return retval;
        }

      [WebInvoke(
Method = "POST",
BodyStyle = WebMessageBodyStyle.Wrapped,
RequestFormat = WebMessageFormat.Json,
ResponseFormat = WebMessageFormat.Json)]

      public FindResult GetPeople(int SessionID, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting People ", SessionID);
          FindResult retval = new FindResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {

                  Logger.Info("{0} Calling DBI Get People filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {

                      var temp = DBI.getPeople(Filter, MaxCount,DBI.getUnit(SessionID));
                      Logger.Debug("{0} Found {1} people", SessionID, temp.Count);
                      List<List<string>> l1 = new List<List<string>>();
                      foreach (Person cgm in temp)
                      {
                          //Logger.Debug("processing Member");
                          List<string> l2 = new List<string>();
                          l2.Add(cgm.id.ToString());
                          //l2.Add(cgm.UserId);
                          //l2.Add(cgm.First_Name);
                          //l2.Add(cgm.Last_Name);
                          //l2.Add(cgm.Mobile);
                          //l2.Add(cgm.Home);
                          //l2.Add(cgm.Work);
                          //l2.Add(cgm.email);
                          //l2.Add((cgm.TotalHours.HasValue) ? Math.Round(cgm.TotalHours.Value, 2).ToString() : "0");
                          //l2.Add((cgm.Skipper.HasValue) ? cgm.Skipper.Value.ToString() : "False");
                          //l2.Add((cgm.Operational.HasValue) ? cgm.Operational.Value.ToString() : "False");
                          //l2.Add((cgm.Trainee.HasValue) ? cgm.Trainee.Value.ToString() : "False");
                          //l2.Add((cgm.Senior.HasValue) ? cgm.Senior.Value.ToString() : "False");
                          //l2.Add((cgm.Deleted.HasValue) ? cgm.Deleted.Value.ToString() : "False");
                          //l1.Add(l2);
                      }
                      retval.titles = new List<String>();
                      retval.titles.Add("ID");
                      //retval.titles.Add("User ID");
                      //retval.titles.Add("First Name");
                      //retval.titles.Add("Last Name");
                      //retval.titles.Add("Mobile");
                      //retval.titles.Add("Home");
                      //retval.titles.Add("Work");
                      //retval.titles.Add("Email");
                      //retval.titles.Add("Total Hours");
                      //retval.titles.Add("Skipper");
                      //retval.titles.Add("Operational");
                      //retval.titles.Add("Trainee");
                      //retval.titles.Add("Senior");
                      //retval.titles.Add("Deleted");
                      retval.data = l1;
                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get Person filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Trips Person");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }
  
      [WebInvoke(
    Method = "POST",
    BodyStyle = WebMessageBodyStyle.Wrapped,
    RequestFormat = WebMessageFormat.Json,
    ResponseFormat = WebMessageFormat.Json)]
      
        public FindResult FindCourses(int SessionID,string Filter,int MaxCount)
        {
            Logger.Info("{0} Getting Courses ",SessionID);
            FindResult retval = new FindResult();
            try
            {
                using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
                {

                    Logger.Info("{0} Calling DBI Get Courses filter [{1}] max count[{2}]", SessionID,Filter,MaxCount);
                    try
                    {
                       var temp = DBI.getCourses(Filter,MaxCount,SessionID);
                       Logger.Debug("{0} Found {1} Users", SessionID, temp.Count);
                       List<List<string>> l1 = new List<List<string>>();
                        foreach (Course c in temp) {
                            //Logger.Debug("processing Member");
                            List<string> l2 = new List<string>();
                            l2.Add(c.Id.ToString());
                            l2.Add(c.Name);
                            l1.Add(l2);
                        }
                        retval.titles = new List<String>();
                        retval.titles.Add("ID");
                        retval.titles.Add("Course Name");
                        retval.data = l1;                      
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorEx(ex,"{0} Calling DBI Get Courses  filter [{1}] max count[{2}]", SessionID,Filter,MaxCount);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex,"Get Trips Hegits");
            }
            //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
            return retval;
        }

      [WebInvoke(
 Method = "POST",
 BodyStyle = WebMessageBodyStyle.Wrapped,
 RequestFormat = WebMessageFormat.Json,
 ResponseFormat = WebMessageFormat.Json)]
    
      public FindResult FindDrills(int SessionID, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting Drills ", SessionID);
          FindResult retval = new FindResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {

                  Logger.Info("{0} Calling DBI Get Drills filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {
                      var temp = DBI.getDrills(Filter, MaxCount,SessionID);
                      Logger.Debug("{0} Found {1} Drills", SessionID, temp.Count);
                      List<List<string>> l1 = new List<List<string>>();
                      foreach (Drill c in temp)
                      {
                          //Logger.Debug("processing Member");
                          List<string> l2 = new List<string>();
                          l2.Add(c.id.ToString());
                          l2.Add(c.Code);
                          l2.Add(c.Name);
                          l2.Add((c.Deleted.HasValue?c.Deleted.Value:false).ToString());
                          l1.Add(l2);
                      }
                      retval.titles = new List<String>();
                      retval.titles.Add("ID");
                      retval.titles.Add("Code");
                      retval.titles.Add("Course Name");
                      retval.titles.Add("Deleted");
                      retval.data = l1;
                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get Drills  filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Trips Hegits");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }
      [WebInvoke(
   Method = "POST",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   RequestFormat = WebMessageFormat.Json,
   ResponseFormat = WebMessageFormat.Json)]
    
      public FindResult FindProcedures(int SessionID, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting Procedures ", SessionID);
          FindResult retval = new FindResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {

                  Logger.Info("{0} Calling DBI Get Procedures filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {
                      var temp = DBI.getProcedures(Filter, MaxCount,SessionID);
                      Logger.Debug("{0} Found {1} Procedures", SessionID, temp.Count);
                      List<List<string>> l1 = new List<List<string>>();
                      foreach (Procedure c in temp)
                      {
                          //Logger.Debug("processing Member");
                          List<string> l2 = new List<string>();
                          l2.Add(c.id.ToString());
              
                          l2.Add(c.Name);
                          l2.Add(TypeToString(c.Type));
                          l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                          l1.Add(l2);
                      }
                      retval.titles = new List<String>();
                      retval.titles.Add("ID");
              
                      retval.titles.Add("Procedure Name");
                      retval.titles.Add("Procedure Type");
                      retval.titles.Add("Deleted");
                      retval.data = l1;
                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get Procedures  filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Trips Hegits");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }
        private string TypeToString(int typeid) {
            var retval = "";
            var types = new List<String>();
            types.Add("Weekly");
            types.Add("Monthly");
            types.Add("StartUp");
            types.Add("Maintenance");
            types.Add("Safety");
            types.Add("Radio");
            retval = types[typeid];
            return retval;

        }
        

      [WebInvoke(
   Method = "POST",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   RequestFormat = WebMessageFormat.Json,
   ResponseFormat = WebMessageFormat.Json)]
    
      public FindResult FindAlerts(int SessionID, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting Alerts ", SessionID);
          FindResult retval = new FindResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {

                  Logger.Info("{0} Calling DBI Get Alerts filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {
                      var temp = DBI.getAlerts(Filter, MaxCount,SessionID);
                      Logger.Debug("{0} Found {1} Alerts", SessionID, temp.Count);
                      List<List<string>> l1 = new List<List<string>>();
                      foreach (Alert c in temp)
                      {
                          //Logger.Debug("processing Member");
                          List<string> l2 = new List<string>();
                          l2.Add(c.id.ToString());
                         
                          l2.Add(c.Name);
                          l2.Add(c.Fields);
                          l2.Add(c.Tables);
                          l2.Add(c.Where);
                          l2.Add(c.Text);
                          l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                          l1.Add(l2);
                      }
                      retval.titles = new List<String>();
                      retval.titles.Add("ID");
                      
                      retval.titles.Add("Alert Name");
                      retval.titles.Add("Fields");
                      retval.titles.Add("Tables");
                      retval.titles.Add("Where");
                      retval.titles.Add("Text");
                      retval.titles.Add("Deleted");
                      retval.data = l1;
                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get Alerts  filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Trips Hegits");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }
      [WebInvoke(
 Method = "POST",
 BodyStyle = WebMessageBodyStyle.Wrapped,
 RequestFormat = WebMessageFormat.Json,
 ResponseFormat = WebMessageFormat.Json)]
    
      public EditResult GetEditGrid(int SessionID,string TableName, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting Edit Grid for table [{1}] ", SessionID,TableName);
          EditResult retval = new EditResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {
                  var unit = DBI.getUnit(SessionID).HasValue ? DBI.getUnit(SessionID).Value : new Guid();

                  Logger.Info("{0} Calling DBI Get Edit Grid filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {
                      //Type entityType = DBI.GetEntityTypeFromTableName(TableName);
                      retval.Name = TableName;    
                    
                      
                      List<List<string>> l1 = new List<List<string>>();
                      switch (TableName.ToLower()) {
                          case "activitytype":
                               foreach (ActivityType c in DBI.getData<ActivityType>(TableName))
                               {
                                    //Logger.Debug("processing Member");
                                   if (c.Unit == unit)
                                   {
                                       List<string> l2 = new List<string>();
                                       l2.Add(c.id.ToString());
                                       l2.Add(c.Name);
                                       l2.Add((c.IsWaterBased.HasValue ? c.IsWaterBased.Value : false).ToString());
                                       l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                                       l1.Add(l2);
                                   }
                               }

                               retval.titles = DBI.getTitles(TableName);
                               retval.data = l1;
                  
                              break;
                          case "roletype":
                              foreach (RoleType c in DBI.getData<RoleType>(TableName))
                              {
                                  //Logger.Debug("processing Member");
                                  if (c.Unit == unit)
                                  {
                                      List<string> l2 = new List<string>();
                                      l2.Add(c.id.ToString());
                                      l2.Add(c.Name);
                                      l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                                      l1.Add(l2);
                                  }
                              }

                              retval.titles = DBI.getTitles(TableName);
                              retval.data = l1;
                              
                              break;

                          case "alert":
                              foreach (Alert c in DBI.getData<Alert>(TableName))
                              {
                                  //Logger.Debug("processing Member");
                                  if (c.Unit == unit)
                                  {
                                      List<string> l2 = new List<string>();
                                      l2.Add(c.id.ToString());
                                      l2.Add(c.Name);
                                      l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                                      l1.Add(l2);
                                  }
                              }
                               retval.titles = new List<String>();
                               retval.titles.Add("ID");
                               retval.titles.Add("Alert Name");
                               retval.titles.Add("Deleted");
                               retval.data = l1;
                              break;
                          case "custom_lookups":
                              foreach (Custom_Lookup c in DBI.getData<Custom_Lookup>(TableName))
                              {
                                 // if (c.Unit == unit)
                                 // {
                                      //Logger.Debug("processing Member");
                                      List<string> l2 = new List<string>();
                                      l2.Add(c.id.ToString());
                                      l2.Add(c.Name);
                                      l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                                      l1.Add(l2);
                                  //}
                              }
                              retval.titles = new List<String>();
                              retval.titles.Add("ID");
                              retval.titles.Add("Lookup Name");
                              retval.titles.Add("Deleted");
                              retval.data = l1;
                              break;
            
                          case "destination":
                              Logger.Debug("{0} Processing Destination", SessionID);
                              foreach (Destination c in DBI.getData<Destination>("Destinations"))
                              {
                                  if (c.Unit == unit)
                                  {
                                      Logger.Debug("processing Desination");
                                      List<string> l2 = new List<string>();
                                      l2.Add(c.id.ToString());
                                      l2.Add(c.Name);
                                      l2.Add(c.Latitude.ToString());
                                      l2.Add(c.Longitude.ToString());
                                      l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                                      l1.Add(l2);
                                  }
                              }
                              retval.titles = DBI.getTitles(TableName); 
                              retval.data = l1;
                              break;

                              case "crv":
                              Logger.Debug("{0} Processing crv", SessionID);
                              foreach (CoastguardRescueVessel c in DBI.getData<CoastguardRescueVessel>("CoastguardRescueVessel"))
                              {
                                  if (c.Unit == unit)
                                  {
                                      Logger.Debug("processing crv");
                                      List<string> l2 = new List<string>();
                                     // l2.Add(c.id.ToString());
                                      l2.Add(c.id.ToString());
                                      l2.Add(c.name);
                                      //l2.Add(c.MiddleEngine.ToString());
                                      //l2.Add(c.PortEngine.ToString());
                                      //l2.Add(c.StarboardEngine.ToString());
                                      //l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                                      l1.Add(l2);
                                  }
                              }
                              retval.titles = DBI.getTitles("CoastguardRescueVessel"); 
                              retval.data = l1;
                              break;
                              case "engine":
                              Logger.Debug("{0} Processing crv", SessionID);
                              foreach (Engine e in DBI.getData<Engine>("Engine"))
                              {
                                  //var crv = (CoastguardRescueVessel)e.CRV.Value;
                                  //if (e.CRV.Value == unit)
                                  //{
                                      Logger.Debug("processing Engine");
                                      List<string> l2 = new List<string>();
                                      l2.Add(e.id.ToString());
                                     // l2.Add(e.name);
                                      l2.Add(e.SerialNumber);
                                      l2.Add(e.Hours.ToString());
                                      l2.Add(e.Active.ToString());
                                      l1.Add(l2);
                                  //}
                              }
                              retval.titles = DBI.getTitles(TableName);
                              retval.data = l1;
                              break;
                          case "attachment":
                              Logger.Debug("{0} Processing Attachment", SessionID);
                              var t = DBI.getWhere("Attachment", Filter);
                              foreach (Attachment a in DBI.getData<Attachment>("Attachment", t.where, t.objects))
                              {
                                  //if (c.Unit == unit)
                                  //{
                                      Logger.Debug("processing Attachmnet");
                                      List<string> l2 = new List<string>();
                                      l2.Add(a.id.ToString());

                                      l2.Add(a.Name);
                                      l2.Add(a.Path.ToString());
                                      l2.Add(a.DateUpdated.ToString());
                                      l2.Add(a.DateCreated.ToString());
                                      l2.Add(Left(a.Description, 50));

                                      l1.Add(l2);
                                  //}
                              }
                              retval.titles = DBI.getTitles(TableName);
                              retval.data = l1;
                              break;

                          case "backupfiles":
                              Logger.Debug("{0} Processing Get Backup Files", SessionID);
                              var path = System.Web.Hosting.HostingEnvironment.MapPath("~") + @"\Backups";
                              Logger.Debug(" Checking [{0}] for Backups", path);
                              System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(path);
                              var i = 0;
                              foreach (System.IO.FileInfo fi in DI.GetFiles("*.bak"))
                              {
                              
                                  List<string> l2 = new List<string>();
                                  var name = fi.Name;
                                  if (fi.Extension == ".bak")
                                  {
                                      name = name.Substring(0, name.Length - 4);
                                  }
                                  l2.Add(i.ToString());
                                  i++;
                                  l2.Add(fi.Name);
                                  l2.Add(fi.CreationTime.ToLongDateString());
                                  l2.Add(fi.LastAccessTime.ToLongDateString());
                                  l2.Add(fi.LastWriteTime.ToLongDateString());
                                  l1.Add(l2);
                              }
                              retval.titles = new List<string>();
                              retval.titles.Add("Id");
                              retval.titles.Add("Name");
                              retval.titles.Add("Creation Time");
                              retval.titles.Add("Last Accesses Time");
                              retval.titles.Add("Last Write Time");
                              retval.data = l1;
                              break;
                      }


                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get Alerts  filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Trips Hegits");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }

      public static string Left(string text, int length)
      {
          if (length < 0)
              throw new ArgumentOutOfRangeException("length", length, "length must be > 0");
          else if (length == 0 || text.Length == 0)
              return "";
          else if (text.Length <= length)
              return text;
          else
              return text.Substring(0, length);
      }


      [WebInvoke(
   Method = "POST",
   BodyStyle = WebMessageBodyStyle.Wrapped,
   RequestFormat = WebMessageFormat.Json,
   ResponseFormat = WebMessageFormat.Json)]
    
      public FindResult FindActivityTypes(int SessionID, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting Alerts ", SessionID);
          FindResult retval = new FindResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {

                  Logger.Info("{0} Calling DBI Get ActivityTypes filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {
                      var temp = DBI.getActivityTypes(Filter, MaxCount,SessionID);
                      Logger.Debug("{0} Found {1} ActivityTypes", SessionID, temp.Count);
                      List<List<string>> l1 = new List<List<string>>();
                      foreach (ActivityType c in temp)
                      {
                          //Logger.Debug("processing Member");
                          List<string> l2 = new List<string>();
                          l2.Add(c.id.ToString());

                          l2.Add(c.Name);
                          l2.Add((c.Deleted.HasValue ? c.Deleted.Value : false).ToString());
                          l1.Add(l2);
                      }
                      retval.titles = new List<String>();
                      retval.titles.Add("ID");
                      retval.titles.Add("Name");
                      retval.titles.Add("Deleted");
                      retval.data = l1;
                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get ActivityTypes  filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Activity Types");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }
     [WebInvoke(
 Method = "POST",
 BodyStyle = WebMessageBodyStyle.Wrapped,
 RequestFormat = WebMessageFormat.Json,
 ResponseFormat = WebMessageFormat.Json)]
    
      public FindResult FindScenarios(int SessionID, string Filter, int MaxCount)
      {
          Logger.Info("{0} Getting Scenarios ", SessionID);
          FindResult retval = new FindResult();
          try
          {
              using (DBIInterface DBI = new DBIInterface(Properties.Settings.Default.ConnectionString))
              {

                  Logger.Info("{0} Calling DBI Get Scenarios filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  try
                  {
                      var temp = DBI.getScenarios(Filter, MaxCount,DBI.getUnit(SessionID));
                      Logger.Debug("{0} Found {1} Scenarios", SessionID, temp.Count);
                      List<List<string>> l1 = new List<List<string>>();
                      foreach (Scenario c in temp)
                      {
                          //Logger.Debug("processing Member");
                          List<string> l2 = new List<string>();
                          l2.Add(c.id.ToString());
                          l2.Add(c.Code);
                          l2.Add(c.Name);
                          l2.Add((c.Deleted.HasValue?c.Deleted.Value:false).ToString());
                          l1.Add(l2);
                      }
                      retval.titles = new List<String>();
                      retval.titles.Add("ID");
                      retval.titles.Add("Code");
                      retval.titles.Add("Course Name");
                      retval.titles.Add("Deleted");
                      retval.data = l1;
                  }
                  catch (Exception ex)
                  {
                      Logger.ErrorEx(ex, "{0} Calling DBI Get Scenarios  filter [{1}] max count[{2}]", SessionID, Filter, MaxCount);
                  }

              }
          }
          catch (Exception ex)
          {
              Logger.ErrorEx(ex, "Get Trips Hegits");
          }
          //Logger.Debug("{1} Get Tide Trips returning [{0}]", retval.Count,SessionID);
          return retval;
      }
    }

}
