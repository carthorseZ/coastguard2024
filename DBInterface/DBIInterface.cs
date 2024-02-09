using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Linq.SqlClient;
using System.Text;
using System.Globalization;
using System.Threading;

using Utils;
namespace DBInterface
{
	public struct ddPair
	{
		public object id;
		public string desc;
		public override string ToString() { return desc; }
	}

	public struct whereClause
	{
		public string where;
		public object[] objects;
	}

	public class ActivityID
	{
		public Guid id;
		public int LogNo;

	}

	public class DBIInterface:IDisposable
	{
		private static readonly Logger Logger = new Logger(typeof(DBIInterface));
		CoastGuardDataContext db = new CoastGuardDataContext(Properties.Settings.Default.CoastGuardConnectionString);
		private bool disposed = false;

		public DBIInterface()
		{
		}
		public DBIInterface(string connstring)
		{
			db = new CoastGuardDataContext(connstring);
		}
		public void Dispose()
		{
			Dispose(true);
		}
		private void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				disposed = true;

			}
		}

		public EngineStartHours getEngineStartHours(int SessionId,Guid Vessel,int LogNo)
		{
			var retval = new EngineStartHours();
			retval.PortEngineStart = 0;
			retval.StarboardEngineStart = 0;
            retval.MiddleEngineStart = 0;
            try

			{

				var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
                
                decimal? es = 0;
                if (LogNo > 0)
                {

                    Activity curAct = (from a in db.Activities where a.LogNo == LogNo select a).FirstOrDefault();
                    retval.MiddleEngineStart = 0; // Need to add fix for middle engine
                    retval.PortEngineStart = (decimal)curAct.EnginePortTotalHours - (decimal)curAct.EnginePortRunHours;
                    retval.StarboardEngineStart = (decimal)curAct.EngineStarboardTotalHours - (decimal)curAct.EngineStarboardRunHours;                                        
                    return retval;
                }
                else
                {
                    var vessel = (from v in db.CoastguardRescueVesselDetails where v.id == Vessel select v).FirstOrDefault();
                    
                    retval.PortEngineStart = (from e in db.Engines where vessel.PortEngine == e.id select e.Hours).FirstOrDefault();
                    retval.MiddleEngineStart = (from e in db.Engines where vessel.MiddleEngine == e.id select e.Hours).FirstOrDefault();
                    retval.StarboardEngineStart = (from e in db.Engines where vessel.StarboardEngine == e.id select e.Hours).FirstOrDefault();
                    
                    return retval;
                }
			}
			catch (Exception ex)
			{
				Logger.DebugEx(ex, "");
				return retval;
			}
		}

		public whereClause getWhere(string tablename,string sql)
		{
			var retval = new whereClause();
			int processingState = 0;
			string outsql = "";
			string delim = "";
			//bool nullable = false;
			List<object> myObjects=new List<object>();
			Type t = typeof(string);
			var i = 0;
			foreach (string s in sql.Split(' '))
			{
				switch (processingState)
				{
					case 0:
						t = GetColumnType(tablename, s);
						outsql += delim + s;
						if (IsNullableType(t))
						{
							//nullable = true;
							t = Nullable.GetUnderlyingType(t);
							outsql += ".value";
						}
						processingState = 1;
						break;
					case 1:
						outsql += delim + s;
						processingState = 2;
						break;
					case 2:
						outsql += delim + "@" + i;
						i++;

						
						switch (t.Name.ToLower())
						{
							case "guid":
								//if (nullable){
								//    var gid = new Guid?();
								//    gid = new Guid(RemoveSpecialCharacters(s));
								//    myObjects.Add(gid);
								
							   // } else {
									myObjects.Add(new Guid(RemoveSpecialCharacters(s)));   
							   // }
								break;
								
						  

							default:
								myObjects.Add(s);
								break;
						}
						processingState = 3;
						break;
					case 3:
						outsql += delim + s;
						processingState = 0;
						break;
				}
				delim = " ";
			}
			retval.objects = myObjects.ToArray();
			retval.where = outsql;
			return retval;
		}

		public List<string> getTitles(string TableName)

		{
			Logger.Debug("Getting Titles for [{0}]", TableName);
			var retval = new List<string>();
			var fields=(from field in db.Custom_Fields where field.TableName.ToLower() == TableName.ToLower() orderby field.Order select field).ToList();
			
			foreach (Custom_Field f in fields) {
				retval.Add(string.IsNullOrEmpty(f.Caption)?f.FieldName:f.Caption);
			}
			return retval;
		}
		public int getNextActivityID()
		{
			try
			{
				return (from t in db.Activities select t.LogNo).Max() + 1;
			} catch {
				return 1;
			}
		}

		public int getNextActivityID(Guid? unit)
		{
			try
			{
				if (unit.HasValue)
				{
					return (from t in db.Activities where t.Unit == unit select t.LogNo).Max() + 1;
				}
				else
				{
					return (from t in db.Activities select t.LogNo).Max() + 1;
				}
			}
			catch
			{
				return 1;
			}
		}
		public static string RemoveSpecialCharacters(string str) 
		{
			char[] buffer = new char[str.Length]; 
			int idx = 0; foreach (char c in str) { 
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c == '-')) { 
					buffer[idx] = c; idx++; 
				} 
			} 
			return new string(buffer, 0, idx); 
		} 

		public List<Activity> getActivities(DateTime Startdt,DateTime Enddt,string filter,int maxCount)
		{

			return (from t in db.Activities where t.Date >= Startdt && t.Date <= Enddt select t).Take(maxCount).ToList();

		}

		public List<Activity> getActivities(DateTime Startdt, DateTime Enddt, string filter, int maxCount, Guid? unit)
		{

			if (unit.HasValue)
			{
				return (from t in db.Activities where t.Date >= Startdt && t.Date <= Enddt && t.Unit == unit orderby t.LogNo descending select t ).Take(maxCount).ToList();
			}
			else
			{
				return (from t in db.Activities where t.Date >= Startdt && t.Date <= Enddt orderby t.LogNo descending select t).Take(maxCount).ToList();
			}
		}
		public List<Activity> getActivities( int maxCount,Guid unit)
		{
			return (from t in db.Activities where t.Unit == unit orderby t.LogNo descending  select t).Take(maxCount).ToList();
		}
		public Type getType(string s)
		{
			return Type.GetType(s,false,true);
		}
		//public List<CoastGuardMember> getUsers( string filter, int maxCount)
		//{
		//	return (from t in db.CoastGuardMembers select t).Take(maxCount).ToList();
		//}
		public List<CrewStatisticsTotal> getUsers( string filter, int maxCount,Guid unit)
		{
			return (from t in db.CrewStatisticsTotals where t.Unit == unit select t).Take(maxCount).ToList();
		}
		public List<Person> getPeople(string filter, int maxCount,Guid? unit)
		{
			if (unit.HasValue)
			{
				return (from t in db.Persons where t.Unit == unit select t).Take(maxCount).ToList();
			}
			else
			{
				return (from t in db.Persons select t).Take(maxCount).ToList();
			}
		}
		public List<Course> getCourses(string filter, int maxCount,int SessionId)
		{
			var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
			return (from t in db.Courses where t.Unit == unit select t).Take(maxCount).ToList();
		}
		public List<Drill> getDrills(string filter, int maxCount,int SessionId)
		{
			var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
			return (from t in db.Drills where t.Unit == unit select t).Take(maxCount).ToList();
		}
		public List<ActivityType> getActivityTypes(string filter, int maxCount,int SessionId)
		{
			var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
			//return (from t in db.ActivityTypes where t.Unit == unit select t).Take(maxCount).ToList();
			return (from t in db.ActivityTypes select t).Take(maxCount).ToList();
		}
		public List<Procedure> getProcedures(string filter, int maxCount,int SessionId)
		{
			var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
			return (from t in db.Procedures where t.Unit == unit select t).Take(maxCount).ToList();
		}
 
		public List<Alert> getAlerts(string filter, int maxCount,int SessionId)
		{
			var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
			return (from t in db.Alerts where t.Unit == unit select t).Take(maxCount).ToList();
		}

		public System.Data.Linq.ITable getITable(string TableName)
		{
			return db.GetTable(GetEntityTypeFromTableName(TableName));
		}
		public System.Data.Linq.ITable getITable(string TableName,string where)
		{
			return db.GetTable(GetEntityTypeFromTableName(TableName));
		}

		public List<WCFEvent> getEvents(int SessionId)
		{
			try
			{
				var retval = (from e in db.WCFEvents where e.session == SessionId orderby e.Timestamp select e).ToList();
				//var retval = select.ToList();
				if (retval != null)
				{
					db.WCFEvents.DeleteAllOnSubmit(retval);
					db.SubmitChanges();
				}
				return retval;
			}
			catch (Exception ex)
			{
				Logger.WarnEx(ex, "");
				return new List<WCFEvent>();
			}
		}

		public List<T> getData<T>(string TableName)
		{
			List<T> retval = new List<T>();
			var temp = db.GetTable(GetEntityTypeFromTableName(TableName));
			
				foreach (T t in temp)
				{                                        
					retval.Add(t);
				}
			return retval;
		}
		public List<T> getData<T>(string TableName,string where,object[] data)
		{
			List<T> retval = new List<T>();
			Logger.Debug("Processing Table [{0}] where [{1}]", TableName, where);
			try
			{
				var temp = db.GetTable(GetEntityTypeFromTableName(TableName)).Where(where,data);

				foreach (T t in temp)
				{
					retval.Add(t);
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Exeception Processing Table [{0}] where [{1}]",TableName,where);
			}
			return retval;
		}

		public List<String> getDataAsStringList(string TableName)
		{
			List<String> retval = new List<string>();
			var temp =db.GetTable(GetEntityTypeFromTableName(TableName));
			
			return retval;

		}

		
		public string getMessageName(int msgId)
		{
			Logger.Debug("Calling get MessageName for id {0}",msgId);
			var retval = (from ml in db.MessageLevels where ml.id == msgId select ml.Name).FirstOrDefault();
			if (retval == null) return ""; else return retval; 
		}

		public void AddEvent(int Sessionid,string name,object myData)
		{
			var myEvent = new WCFEvent();
			try
			{
				System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(myData.GetType());
				
				System.IO.MemoryStream ms = new System.IO.MemoryStream();
				serializer.WriteObject(ms, myData);
				string json = Encoding.Default.GetString(ms.ToArray());
				myEvent.objectstr = json;

			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			myEvent.id = Guid.NewGuid();
			myEvent.name = name;
			myEvent.session = Sessionid;
			db.WCFEvents.InsertOnSubmit(myEvent);
			db.SubmitChanges();
		}

		public bool BackupDatabase(string backupFName)
		{
			string sql = string.Format("BACKUP DATABASE {0} TO DISK = '{1}' WITH FORMAT;", "COASTGUARD", backupFName);

 
			System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString);
			System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(sql, sqlConn);
			try
			{
				sqlConn.Open();
				sqlcmd.ExecuteNonQuery();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Processing sql [{0}]",sql);
				throw (ex);
			}
			return true;
		}

		public bool RestoreDatabase(string backupFName)
		{
			string sql = string.Format("RESTORE DATABASE {0} FROM DISK = '{1}'", "COASTGUARD", backupFName);
			string killOtherProcesses = string.Format("DECLARE @SQL varchar(max) SELECT @SQL = COALESCE(@SQL,'') + 'Kill ' + Convert(varchar, SPId) + ';' FROM MASTER..SysProcesses as sp left outer join master..sysdatabases as db on sp.dbid  = db.dbid  WHERE db.name = '{0}' EXEC(@SQL)", "COASTGUARD"); 

			System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString);
			System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(sql, sqlConn);
			System.Data.SqlClient.SqlCommand sqlcmd2 = new System.Data.SqlClient.SqlCommand(killOtherProcesses, sqlConn);
			try
			{
				sqlConn.Open();
				sqlConn.ChangeDatabase("master");
				sqlcmd2.ExecuteNonQuery();
				sqlcmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Processing sql [{0}]", sql);
				throw (ex);
			}
			return true;
		}

		public bool UpdateStatistics(Guid? unit)
		{
			string sql = string.Format("exec CalcNewTripStatistics");
			

			System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString);
			System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(sql, sqlConn);
			
			try
			{
				sqlConn.Open();
				//sqlConn.ChangeDatabase("master");
				sqlcmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Processing sql [{0}]", sql);
				throw (ex);
			}
			return true;
		}
		public System.Data.DataSet ProcessAlert(string sql)
		{
			//var retval = new List<string>();
			//string sql = string.Format("Select * from {0} where {1}", table, where);
			System.Data.DataSet ds = new System.Data.DataSet();
			System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString);
			
			System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(sql,sqlConn);
			try
			{
				sqlConn.Open();

				System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd);
				sda.Fill(ds);
				sqlConn.Close();
			} catch (Exception ex) {
				Logger.ErrorEx(ex,"");
				
				if(sqlConn.State != System.Data.ConnectionState.Closed) {
				sqlConn.Close();
				}
				throw(ex);
			}
			return ds;
		}


		public System.Data.DataSet RunReport(string sql)
		{
			//var retval = new List<string>();
			//string sql = string.Format("Select * from {0} where {1}", table, where);
			System.Data.DataSet ds = new System.Data.DataSet();
			System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString);

			System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(sql, sqlConn);
			try
			{
				sqlConn.Open();

				System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd);
				sda.Fill(ds);
				sqlConn.Close();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");

				if (sqlConn.State != System.Data.ConnectionState.Closed)
				{
					sqlConn.Close();
				}
				throw (ex);
			}
			return ds;
		}
		public System.Data.DataSet RunReport(List<string> sql)
		{
			//var retval = new List<string>();
			//string sql = string.Format("Select * from {0} where {1}", table, where);
			System.Data.DataSet ds = new System.Data.DataSet();
			System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(db.Connection.ConnectionString);

		   
			try
			{
				sqlConn.Open();
				var CompleteSQL = "";
				var delim = "";
				foreach (string s in sql)
				{
					CompleteSQL += delim + s;
					delim = System.Environment.NewLine;
				}
				Logger.Debug("running sql [{0}]", CompleteSQL);
				System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(CompleteSQL, sqlConn);
				System.Data.SqlClient.SqlDataAdapter sda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd);

				sda.Fill(ds);
				sqlConn.Close();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");

				if (sqlConn.State != System.Data.ConnectionState.Closed)
				{
					sqlConn.Close();
				}
				throw (ex);
			}
			return ds;
		}
		private bool IsNullableType(Type inType)
		{
			return (inType.IsGenericType && inType.GetGenericTypeDefinition() == typeof(Nullable<>));

		}

		public Type GetColumnType(string tablename, string columnname)
		{
			System.Data.Linq.DataContext dc = db;
			System.Data.Linq.Mapping.MetaTable table = dc.Mapping.GetTables().Where(t => t.TableName.Equals("dbo." + tablename)).FirstOrDefault();
			if (table == null)
			{
				table = dc.Mapping.GetTables().Where(t => t.TableName.Contains(tablename)).FirstOrDefault();

				if (table == null)
				{
					var temp = (from a in dc.Mapping.GetTables() select a.TableName);
					foreach (string s in temp)
					{
						Logger.Debug("Table Name [{0}] found", s);
					}
				}
			}
			if (table != null)
			{
				foreach (System.Data.Linq.Mapping.MetaDataMember m in table.RowType.DataMembers)
				{
					if (m.Name == columnname)
					{
						//if (IsNullableType(m.Type))
						//{
						//    return Nullable.GetUnderlyingType(m.Type);
						//}
						//else
						//{
							return m.Type;
						//}
					}
				}
				return null ;
			}
			else
			{
				throw new ArgumentException("Invalid table name.");
			}
		}


		public Type GetEntityTypeFromTableName(string qualifiedTableName)
		{
			System.Data.Linq.DataContext dc = db;
			System.Data.Linq.Mapping.MetaTable table = dc.Mapping.GetTables().Where(t => t.TableName.Equals("dbo."+qualifiedTableName)).FirstOrDefault();
			if (table == null)
			{
				table = dc.Mapping.GetTables().Where(t => t.TableName.Contains(qualifiedTableName)).FirstOrDefault();
			   
				if (table == null)
				{
					var temp = (from a in dc.Mapping.GetTables() select a.TableName);
					foreach (string s in temp)
					{
						Logger.Debug("Table Name [{0}] found", s);
					}
				}
			}
			if (table != null)
			{
				return table.RowType.Type;
			}
			else
			{
				throw new ArgumentException(string.Format("Invalid table name[{0}].",qualifiedTableName));
			}
		}

 





		public List<Scenario> getScenarios(string filter, int maxCount,Guid? unit)
		{
			if (unit.HasValue)
			{
				return (from t in db.Scenarios where t.Unit == unit select t).Take(maxCount).ToList();
			}
			else
			{
				return (from t in db.Scenarios select t).Take(maxCount).ToList();
			}
		}
		public Activity getActivity(Guid activityId)
		{
			return (from t in db.Activities where t.id == activityId select t).FirstOrDefault();
		}
		public ActivityType getActivityType(Guid activityTypeId)
		{
			return (from t in db.ActivityTypes where t.id == activityTypeId select t).FirstOrDefault();
		}
		public CoastGuardMember getMemberInfo(string username, string password,string Unit)
		{
			CoastGuardMember tempval;
			//Member tempval = new Member();
			//tempval.id = new Guid();
			//
			//tempval.UserId = username;
			var myUnitid = (from u in db.Units where Unit.ToLower() == u.Name.ToLower() select u.id).FirstOrDefault();
			try {
			tempval = (from m in db.CoastGuardMembers where m.UserId == username && m.Unit == myUnitid select m).FirstOrDefault();
			} catch (Exception ex) {
				Logger.ErrorEx(ex, "{0} Get Member Info Conection String [{1}]","",this.db.Connection.ConnectionString);
				tempval = null;    
			}
			return tempval;
		   
		}
		public CoastGuardMember getMemberInfo(Guid memberid,Guid Unit)
		{
			CoastGuardMember tempval;
			//Member tempval = new Member();
			//tempval.id = new Guid();
			//
			//tempval.UserId = username;
			var myUnitid = (from u in db.Units where u.id == Unit select u.id).FirstOrDefault();
			try
			{
				tempval = (from m in db.CoastGuardMembers where m.id == memberid && m.Unit == myUnitid select m).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "{0} Get Member Info", "");
				tempval = null;
			}
			return tempval;

		}

		public CoastGuardMember getMemberInfo(Guid memberid, int Sessionid)
		{
			CoastGuardMember tempval;
			//Member tempval = new Member();
			//tempval.id = new Guid();
			//
			//tempval.UserId = username;
		   
			var myUnitid = (from u in db.Sessions where u.id == Sessionid select u.Unit).FirstOrDefault();
			try
			{
				tempval = (from m in db.CoastGuardMembers where m.id == memberid && m.Unit == myUnitid select m).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "{0} Get Member Info", "");
				tempval = null;
			}
			return tempval;

		}

		public Session getSessionInfo(int SessionId)
		{
			
			return (from a in db.Sessions where a.id == SessionId select a).FirstOrDefault();

		}
		public bool ModifyMember(CoastGuardMember t)
		{
			var curMember = (from m in db.CoastGuardMembers where m.id == t.id select m).FirstOrDefault();            
			try
			{
				if (curMember != null)
				{
					Logger.Debug("Updating Member ({0})", t);
					curMember.Last_Name = t.Last_Name;
					curMember.First_Name = t.First_Name;
					curMember.email = t.email;
					curMember.Home = t.Home;
					curMember.Mobile = t.Mobile;
					curMember.Password = t.Password;
					curMember.Skipper = t.Skipper;
					curMember.UserId = t.UserId;
					curMember.Work = t.Work;
					curMember.Deleted = t.Deleted;
					curMember.Operational = t.Operational;
					curMember.Senior = t.Senior;
					curMember.Trainee = t.Trainee;
				}
				else
				{
					Logger.Debug("Adding Member ({0})", t);
					t.id = Guid.NewGuid();
					db.CoastGuardMembers.InsertOnSubmit(t);
				} 
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying Activity ({0})",t);
				return false;
			}
		}
		public bool ModifyMemberSetting(MemberSetting t)
		{
			var curMember = (from m in db.MemberSettings where m.MemberId == t.MemberId select m).FirstOrDefault();
			try
			{
				if (curMember != null)
				{
					Logger.Debug("Updating Member ({0})", t);
					curMember.MemberId = t.MemberId;
					curMember.LogEnabled = t.LogEnabled;
					curMember.LogLevel = t.LogLevel;
				}
				else
				{
					Logger.Debug("Adding Member ({0})", t);
					//t.MemberId = Guid.NewGuid();
					db.MemberSettings.InsertOnSubmit(t);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying Activity ({0})", t);
				return false;
			}
		}
		public bool ModifyCourse(Course t)
		{
			var curCourse = (from c in db.Courses where c.Id == t.Id select c).FirstOrDefault();
			try
			{
				if (curCourse != null)
				{
					Logger.Debug("Updating Course ({0})", t);
					curCourse.Name = t.Name;
					
				}
				else
				{
					Logger.Debug("Adding Course ({0})", t);
					t.Id = Guid.NewGuid();
					db.Courses.InsertOnSubmit(t);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying course ({0})", t);
				return false;
			}
		}

		public bool ModifyDrill(Drill d)
		{
			var curDrill = (from c in db.Drills where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curDrill != null)
				{
					Logger.Debug("Updating Drill ({0})", d);
					curDrill.Name = d.Name;
					curDrill.Code = d.Code;
					curDrill.Deleted = d.Deleted;
				}
				else
				{
					Logger.Debug("Adding Drill ({0})", d);
					d.id = Guid.NewGuid();
					db.Drills.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying drill ({0})", d);
				return false;
			}
		}
		public bool ModifyAttachment(Attachment a)
		{
			var curAttachment = (from attach in db.Attachments where a.id == attach.id select attach).FirstOrDefault();
			try
			{
				if (curAttachment != null)
				{
					Logger.Debug("Updating Attachment ({0})", a);
					curAttachment.Name = a.Name;
					curAttachment.Description = a.Description;
					curAttachment.DateUpdated = System.DateTime.Now;
					curAttachment.ParentId = a.ParentId;
					curAttachment.ParentType = a.ParentType;
					curAttachment.Path = a.Path;
					curAttachment.Type = a.Type;

				   
				}
				else
				{
					Logger.Debug("Adding Attachment ({0})", a);
					a.id = Guid.NewGuid();
					a.DateCreated = System.DateTime.Now;
					a.DateUpdated = System.DateTime.Now;
					
					db.Attachments.InsertOnSubmit(a);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying attachment ({0})", a);
				return false;
			}
		}
		public bool ModifyActivityType(ActivityType d)
		{
			var curActivityType = (from c in db.ActivityTypes where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curActivityType != null)
				{
					Logger.Debug("Updating ActivityType ({0})", d);
					curActivityType.Name = d.Name;
                    curActivityType.IsWaterBased = d.IsWaterBased;
                    
				  
					curActivityType.Deleted = d.Deleted;
				}
				else
				{
					Logger.Debug("Adding ActivityType ({0})", d);
					d.id = Guid.NewGuid();
					
					db.ActivityTypes.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying ActivityType ({0})", d);
				return false;
			}
		}
        public bool ModifyEngine(Engine e)
        {
            var curEngine = (from c in db.Engines where c.id == e.id select c).FirstOrDefault();
            try
            {
                if (curEngine != null)
                {
                    Logger.Debug("Updating Engine ({0})", e);
                    curEngine.SerialNumber = e.SerialNumber;
                    curEngine.Hours = e.Hours;
                    curEngine.Active = e.Active;
                    
                }
                else
                {
                    Logger.Debug("Adding engine ({0})", e);
                    e.id = Guid.NewGuid();

                    db.Engines.InsertOnSubmit(e);
                }
                db.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modifying Engine ({0})", e);
                return false;
            }
        }


        public bool ModifyCoastguardRescueVessel(CoastguardRescueVesselDetail crvd)
        {
            
            try
            {
                var curVessel = (from c in db.CoastguardRescueVessels where c.id == crvd.id select c).FirstOrDefault();
                if (curVessel != null)
                {
                    Logger.Debug("Updating Vessel ({0})", curVessel);
                    curVessel.name = crvd.name;                    
                }
                else
                {
                    Logger.Debug("Adding Vessel ({0})", curVessel);
                    CoastguardRescueVessel c = new CoastguardRescueVessel();
                    c.id = Guid.NewGuid();
                    c.name = crvd.name;
                    c.Unit = crvd.Unit;
                    db.CoastguardRescueVessels.InsertOnSubmit(c);
                }
                db.SubmitChanges();
                // Port Engine


                var curVesselPortEngineDetail = (from c in db.CRV_Engines where c.id == crvd.id && c.Position == 1 select c).FirstOrDefault();
                if (curVesselPortEngineDetail != null)
                {
                    if (crvd.PortEngine.HasValue)
                    {
                        curVesselPortEngineDetail.Engine = crvd.PortEngine.Value;
                    }
                    else
                    {
                        db.CRV_Engines.DeleteOnSubmit(curVesselPortEngineDetail);
                    }
                    
                    
                    Logger.Debug("Updating Vessel Engine ({0})", curVessel);
                    //curVessel.name = crvd.name;
                }
                else
                {
                    Logger.Debug("Adding Vessel ({0})", curVessel);
                    if (crvd.PortEngine.HasValue)
                    {
                        curVesselPortEngineDetail.Engine = crvd.PortEngine.Value;
                        curVesselPortEngineDetail.CRV = crvd.id;
                        curVesselPortEngineDetail.Active = true;
                        curVesselPortEngineDetail.Position = 1;
                    }

                    //CoastguardRescueVessel c = new CoastguardRescueVessel();
                    //c.id = Guid.NewGuid();
                    //c.name = crvd.name;
                    //c.Unit = crvd.Unit;
                    //db.CoastguardRescueVessels.InsertOnSubmit(c);
                }
                db.SubmitChanges();
                var curVesselStarboardEngineDetail = (from c in db.CRV_Engines where c.id == crvd.id && c.Position == 2 select c).FirstOrDefault();
                if (curVesselStarboardEngineDetail != null)
                {
                    if (crvd.StarboardEngine.HasValue)
                    {
                        curVesselStarboardEngineDetail.Engine = crvd.StarboardEngine.Value;
                    }
                    else
                    {
                        db.CRV_Engines.DeleteOnSubmit(curVesselStarboardEngineDetail);
                    }


                    Logger.Debug("Updating Vessel Engine ({0})", curVessel);
                    //curVessel.name = crvd.name;
                }
                else
                {
                    Logger.Debug("Adding Vessel ({0})", curVessel);
                    if (crvd.StarboardEngine.HasValue)
                    {
                        curVesselStarboardEngineDetail.Engine = crvd.StarboardEngine.Value;
                        curVesselStarboardEngineDetail.CRV = crvd.id;
                        curVesselStarboardEngineDetail.Active = true;
                        curVesselStarboardEngineDetail.Position = 1;
                    }

                    //CoastguardRescueVessel c = new CoastguardRescueVessel();
                    //c.id = Guid.NewGuid();
                    //c.name = crvd.name;
                    //c.Unit = crvd.Unit;
                    //db.CoastguardRescueVessels.InsertOnSubmit(c);
                }
                db.SubmitChanges();
                var curVesselMiddleEngineDetail = (from c in db.CRV_Engines where c.id == crvd.id && c.Position == 3 select c).FirstOrDefault();
                if (curVesselMiddleEngineDetail != null)
                {
                    if (crvd.MiddleEngine.HasValue)
                    {
                        curVesselMiddleEngineDetail.Engine = crvd.MiddleEngine.Value;
                    }
                    else
                    {
                        db.CRV_Engines.DeleteOnSubmit(curVesselMiddleEngineDetail);
                    }


                    Logger.Debug("Updating Vessel Engine ({0})", curVessel);
                    //curVessel.name = crvd.name;
                }
                else
                {
                    Logger.Debug("Adding Vessel ({0})", curVessel);
                    if (crvd.MiddleEngine.HasValue)
                    {
                        curVesselMiddleEngineDetail.Engine = crvd.MiddleEngine.Value;
                        curVesselMiddleEngineDetail.CRV = crvd.id;
                        curVesselMiddleEngineDetail.Active = true;
                        curVesselMiddleEngineDetail.Position = 1;
                    }

                    //CoastguardRescueVessel c = new CoastguardRescueVessel();
                    //c.id = Guid.NewGuid();
                    //c.name = crvd.name;
                    //c.Unit = crvd.Unit;
                    //db.CoastguardRescueVessels.InsertOnSubmit(c);
                }
                db.SubmitChanges();
                
                return true;
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Modifying CoastguardrescueVessel ({0})", crvd);
                return false;
            }
        }
		public bool ModifyRoleType(RoleType d)
		{
			var curRoleType = (from c in db.RoleTypes where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curRoleType != null)
				{
					Logger.Debug("Updating RoleType ({0})", d);
					curRoleType.Name = d.Name;

					curRoleType.Deleted = d.Deleted;
				}
				else
				{
					Logger.Debug("Adding RoleType ({0})", d);
					d.id = Guid.NewGuid();
					db.RoleTypes.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying RoleType ({0})", d);
				return false;
			}
		}
		public bool ModifyDestination(Destination d)
		{
			var curDestination = (from c in db.Destinations where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (d.Name.Length > 256) d.Name = d.Name.Substring(0, 256);
				if (curDestination != null)
				{
					Logger.Debug("Updating Destination ({0})", d);
					curDestination.Name = d.Name;
					curDestination.Latitude = d.Latitude;
					curDestination.Longitude = d.Longitude;
					curDestination.Deleted = d.Deleted;
				}
				else
				{
					Logger.Debug("Adding Destination ({0})", d);
					d.id = Guid.NewGuid();
					db.Destinations.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying Destination ({0})", d);
				return false;
			}
		}
		public bool ModifyProcedure(Procedure d)
		{
			var curProcedure = (from c in db.Procedures where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curProcedure != null)
				{
					Logger.Debug("Updating Procedure ({0})", d);
					curProcedure.Name = d.Name;
					curProcedure.Description = d.Description;
		  
				}
				else
				{
					Logger.Debug("Adding Procedure ({0})", d);
					d.id = Guid.NewGuid();
					db.Procedures.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying Procedure ({0})", d);
				return false;
			}
		}
		public bool ModifyProcedureLog(ProcedureLog d)
		{
			var curProcedureLog = (from c in db.ProcedureLogs where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curProcedureLog != null)
				{
					Logger.Debug("Updating ProcedureLog ({0})", d);
					curProcedureLog.MemberId = d.MemberId;
					curProcedureLog.ProcedureId = d.ProcedureId;
					curProcedureLog.DateCompleted = d.DateCompleted;
					curProcedureLog.ActivityId = d.ActivityId;
				}
				else
				{
					Logger.Debug("Adding ProcedureLog ({0})", d);
					d.id = Guid.NewGuid();
					db.ProcedureLogs.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying ProcedureLog ({0})", d);
				return false;
			}
		}
		public bool ModifyAlert(Alert d)
		{           
			var curAlert = (from c in db.Alerts where c.id == d.id select c).FirstOrDefault();
			try
			{
                
				if (curAlert != null)
				{
					Logger.Debug("Updating Alert ({0})", d);
					curAlert.Name = d.Name;
					curAlert.Deleted = d.Deleted;
				}
				else
				{
					Logger.Debug("Adding Alert ({0})", d);
					d.id = Guid.NewGuid();
					db.Alerts.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying Alert ({0})", d);
				return false;
			}
		}
		public bool ModifyActivityDrill(ActivityDrill d)
		{
			var curDrill = (from c in db.ActivityDrills where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curDrill != null)
				{
					Logger.Debug("Updating ActivityDrill ({0})", d);
					curDrill.Activity = d.Activity;
					curDrill.DrillId = d.DrillId;
					curDrill.id = d.id;
				}
				else
				{
					Logger.Debug("Adding ActivityDrill ({0})", d);
					d.id = Guid.NewGuid();
					db.ActivityDrills.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying ActivityDrill ({0})", d);
				return false;
			}
		}

		
		public bool ModifyFileUploadStatus(FileUploadStatus s)
		{
			var curStatus = (from fus in db.FileUploadStatus where fus.id == s.id select fus).FirstOrDefault();
			try
			{
				if (curStatus != null)
				{
					Logger.Debug("Updating FileUpload Status ({0})", s);
					curStatus.BytesUploaded = s.BytesUploaded;
					curStatus.DateUploadComplete = s.DateUploadComplete;
					curStatus.DateUploadStart = s.DateUploadStart;
					curStatus.FileName = s.FileName;
					curStatus.Path = s.Path;
					curStatus.session = s.session;
					curStatus.TotalBytes = s.TotalBytes;
				}
				else
				{
					Logger.Debug("Adding FileUpload ({0})", s);
					s.id = Guid.NewGuid();
					db.FileUploadStatus.InsertOnSubmit(s);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying FileUpload Status ({0})", s);
				return false;
			}
		}

		public bool ModifyActivityScenario(ActivityScenario d)
		{
			var curActivityScenario = (from c in db.ActivityScenarios where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curActivityScenario != null)
				{
					Logger.Debug("Updating ActivityScenario ({0})", d);
					curActivityScenario.Activity = d.Activity;
					curActivityScenario.Scenario = d.Scenario;
					curActivityScenario.id = d.id;
				}
				else
				{
					Logger.Debug("Adding Alert ({0})", d);
					d.id = Guid.NewGuid();
					db.ActivityScenarios.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying ActivityScenario ({0})", d);
				return false;
			}
		}

		public bool ModifyActivityDestination(ActivityDestination d)
		{
			var curActivityDestination = (from c in db.ActivityDestinations where c.ID == d.ID select c).FirstOrDefault();
			try
			{
				if (curActivityDestination != null)
				{
					Logger.Debug("Updating ActivityDestination ({0})", d);
					curActivityDestination.Activity = d.Activity;
					curActivityDestination.Destination= d.Destination;
					curActivityDestination.ID = d.ID;
				}
				else
				{
					Logger.Debug("Adding Alert ({0})", d);
					d.ID = Guid.NewGuid();
					db.ActivityDestinations.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying ActivityDestination ({0})", d);
				return false;
			}
		}

		public bool ModifyActivityPassanger(ActivityPassanger d)
		{
			var curActivityPassanger = (from c in db.ActivityPassangers where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curActivityPassanger != null)
				{
					Logger.Debug("Updating ActivityScenario ({0})", d);
					curActivityPassanger.Activity = d.Activity;
					curActivityPassanger.Comments = d.Comments;
					curActivityPassanger.Person = d.Person;
					curActivityPassanger.id = d.id;
				}
				else
				{
					Logger.Debug("Adding Alert ({0})", d);
					d.id = Guid.NewGuid();
					db.ActivityPassangers.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying ActivityPassangers ({0})", d);
				return false;
			}
		}

		public bool ModifyScenario(Scenario d)
		{
			var curScenario = (from c in db.Scenarios where c.id == d.id select c).FirstOrDefault();
			try
			{
				if (curScenario != null)
				{
					Logger.Debug("Updating Scenario ({0})", d);
					curScenario.Name = d.Name;
					curScenario.Code = d.Code;
					curScenario.Deleted = d.Deleted;
				}
				else
				{
					Logger.Debug("Adding Scenario ({0})", d);
					d.id = Guid.NewGuid();
					db.Scenarios.InsertOnSubmit(d);
				}
				db.SubmitChanges();
				return true;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Modifying Scenario ({0})", d);
				return false;
			}
		}

		public ActivityID addModifyActivity(Activity t)
		{
			ActivityID retval = new ActivityID();	
			//Logger.Dump<Activity>(t);
			//Logger.Debug("entering DBI add Modify Activity Trace {0}",ServiceStack.Text.JsvFormatter.Dump<Activity>(t) );
			Logger.Debug("entering DBI add Modify Activity Trace {0}","" );
			if (t.LogNo == null) t.LogNo = 1;
            
			var curActivity = (from Activity activity in db.Activities where activity.id == t.id select activity ).FirstOrDefault();
			try
			{
				if (curActivity != null)
				{
					curActivity.ActivityType = t.ActivityType;
					curActivity.AirTemp = t.AirTemp;
					curActivity.BarometicPresure = t.BarometicPresure;
					curActivity.Date = t.Date;
					curActivity.DepartTime = t.DepartTime;
					curActivity.Maintenance = t.Maintenance;
					curActivity.MaintenanceComment = t.MaintenanceComment;
					curActivity.MaintenanceInitial = t.MaintenanceInitial;
					curActivity.MonthlyChecks = t.MonthlyChecks;
					curActivity.MonthlyChecksComment = t.MonthlyChecksComment;
					curActivity.MonthlyChecksInitial = t.MonthlyChecksInitial;
					curActivity.POB = t.POB;
					curActivity.RadioCheck = t.RadioCheck;
					curActivity.RadioCheckComments = t.RadioCheckComments;
					curActivity.RadioCheckInitial = t.RadioCheckInitial;
					curActivity.ReturnTime = t.ReturnTime;
					curActivity.SafetyCheck = t.SafetyCheck;
					curActivity.SafetyCheckComment = t.SafetyCheckComment;
					curActivity.SafetyCheckInitial = t.SafetyCheckInitial;
					curActivity.Skipper = t.Skipper;
					curActivity.StartupProcedures = t.StartupProcedures;
					curActivity.StartupProceduresComment = t.StartupProceduresComment;
					curActivity.StatupProceduresInitial = t.StatupProceduresInitial;
					curActivity.Actions = t.Actions;
					curActivity.CompletedBy = t.CompletedBy;
					curActivity.SafetyOfficerAdvised = t.SafetyOfficerAdvised;
					curActivity.DateSafetyOfficerAdvised = t.DateSafetyOfficerAdvised;
					curActivity.EngineFuelDollars = t.EngineFuelDollars;
					curActivity.EnginePortOilAdded = t.EnginePortOilAdded;
					curActivity.EnginePortRunHours = t.EnginePortRunHours;
					curActivity.EnginePortTotalHours = t.EnginePortTotalHours;
					curActivity.EngineFuelLitres = t.EngineFuelLitres;
					curActivity.EngineStarboardOilAdded = t.EngineStarboardOilAdded;
					curActivity.EngineStarboardRunHours = t.EngineStarboardRunHours;
					curActivity.EngineStarboardTotalHours = t.EngineStarboardTotalHours;
					curActivity.FirepumpStarted = t.FirepumpStarted;
					curActivity.NavHazards = t.NavHazards;
					curActivity.NavHazardsDetails = t.NavHazardsDetails;
					curActivity.PostOperationChecks = t.PostOperationChecks;
					curActivity.SecondaryWeather = t.SecondaryWeather;
					
					//curActivity.ActivityCourses = t.ActivityCourses;
					//curActivity.ActivityCrews = t.ActivityCrews;
					curActivity.UnusualWeather = t.UnusualWeather;
					curActivity.WaterTemp = t.WaterTemp;
					curActivity.Weather = t.Weather;
					curActivity.WeeklyChecks = t.WeeklyChecks;
					curActivity.WeeklyChecksComment = t.WeeklyChecksComment;
					curActivity.WeeklyChecksInitial = t.WeeklyChecksInitial;
					curActivity.Notes = t.Notes;
             
					var tCount = curActivity.ActivityMembers.Count;

					db.ActivityMembers.DeleteAllOnSubmit(curActivity.ActivityMembers);
					curActivity.ActivityMembers = new System.Data.Linq.EntitySet<ActivityMember>();

                    var portEngineid = (from crve in db.CRV_Engines where crve.CRV == t.CRV && crve.Active && crve.Position == 1 select crve.Engine).FirstOrDefault();
                    var starboardEngineid = (from crve in db.CRV_Engines where crve.CRV == t.CRV && crve.Active && crve.Position == 2 select crve.Engine).FirstOrDefault();
                    var middleEngineid = (from crve in db.CRV_Engines where crve.CRV == t.CRV && crve.Active && crve.Position == 3 select crve.Engine).FirstOrDefault();

                    if (portEngineid != new Guid())
                    {
                        var portEngine = (from e in db.Engines where e.id == portEngineid select e).FirstOrDefault();
                        t.EnginePort = portEngineid;
                        if (t.EnginePortTotalHours.HasValue && t.EnginePortTotalHours.Value > portEngine.Hours) portEngine.Hours = t.EnginePortTotalHours.Value;

                    }
                    if (starboardEngineid != new Guid())
                    {
                        t.EngineStarboard = starboardEngineid;
                        var starboardEngine = (from e in db.Engines where e.id == starboardEngineid select e).FirstOrDefault();
                        if (t.EngineStarboardTotalHours.HasValue && t.EngineStarboardTotalHours.Value > starboardEngine.Hours) starboardEngine.Hours = t.EngineStarboardTotalHours.Value;
                    }
                    if (middleEngineid != new Guid())
                    {
                        t.EngineMiddle = middleEngineid;
                        var middleEngine = (from e in db.Engines where e.id == middleEngineid select e).FirstOrDefault();
                        //if (t.EngineMiddle.HasValue && t.EngineMiddle.Value > middleEngine.Hours) middleEngine.Hours = t.EngineMiddle.Value;                         
                    }

					Logger.Debug("Submitting Changes");
					db.SubmitChanges();
					foreach (ActivityMember tc in t.ActivityMembers)
					{
						ActivityMember temp = new ActivityMember();
						temp.id = Guid.NewGuid();
						temp.MemberId = tc.MemberId;
						temp.Tripid = tc.Tripid;
					   
							curActivity.ActivityMembers.Add(temp);
							db.ActivityMembers.InsertOnSubmit(temp);
					}
					retval.id = curActivity.id;
					retval.LogNo = curActivity.LogNo;
			   }
				else
				{
					//t.LogNo = getNextActivityID();
					t.LogNo = getNextActivityID(t.Unit);
                    var portEngineid = (from crve in db.CRV_Engines where crve.CRV == t.CRV && crve.Active && crve.Position == 1 select crve.Engine).FirstOrDefault();
                    var starboardEngineid = (from crve in db.CRV_Engines where crve.CRV == t.CRV && crve.Active && crve.Position == 2 select crve.Engine).FirstOrDefault();
                    var middleEngineid= (from crve in db.CRV_Engines where crve.CRV == t.CRV && crve.Active && crve.Position == 3 select crve.Engine).FirstOrDefault();
                                        
                    if (portEngineid != new Guid())
                    {
                        var portEngine = (from e in db.Engines where e.id == portEngineid select e).FirstOrDefault();
                        t.EnginePort = portEngineid;
                        if (t.EnginePortTotalHours.HasValue && t.EnginePortTotalHours.Value > portEngine.Hours) portEngine.Hours = t.EnginePortTotalHours.Value;                         
                   
                    }
                    if (starboardEngineid != new Guid())
                    {
                        t.EngineStarboard = starboardEngineid;
                        var starboardEngine = (from e in db.Engines where e.id == starboardEngineid select e).FirstOrDefault(); 
                        if (t.EngineStarboardTotalHours.HasValue && t.EngineStarboardTotalHours.Value > starboardEngine.Hours) starboardEngine.Hours = t.EngineStarboardTotalHours.Value;                         
                    }
                    if (middleEngineid != new Guid())
                    {
                        t.EngineMiddle = middleEngineid;
                        var middleEngine = (from e in db.Engines where e.id == middleEngineid select e).FirstOrDefault();
                        //if (t.EngineMiddle.HasValue && t.EngineMiddle.Value > middleEngine.Hours) middleEngine.Hours = t.EngineMiddle.Value;                         
                    }
					db.Activities.InsertOnSubmit(t);


					foreach (ActivityMember tc in t.ActivityMembers)
					{
						db.ActivityMembers.InsertOnSubmit(tc);
					}
				}
				Logger.Debug("submitting changes");
					db.SubmitChanges();
					retval.id = t.id;
					retval.LogNo = t.LogNo;
				return retval;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Adding Activity");
				return null;
			}
		}
		public Activity newActivity()
		{
			return new Activity { AirTemp=0};
		}
		public List<ddPair>getDropDown(string objectname)
		{
			var retval = new List<ddPair>();
			switch (objectname.ToLower())
			{
				case "port":
					retval = (from p in db.TidePorts select (new ddPair { id = p.ID, desc = p.Name })).ToList<ddPair>();
					break;
				case "primaryport":
					retval = (from p in db.TidePorts where p.isPrimaryPort.Value select (new ddPair { id = p.ID, desc = p.Name })).ToList<ddPair>();
					break;

				}
			return retval;
		}

		public List<CoastGuardMember> getSkippers(int Sessionid)
		{
			var tempval = new List<CoastGuardMember>();
			try
			{
				var myUnit = (from s in db.Sessions where s.id == Sessionid select s.Unit).FirstOrDefault();
				tempval = (from m in db.CoastGuardMembers where m.Unit == myUnit && m.Skipper == true select m).ToList<CoastGuardMember>();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}
		public List<Course> getCourses()
		{
			var tempval = new List<Course>();
			try
			{
				tempval = (from c in db.Courses select c).ToList<Course>();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}
		public List<Course> getCourses(int SessionId)
		{
			var tempval = new List<Course>();
			try
			{
				var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
				tempval = (from c in db.Courses where c.Unit == unit select c).ToList<Course>();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}
		public List<Drill> getDrills(Guid? unit)
		{
			var tempval = new List<Drill>();
			try
			{
				if (unit.HasValue)
				{
					tempval = (from c in db.Drills where c.Unit == unit select c).ToList<Drill>();
				}
				else
				{
					tempval = (from c in db.Drills select c).ToList<Drill>();
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}
		public List<ActivityType> getActivityTypes(Guid? unit)
		{
			var tempval = new List<ActivityType>();
			try
			{
				if (unit.HasValue)
				{
					tempval = (from c in db.ActivityTypes where c.Unit == unit select c).ToList<ActivityType>();
				}
				else
				{
					tempval = (from c in db.ActivityTypes select c).ToList<ActivityType>();
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}

        public List<CoastguardRescueVessel> getVessels(Guid? unit)
        {
            var tempval = new List<CoastguardRescueVessel>();
            try
            {
                if (unit.HasValue)
                {
                    tempval = (from c in db.CoastguardRescueVessels where c.Unit == unit select c).ToList<CoastguardRescueVessel>();
                }
                else
                {
                    tempval = (from c in db.CoastguardRescueVessels select c).ToList<CoastguardRescueVessel>();
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
            return tempval;

        }
		public List<Procedure> getProcedures(int SessionId)
		{
			var tempval = new List<Procedure>();
			try
			{
				var unit = (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault();
				tempval = (from c in db.Procedures where c.Unit == unit select c).ToList<Procedure>();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}

		public List<ProcedureLastCompleted> getActivityProcedures(Guid? unit)
		{
			var tempval = new List<ProcedureLastCompleted>();
			try
			{
				if (unit.HasValue)
				{
					var procList = (from c in db.ProcedureLastCompleteds where c.Unit == unit && c.Type == 0 select c).ToList<ProcedureLastCompleted>();
					procList.Union((from c in db.ProcedureLastCompleteds where c.Unit == unit && c.Type == 1 && (c.LastDateCompleted == null || SqlMethods.DateDiffMonth(System.DateTime.Now, c.LastDateCompleted) >= 1) select c).ToList<ProcedureLastCompleted>());
					procList.Union((from c in db.ProcedureLastCompleteds where c.Unit == unit && c.Type == 2 select c).ToList<ProcedureLastCompleted>());
					tempval = procList;
				}
				else
				{
					var procList = (from c in db.ProcedureLastCompleteds where c.Type == 0 select c).ToList<ProcedureLastCompleted>();
					procList.Union((from c in db.ProcedureLastCompleteds where c.Type == 1 && (c.LastDateCompleted == null || SqlMethods.DateDiffMonth(System.DateTime.Now, c.LastDateCompleted) >= 1) select c).ToList<ProcedureLastCompleted>());
					procList.Union((from c in db.ProcedureLastCompleteds where c.Type == 2 select c).ToList<ProcedureLastCompleted>());
					tempval = procList;
				}

				
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}

		public Guid? getUnit(int SessionId)
		{
			return (from s in db.Sessions where s.id == SessionId select s.Unit).FirstOrDefault(); 
		}


		public List<Alert> getAlerts(Guid? unit)
		{
			var tempval = new List<Alert>();
			try
			{
				if (unit.HasValue)
				{
					tempval = (from c in db.Alerts where c.Deleted != true && c.Unit == unit select c).ToList<Alert>();
				}
				else
				{
					tempval = (from c in db.Alerts where c.Deleted != true select c).ToList<Alert>();
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}
		public List<Scenario> getScenarios(Guid? unit)
		{
			var tempval = new List<Scenario>();
			try
			{
				if (unit.HasValue)
				{
					tempval = (from c in db.Scenarios where c.Unit == unit select c).ToList<Scenario>();
				}
				else
				{
					tempval = (from c in db.Scenarios  select c).ToList<Scenario>();
				}
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}


		
		public List<CoastGuardMember> getCrew(int SessionID)
		{
			var tempval = new List<CoastGuardMember>();
			try
			{
				var myUnit = (from s in db.Sessions where s.id == SessionID select s.Unit).FirstOrDefault();
				tempval = (from cgm in db.CoastGuardMembers
						   join ts in db.TripStatistics on cgm.id equals ts.Userid into j1
						   from myj in j1.DefaultIfEmpty()
						   where cgm.Unit == myUnit
						   orderby myj.TripCount descending
						   select (cgm)
						   ).ToList<CoastGuardMember>();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return tempval;

		}
		public CoastGuardMember getCrewMember(Guid id)
		{
			var tempval = new CoastGuardMember();
			try
			{
				tempval = (from m in db.CoastGuardMembers where m.id == id select (m)).FirstOrDefault();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "In getCrewmember");
				throw ex;
			}
			return tempval;

		}

		public bool Logoff(int SessionId)
		{
			var s = (from session in db.Sessions where session.id == SessionId select session);
			try
			{
				if (s != null)
				{
					Logger.Debug("DBI removing Session {0}", SessionId);
					db.Sessions.DeleteAllOnSubmit(s);
					db.SubmitChanges();
				}
				return true;
			}
			catch (Exception ex)
			{
				
				Logger.ErrorEx(ex, "");
				return false;
			}
		}

		public int newSession(CoastGuardMember m)
		{
			int newId = 0;
			try
			{
				var curId = (from session in db.Sessions select session.id).Max();
				newId = curId + 1;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
				
			
			Session s = (from session in db.Sessions where session.Memberid == m.id select session).FirstOrDefault();

			if (s == null)
			{
				
				s = new Session { Memberid = m.id,id=newId,Unit=m.Unit };
				try
				{
					var settings = (from MemberSetting ms in db.MemberSettings select ms).FirstOrDefault();
					if (settings != null)
					{
						s.LogEnabled = settings.LogEnabled.HasValue?settings.LogEnabled.Value:false;
						s.LogLevel = settings.LogLevel.HasValue?settings.LogLevel.Value:0;
					}
				}
				catch (Exception ex)
				{
					Logger.ErrorEx(ex, "");
				}
				db.Sessions.InsertOnSubmit(s);
				db.SubmitChanges();
			}

			return s.id;
		}

		public MemberSetting getMemberSetting(Guid id)
		{
			try
			{
				var retval =(from MemberSetting ms in db.MemberSettings where ms.MemberId==id select ms ).FirstOrDefault();
				if (retval == null)
				{
					retval = new MemberSetting();
					retval.MemberId = id;
				}
				return retval;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
				return null;
			}
		}


		private DateTime tsToDateTime(string s,DateTime dt)
		{
			DateTime retval = new DateTime(dt.Year, dt.Month, dt.Day);
			var ts = s.Split(':');
			retval = retval.AddHours(Convert.ToInt32(ts[0]));
			retval = retval.AddMinutes(Convert.ToInt32(ts[1]));
			return retval;
		}

		public PortTideTime getPortTimes(string Port,DateTime dt)
		{
			var p = (from tp in db.TidePorts where tp.Name == Port && tp.isPrimaryPort.Value select tp).FirstOrDefault();
			if (p == null) return null;
			return getPortTimes(p.PrimaryPort.Value,dt);
		}

	   /* public bool updateNextPrev(System.Windows.Forms.Label lbl,System.Windows.Forms.Form frm){
			var plist = (from ptt in db.PortTideTimes where ptt.NH1 == null select ptt).ToList();
			foreach (PortTideTime p in plist)
			{
				var temp = getPortTimes(p.Port.Value, p.Date);
				lbl.Text = p.Date.ToLongDateString();
				frm.Refresh();
			}
			try
			{
				db.SubmitChanges();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}

			var plist2 = (from ptt in db.PortTideTimes where ptt.PH1 == null select ptt).ToList();
			foreach (PortTideTime p in plist2)
			{
				var temp = getPortTimes(p.Port.Value, p.Date);
				lbl.Text = p.Date.ToLongDateString();
				frm.Refresh();
			}
			try
			{
				db.SubmitChanges();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return true;
		}

		*/



		public PortTideTime getPortTimes(Guid Port,DateTime dt){
			var retval = new PortTideTime();
			var ptt = (from pt in db.PortTideTimes where pt.Port == Port && pt.Date == dt select pt).FirstOrDefault();
			if (ptt == null) return null;
			if (ptt.NH1 == null)
			{
				var nextTide = (from tt in db.PortTideTimes where tt.Date == ptt.Date.AddDays(1) select tt).FirstOrDefault();
				if (nextTide != null)
				{
					ptt.NH1 = nextTide.H1;
					ptt.NH2 = nextTide.H2;
					ptt.NTime1 = nextTide.Time1;
					ptt.NTime2 = nextTide.Time2;
					try
					{
						db.SubmitChanges();
					}
					catch (Exception ex)
					{
						Logger.ErrorEx(ex, "Submitting Changes to DB");
					}

				}
			}
			if (ptt.PH1 == null)
			{
				var prevTide = (from tt in db.PortTideTimes where tt.Date == ptt.Date.AddDays(-1) select tt).FirstOrDefault();
				if (prevTide != null)
				{
					if (prevTide.Time4.HasValue)
					{
						ptt.PH1 = prevTide.H4;
						ptt.PH2 = prevTide.H3;
						ptt.PTime1 = prevTide.Time4;
						ptt.PTime2 = prevTide.Time3;

					}
					else
					{
						ptt.PH1 = prevTide.H3;
						ptt.PH2 = prevTide.H2;
						ptt.PTime1 = prevTide.Time3;
						ptt.PTime2 = prevTide.Time2;
					}
					try
					{
						db.SubmitChanges();
					}
					catch (Exception ex)
					{
						Logger.ErrorEx(ex, "Submitting Changes to DB");
					}
				}
			}
			return ptt;
			
		}

		public List<PortTideTime> getTideHeights(DateTime dt, List<String> ports)
		{
			var retval = new List<PortTideTime>();
			foreach (string s in ports)
			{
				Logger.Debug("Processing Port:{0}", s);
				var p = processPort(dt, s);


				if (p != null)
				{
					Logger.Debug("Processing Port:{0} TideHeights[Port({1}) Date({2}) Time({3}) ", s, p.Port, p.Date, p.Time1);
					retval.Add(p);
				}
				else
				{
					Logger.Debug("Processing Port:{0} No Tide Heights found ", s);
				};
			}
			return retval;
		}

		public List<PortTideTime> getTideHeights(DateTime dt,List<Guid> ports) {
			var retval = new List<PortTideTime>();
			foreach (Guid g in ports)
			{
				var p = processPort(dt,g);
				if (p != null) { retval.Add(p); };
			}
			return retval;
		}

		private PortTideTime processPort(DateTime dt, Guid p)
		{
			var port = (from tp in db.TidePorts where tp.ID == p select tp).FirstOrDefault();
			if (port == null) return null;
			var ptt = (from pttdb in db.PortTideTimes where pttdb.Port == port.ID && pttdb.Date == dt select pttdb).FirstOrDefault();
			if (ptt == null) return null;
			if (port.isPrimaryPort.Value) return ptt;
			return getPortAddjustment(ptt,port);            //ptt.
			
		}

		public DateTime DateOnly(DateTime inDt)
		{
			return new DateTime(inDt.Year, inDt.Month, inDt.Day);
		}

		private PortTideTime processPort(DateTime dt, string p)
		{

            try
            {
                var port = (from tp in db.TidePorts where tp.Name == p select tp).FirstOrDefault();

                if (port == null)
                {
                    Logger.Debug("No port found for port:{0}", p);
                    return null;
                }
                var portlookup = port.isPrimaryPort.Value ? port.ID : port.PrimaryPort;
                var ptt = (from pttdb in db.PortTideTimes where pttdb.Port == portlookup && pttdb.Date == dt select pttdb).FirstOrDefault();
                if (ptt == null)
                {

                    Logger.Debug("No port times found for port [{0}({2})] primary port [{3}] on [{1}]", p, dt, port.ID, port.PrimaryPort);
                    return null;
                }
                var dlst = ptt.Copy();
                if (ptt.Time1.Value.IsDaylightSavingTime())
                {

                    if (dlst.Time1.HasValue) dlst.Time1 = ptt.Time1.Value.AddHours(1);
                    if (dlst.Time2.HasValue) dlst.Time2 = ptt.Time2.Value.AddHours(1);
                    if (dlst.Time3.HasValue) dlst.Time3 = ptt.Time3.Value.AddHours(1);
                    if (dlst.Time4.HasValue) dlst.Time4 = ptt.Time4.Value.AddHours(1);

                }
                Logger.Debug("Ptt Date[{5}] Port[{0}] time1[{1}] time2[{2}] time3[{3}] time4[{4}]", ptt.Port, ptt.Time1, ptt.Time2, ptt.Time3, ptt.Time4, ptt.Date);
                Logger.Debug("Ptt Date[{5}] Port[{0}] time1[{1}] time2[{2}] time3[{3}] time4[{4}]", dlst.Port, dlst.Time1, dlst.Time2, dlst.Time3, dlst.Time4, dlst.Date);
                if (port.isPrimaryPort.Value)
                {

                    return dlst;
                }
                Logger.Debug("Calling get adjustment for Port[{0}]", p);
                //return ptt;
                return getPortAddjustment(dlst.Copy(), port);            //ptt.
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "In ProcessPort:", p);
                throw;
            }
		}
		private PortTideTime getPortAddjustment(PortTideTime ptt, TidePort p)
		{
			
			//ptt.Date = pttin.Date;
			//ptt.Time1 = pttin.Time1;

			var primary = (from tp in db.TidePorts where tp.ID == p.PrimaryPort select tp).FirstOrDefault();
			Logger.Debug("Port({0})", p);
			Logger.Debug("Primary Port({0})",primary);
			ptt.Time1 = calcTime(p.HW.Value, p.LW.Value, ptt.H1.Value, ptt.Time1.Value);

			if (ptt.Time1 < ptt.Date)
			{
				ptt.H1 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H2.Value);
				ptt.H2 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H3.Value);
				ptt.Time1 = calcTime(p.HW.Value, p.LW.Value, ptt.H2.Value, ptt.Time2.Value);
				ptt.Time2 = calcTime(p.HW.Value, p.LW.Value, ptt.H2.Value, ptt.Time3.Value);

				if (ptt.H4.HasValue)
				{
					ptt.H3 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H4.Value);
					ptt.Time3 = calcTime(p.HW.Value, p.LW.Value, ptt.H3.Value, ptt.Time4.Value);
					ptt.H4 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.NH1.Value);
					ptt.Time4 = calcTime(p.HW.Value, p.LW.Value, ptt.H3.Value, ptt.NTime1.Value);
					ptt.NH1 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.NH2.Value);
					ptt.NTime1 = calcTime(p.HW.Value, p.LW.Value, ptt.H3.Value, ptt.NTime2.Value);
					ptt.NH2 = null;
					ptt.NTime2 = null;
				}
				else
				{
					ptt.H3 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.NH1.Value);
					ptt.Time3 = calcTime(p.HW.Value, p.LW.Value, ptt.H3.Value, ptt.NTime1.Value);
					ptt.NH1 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.NH2.Value);
					ptt.NTime1 = calcTime(p.HW.Value, p.LW.Value, ptt.H3.Value, ptt.NTime2.Value);
					ptt.NTime2 = null;
					ptt.NH2 = null;
				}
			}
			else
			{
				ptt.H1 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H1.Value);
				ptt.H2 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H2.Value);
				ptt.H3 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H3.Value);
				if (ptt.H4.HasValue) ptt.H4 = calcHeight(primary.MSL.Value, primary.MLWS.Value, primary.MHWS.Value, p.MSL.Value, p.MLWS.Value, p.MHWS.Value, ptt.H4.Value);

				ptt.Time2 = calcTime(p.HW.Value, p.LW.Value, ptt.H2.Value, ptt.Time2.Value);
				ptt.Time3 = calcTime(p.HW.Value, p.LW.Value, ptt.H3.Value, ptt.Time3.Value);
				if (ptt.H4.HasValue) ptt.Time4 = calcTime(p.HW.Value, p.LW.Value, ptt.H4.Value, ptt.Time4.Value);


			}
			return ptt;
		}
		/*
		 function adjustheight(pmsl,pmlws,pmhws,smsl,smlws,smhws,heights)
{
newHeight = new Array();
if (pmsl == 0) return heights;
for (var i in heights)
{
var t1 =  toNum(heights[i]) - toNum(pmsl);
var t2 = toNum(smhws) - toNum(smlws);
var t3 = toNum(pmhws) - toNum(pmlws);

newHeight[i] = toNum(smsl) + (t1 * t2 / t3);

}
return newHeight;
}

function adjusttime(hta,lta,heights,times)
{
newTime = new Array();
for (var i in heights)
{
	if (heights[i] < 1.7){
		newTime[i]=times[i] +new Number(lta);
	} else {
		newTime[i]=times[i] +new Number(hta); 
	}
//	if (newTime[i] < 0) newTime[i] += 24; 
}
		 */
		private DateTime calcTime(decimal hta,decimal lta,decimal height,DateTime curtime) {
			if (height < Convert.ToDecimal(1.7))
			{
				return curtime.AddMinutes(Convert.ToDouble(lta*60));
			}
			else
			{
				return curtime.AddMinutes(Convert.ToDouble(hta*60));
			}
		}
		private decimal calcHeight(Decimal pmsl,Decimal pmlws,Decimal pmhws,Decimal smsl,Decimal smlws,Decimal smhws,Decimal PrimaryHeight)
		{
			if (pmsl == 0) return PrimaryHeight;
			var t1 = PrimaryHeight - pmsl;
			var t2 = smhws - smlws;
			var t3 = pmhws - pmlws;
			return smsl + (t1 * t2 / t3);
		}



		public bool addPort(string[] ts)
		{
			CultureInfo cultureInfo   = Thread.CurrentThread.CurrentCulture;
			TextInfo textInfo = cultureInfo.TextInfo;
			
			// Check if Port Exists
			bool isPrimary = (string.Compare(ts[0].ToLower(), ts[1].ToLower()) == 1);
			if (string.IsNullOrEmpty(ts[7]) || string.IsNullOrEmpty(ts[8])) return false;
			var ppid = (from p in db.TidePorts where p.Name.ToLower() == ts[1].ToLower() select p.ID).FirstOrDefault();
			var ep = (from p in db.TidePorts where p.Name.ToLower() == ts[0].ToLower() select p).FirstOrDefault();
			if (ep != null)
			{
					ep.Name = textInfo.ToTitleCase(ts[0]);
					ep.PrimaryPort = ppid;
					ep.Area = textInfo.ToTitleCase(ts[2]);
					ep.HW = timeToDec(ts[7],5);
					//isPrimaryPort = false;
					ep.Latitude = Convert.ToDecimal(ts[3] + "." + ts[4]);
					ep.Longitude = Convert.ToDecimal(ts[5] + "." + ts[6]);
					ep.LW = timeToDec(ts[8],5);
					ep.MHWN = stringToDec(ts[10]);
					ep.MHWS = stringToDec(ts[9]);
					ep.MLWN = stringToDec(ts[11]);
					ep.MLWS = stringToDec(ts[12]);
					ep.MSL = stringToDec(ts[13]);
				
			}
			else
			{
				var pid = Guid.NewGuid();
				if (ppid == null) ppid = pid;
				var port = new TidePort
				{
					ID = pid,
					Name = textInfo.ToTitleCase(ts[0]),
					PrimaryPort = ppid,
					Area = textInfo.ToTitleCase(ts[2]),
					HW = timeToDec(ts[7],5),
					isPrimaryPort = false,
					Latitude = Convert.ToDecimal(ts[3] + "." + ts[4]),
					Longitude = Convert.ToDecimal(ts[5] + "." + ts[6]),
					LW = timeToDec(ts[8],5),
					MHWN = stringToDec(ts[10]),
					MHWS = stringToDec(ts[9]),
					MLWN = stringToDec(ts[11]),
					MLWS = stringToDec(ts[12]),
					MSL = stringToDec(ts[13])
					//PrimaryPort = ts[10],
					//ration  = Convert.ToDecimal(ts[11])
				};
				db.TidePorts.InsertOnSubmit(port);
			}
			try
			{
				db.SubmitChanges();
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "");
			}
			return true;
 
		}

		private Decimal stringToDec(string ts)
		{
			decimal retval = 0;
			try
			{
				var temp = Convert.ToDecimal(ts);
				return temp;
			}
			catch (Exception ex)
			{
				Logger.DebugEx(ex, "string to dec");
			}
			return retval;
		}

		private Decimal timeToDec(string ts){
			decimal retval = 0;
			try
			{
				var temp = Convert.ToDecimal(ts);
				var hour = (temp > 0)?Math.Floor(temp / 100):0-(Math.Floor(-1*temp/100));
				var min = temp % 100;
				var minDec = (min / 60);
				Logger.Debug("ts[{0}] temp[{1}] Hour [{2}] Min[{3}] mindec[{4}]", ts, temp, hour, min, minDec);
				return hour + minDec;
			}
			catch (Exception ex)
			{
				Logger.DebugEx(ex, "");
			}
			return retval;
	}
		private Decimal timeToDec(string ts,int pre)
		{
			decimal retval = 0;
			try
			{
				var temp = Convert.ToDecimal(ts);
				var hour = (temp > 0) ? Math.Floor(temp / 100) : 0 - (Math.Floor(-1 * temp / 100));
				var min = temp % 100;
				var minDec = Math.Round((min / 60),pre);
				Logger.Debug("ts[{0}] temp[{1}] Hour [{2}] Min[{3}] mindec[{4}]", ts, temp, hour, min, minDec);
				return hour + minDec;
			}
			catch (Exception ex)
			{
				Logger.DebugEx(ex, "");
			}
			return retval;
		}
		
		public bool addTideTimes(string Port,string[] ts)
		{
			var dt = new DateTime(Convert.ToInt32(ts[3]), Convert.ToInt32(ts[2]), Convert.ToInt32(ts[0]));
			var p = (from tp in db.TidePorts where tp.Name == Port && tp.isPrimaryPort.Value  select tp).FirstOrDefault();
			if (p == null) return false;
			var ptt = (from pttdb in db.PortTideTimes where p.ID == pttdb.Port && dt == pttdb.Date select pttdb).FirstOrDefault();
			if (ptt == null)
			{
				ptt = new PortTideTime();
				ptt.Port = p.ID;
				ptt.id = Guid.NewGuid();
				ptt.Date = dt;
			}
			ptt.H1 = Convert.ToDecimal(ts[5]);
			ptt.Time1 = tsToDateTime(ts[4],ptt.Date);
			ptt.H2 = Convert.ToDecimal(ts[7]);
			ptt.Time2 = tsToDateTime(ts[6], ptt.Date); 
			ptt.H3 = Convert.ToDecimal(ts[9]);
			ptt.Time3 = tsToDateTime(ts[8], ptt.Date);
			if (ts.Length > 10 && ts[11] != "")
			{
				ptt.H4 = Convert.ToDecimal(ts[11]);
				ptt.Time4 = tsToDateTime(ts[10], ptt.Date); ;
			}
				var prevTide = (from tt in db.PortTideTimes where tt.Date == ptt.Date.AddDays(-1) select tt).FirstOrDefault();
			/*
			if (prevTide != null) 
			{
				if (prevTide.Time4.HasValue)
				{
					ptt.PH1=prevTide.H4;
					ptt.PH2=prevTide.H3;
					ptt.PTime1=prevTide.Time4;
					ptt.PTime2=prevTide.Time3;

				} else {
					ptt.PH1=prevTide.H3;
					ptt.PH2=prevTide.H2;
					ptt.PTime1=prevTide.Time3;
					ptt.PTime2=prevTide.Time2;
				}
				prevTide.NH1 = ptt.H1;
				prevTide.NH2 = ptt.H2;
				prevTide.NTime1 = ptt.Time1;
				prevTide.NTime2 = ptt.Time2;
				try
				{
					db.SubmitChanges();
				}
				catch (Exception ex)
				{
					Logger.ErrorEx(ex, "Submitting Changes to DB");
				}
				
			}
			 
			var nextTide = (from tt in db.PortTideTimes where tt.Date == ptt.Date.AddDays(1) select tt).FirstOrDefault();
			if (nextTide != null)
			{
				ptt.NH1 = nextTide.H1;
				ptt.NH2 = nextTide.H2;
				ptt.NTime1 = nextTide.Time1;
				ptt.NTime2 = nextTide.Time2;
				if (ptt.Time4.HasValue)
				{
					nextTide.PH1 = ptt.H4;
					nextTide.PH2 = ptt.H3;
					nextTide.PTime1 = ptt.Time4;
					nextTide.PTime2 = ptt.Time3;

				}
				else
				{
					nextTide.PH1 = ptt.H3;
					nextTide.PH2 = ptt.H2;
					nextTide.PTime1 = ptt.Time3;
					nextTide.PTime2 = ptt.Time2;
				}
				try
				{
					db.SubmitChanges();
				}
				catch (Exception ex)
				{
					Logger.ErrorEx(ex, "Submitting Changes to DB");
				}
				
			}
			*/
			db.PortTideTimes.InsertOnSubmit(ptt);
			try
			{
				db.SubmitChanges(System.Data.Linq.ConflictMode.ContinueOnConflict);
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex,"Submitting Changes to DB");
			}

			return true;
		}

		public int DBIToInt(string inStr)
		{
			Logger.Debug("Converting string [{0}] to int", inStr);
			try
			{
				int retval = 0;
				int.TryParse(inStr, out retval);
				return retval;
			}
			catch (Exception ex)
			{
				Logger.ErrorEx(ex, "Occured converting string {0} to int", inStr);
				throw ex;
			}
		}

        public List<CoastguardRescueVesselDetail> getCRVs()
        {
            
            try
            {
                return (from c in db.CoastguardRescueVesselDetails select (c)).ToList<CoastguardRescueVesselDetail>();
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "In getCRVs");
                throw ex;
            }
            
        }

	}
}
