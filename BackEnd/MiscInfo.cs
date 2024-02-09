using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BackEnd
{
    class MiscInfo
    {
    }
    public class FindResult
    {
        public List<List<String>> data;

        //public List<JQDataTableTitle> titles;
        public List<String> titles;


    }
    public class UploadInfo
    {
        public bool IsReady { get; set; }
        public bool Complete { get; set; }
        public int ContentLength { get; set; }
        public int UploadedLength { get; set; }
        public string FileName { get; set; }
    }
    public class EditResult
    {
        public string Name;
        public List<List<String>> data;

        //public List<JQDataTableTitle> titles;
        public List<String> titles;
        public List<String> types;

    }

    public class EventMessage
    {
        public List<DBInterface.WCFEvent> Events;
        public object Data;
    }


    public class JSONObject
    {
        public string Name;
        public string ObjectStr;
    }
    public class JSONObjectAsStringList
    {
        public string Name;
        public List<String> ObjectStr;
    }

    public class JQDataTableTitle
    {
        public JQDataTableTitle(String name)
        {
            sTitle = name;
        }
        public String sTitle = "";
    }
    public class AlertInfo
    {
        public string AlertName;
        public string Text;
        public string Level;
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Name [{0}] Text[{1}] Level[{2}]", this.AlertName, this.Text, this.Level);
        }
    }
    public class ActivityDrillInfo
    {
        public Guid id;
        public Guid DrillId;
        public Guid Activity;
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Id [{0}] Drill[{1}] Activity[{2}]", this.id, this.DrillId, this.Activity);
        }
        public ActivityDrillInfo()
        {
        }
        public ActivityDrillInfo(DBInterface.ActivityDrill ad)
        {
            this.id = ad.id;
            this.DrillId = ad.DrillId;
            this.Activity = ad.Activity;
        }
        public DBInterface.ActivityDrill getActivityDrill()
        {
            var retval = new DBInterface.ActivityDrill();
            retval.id = this.id;
            retval.DrillId = this.DrillId;
            retval.Activity = this.Activity;
            return retval;
        }
    }



    public class ActivityScenarioInfo
    {
        public Guid id;
        public Guid ScenarioId;
        public Guid Activity;
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Id [{0}] Drill[{1}] Activity[{2}]", this.id, this.ScenarioId, this.Activity);

        }
        public ActivityScenarioInfo()
        {
        }
        public ActivityScenarioInfo(DBInterface.ActivityScenario inval)
        {
            this.id = inval.id;
            this.Activity = inval.Activity;
            this.ScenarioId = inval.Scenario;
        }
        public DBInterface.ActivityScenario getActivityScenario()
        {
            var retval = new DBInterface.ActivityScenario();
            retval.id = this.id;
            
            retval.Activity = this.Activity;
            retval.Scenario = this.ScenarioId;
            return retval;
        }
    }

    public class ActivityDestinationInfo
    {
        public Guid id;
        public Guid DestinationId;
        public Guid Activity;
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Id [{0}] Drill[{1}] Activity[{2}]", this.id, this.DestinationId, this.Activity);

        }
        public ActivityDestinationInfo()
        {
        }
        public ActivityDestinationInfo(DBInterface.ActivityDestination inval)
        {
            this.id = inval.ID;
            this.Activity = inval.Activity;
            this.DestinationId = inval.Destination;
        }
        public DBInterface.ActivityDestination getActivityDestination()
        {
            var retval = new DBInterface.ActivityDestination();
            retval.ID = this.id;

            retval.Activity = this.Activity;
            retval.Destination = this.DestinationId;
            return retval;
        }
    }

   


    public class ActivityPassangerInfo
    {
        public Guid id;
        public Guid Person;
        public Guid Activity;
        public String Comments;
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Id [{0}] Drill[{1}] Activity[{2}] Comments[{3}]", this.id, this.Person, this.Activity,this.Comments);

        }
        public ActivityPassangerInfo()
        {
        }
        public ActivityPassangerInfo(DBInterface.ActivityPassanger inval)
        {
            this.id = inval.id;
            this.Activity = inval.Activity;
            this.Person = inval.Person.HasValue?inval.Person.Value:new Guid();
            this.Comments = inval.Comments;
        }
        public DBInterface.ActivityPassanger getActivityPassanger()
        {
            var retval = new DBInterface.ActivityPassanger();
            retval.id = this.id;

            retval.Activity = this.Activity;
            if (this.Person == new Guid()) {
                retval.Person = null;
            } else {
                retval.Person = this.Person;
            }
            retval.Comments = this.Comments;
            
            return retval;
        }
    }
    public class eventInfo
    {
        public string eventName;
        public string eventData;
    }

    public class PersonInfo
    {
        public Guid id;
        public String FirstName;
        public String LastName;
        public String Title;
        public String HomePhone;
        public String WorkPhone;
        public String MobilePhone;
        public String Email;
        public DateTime? DateOfBirth;
        public bool Active;
        public PersonInfo()
        {
        }
        public PersonInfo(DBInterface.Person inVal)
        {
            this.Active = inVal.Active.HasValue?inVal.Active.Value:false;
            this.DateOfBirth = inVal.DateOfBirth;
            this.Email = inVal.Email;
            this.FirstName = inVal.FirstName;
            this.HomePhone = inVal.HomePhone;
            this.id = inVal.id;
            this.LastName = inVal.LastName;
            this.MobilePhone = inVal.MobilePhone;
            this.Title = inVal.Title;
            this.WorkPhone = inVal.WorkPhone;
        }
        public DBInterface.Person GetPerson()
        {
            var retval = new DBInterface.Person();
            retval.Active = this.Active;
            retval.DateOfBirth = this.DateOfBirth;
            retval.Email = this.Email;
            retval.FirstName = this.FirstName;
            retval.HomePhone = this.HomePhone;
            retval.id = this.id;
            retval.LastName = this.LastName;
            retval.MobilePhone = this.MobilePhone;
            retval.Title = this.Title;
            retval.WorkPhone = this.WorkPhone;
            return retval;
        }
    }

    public class MenuInfo
    {
        public string Name;
        public List<DBInterface.Custom_MenuOption> Options;

        public MenuInfo()
        {
            Options = new List<DBInterface.Custom_MenuOption>();
        }
    }

    public class ProcedureInfo
    {
        public string Name;
        public int Type;

        public DateTime DateCompleted;
        public Guid CompletedBy;
        public Guid id;
        public Guid ProcedureId;
        public Guid ActivityId;
        public String Comments;
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Name [{0}] Type[{1}] DateCompleted[{2}] CompletedBy [{3}] Comments[{4}]", this.Name, this.Type, this.DateCompleted,this.CompletedBy,this.Comments);
        }

        public DBInterface.ProcedureLog getProcedureLog()
        {
            var retval = new DBInterface.ProcedureLog();
            retval.ActivityId = this.ActivityId;
            retval.ProcedureId = this.ProcedureId;
            retval.Comments = this.Comments;
            retval.DateCompleted = this.DateCompleted;
            retval.id = this.id;
            retval.MemberId = this.CompletedBy;

            return retval;
        }

        
    }
}
