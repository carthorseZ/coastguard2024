using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace BackEnd
{
    // NOTE: If you change the interface name "ICGBE" here, you must also update the reference to "ICGBE" in App.config.
    [ServiceContract]
    public interface ICGBE
    {
        [OperationContract]
        Int32 Login(string Username,string Password,string Unit);
        [OperationContract]
        Boolean Logoff(int SessionID);
        [OperationContract]
        EventMessage LogMessage(int SessionID, int Level, string Message);
        [OperationContract]
        List<MemberInfo> GetSkippers(int SessionID);
        [OperationContract]
        List<MemberInfo> GetCrew(int SessionID);
        [OperationContract]
        DBInterface.EngineStartHours GetEngineStartHours(int SessionId,Guid Vessel,int LogNo);
        [OperationContract]
        List<String> GetReports(int SessionID);
        [OperationContract]
        List<String> GetBackups(int SessionID);
        [OperationContract]
        List<DBInterface.Course> GetCourses(int SessionID);
        [OperationContract]
        List<DBInterface.Drill> GetDrills(int SessionID);

        [OperationContract]
        List<DBInterface.CoastguardRescueVessel> GetVesselList(int SessionID);
        
        [OperationContract]
        List<DBInterface.ActivityType> GetActivityTypes(int SessionID);

        [OperationContract]
        Boolean BackupDataBase(int SessionID, string fname);
        [OperationContract]
        Boolean RestoreDataBase(int SessionID, string fname);
        [OperationContract]
        List<DBInterface.Procedure> GetProcedures(int SessionID);
        [OperationContract]
        List<ProcedureInfo> GetActivityProcedures(int SessionID);
        [OperationContract]
        List<DBInterface.Alert> GetAlertDetails(int SessionID);
        [OperationContract]
        List<AlertInfo> GetAlerts(int SessionID);
        [OperationContract]
        List<DBInterface.Scenario> GetScenarios(int SessionID);
        [OperationContract]
  
        ActivityInfo NewActivity(int SessionID);
        [OperationContract]
        MemberInfo NewUser(int SessionID);
        [OperationContract]
        
        DBInterface.Course NewCourse(int SessionID);
        [OperationContract]
        
        DBInterface.Procedure NewProcedure(int SessionID);
        [OperationContract]
        JSONObject NewObject(int SessionID, string ObjectName);
        [OperationContract]
        List<JSONObject> GetObjects(int SessionID, string ObjectName,string where);
        
        
        [OperationContract]
        JSONObjectAsStringList NewObjectAsStringList(int SessionID, string ObjectName);
        [OperationContract]
        DBInterface.Drill NewDrill(int SessionID);
        [OperationContract]

        DBInterface.ActivityType NewActivityType(int SessionID);
        [OperationContract]
        
        DBInterface.Alert NewAlert(int SessionID);
        [OperationContract]
        
        DBInterface.Scenario NewScenario(int SessionID);

        [OperationContract]
        DBInterface.ActivityID AddActivity(int SessionID, ActivityInfo Activity);
        [OperationContract]
        Boolean ModifyCoastGuardMember(int SessionID, MemberInfo Member);
        [OperationContract]
        Boolean ModifyCourse(int SessionID, DBInterface.Course Course);
        [OperationContract]
        Boolean ModifyDrill(int SessionID, DBInterface.Drill Drill);
        [OperationContract]
        Boolean ModifyTable(int SessionID, string TableName,List<string> Fields);
        
        [OperationContract]
        Boolean ModifyActivityType(int SessionID, DBInterface.ActivityType ActivityType);
        [OperationContract]
        Boolean ModifyProcedure(int SessionID, DBInterface.Procedure Procedure);
        [OperationContract]
        Boolean ModifyProcedureInfo(int SessionID, ProcedureInfo Procedure);
        [OperationContract]
        Boolean ModifyAlert(int SessionID, DBInterface.Alert Alert);
        
        [OperationContract]
        Boolean ModifyScenario(int SessionID, DBInterface.Scenario Scenario);

        [OperationContract]
        Boolean ModifyActivityDrill(int SessionID, ActivityDrillInfo inDrillInfo);
        [OperationContract]
        Boolean ModifyActivityScenario(int SessionID, ActivityScenarioInfo inScenarioInfo);
        [OperationContract]
        Boolean ModifyActivityDestination(int SessionID, ActivityDestinationInfo inDestinationInfo);
        [OperationContract]
        Boolean ModifyActivityPassanger(int SessionID, ActivityPassangerInfo inPassangerInfo);
        [OperationContract]
        Boolean ModifyAttachment(int SessionID, DBInterface.Attachment attach);
        [OperationContract]
        List<DBInterface.PortTideTime> getTideHeights(int SessionID,DateTime dt,List<string> Ports);
        [OperationContract]
        FindResult GetActivities(int SessionID, DateTime StartDt, DateTime EndDt, string Filter, int MaxCount);
        [OperationContract]
        FindResult GetRecentActivities(int SessionID, int MaxCount);
        [OperationContract]
        ActivityInfo getActivity(int SessionID, Guid tripid);
        [OperationContract]
        MenuInfo GetMenu(int SessionID, string MenuName);
        [OperationContract]
        FindResult GetUsers(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        FindResult GetPeople(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        FindResult FindCourses(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        FindResult FindDrills(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        FindResult FindActivityTypes(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        FindResult FindProcedures(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        FindResult FindAlerts(int SessionID, string Filter, int MaxCount);
        
        [OperationContract]
        FindResult FindScenarios(int SessionID, string Filter, int MaxCount);
        [OperationContract]
        Boolean UpdateStatistics(int SessionID);
        [OperationContract]
        EditResult GetEditGrid(int SessionID,string TableName, string Filter, int MaxCount);
        
        [OperationContract]
        DBInterface.Session GetSession(int SessionId);
        [OperationContract]
        List<DBInterface.WCFEvent> GetEvents(int SessionId);
        // TODO: Add your service operations here
    }


}
