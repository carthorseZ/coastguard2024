using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Utils;

namespace BackEnd
{
    
    public class MemberInfo
    {
        private static readonly Logger Logger = new Logger(typeof(ActivityInfo));
        //public Guid id = Guid.NewGuid();
        public MemberInfo()
        {
        }
        public MemberInfo(DBInterface.CoastGuardMember cgm)
        {
            this.id = cgm.id;
            this.Home = cgm.Home;
            this.Mobile = cgm.Mobile;
            this.password = cgm.Password;
            this.Skipper = cgm.Skipper.HasValue?cgm.Skipper.Value:false;
            this.Trainee = cgm.Trainee.HasValue ? cgm.Trainee.Value : false;
            this.Operational = cgm.Operational.HasValue ? cgm.Operational.Value : false;
            this.Deleted = cgm.Deleted.HasValue ? cgm.Deleted.Value : false;
            this.Senior = cgm.Senior.HasValue ? cgm.Senior.Value : false;
            this.userid = cgm.UserId;
            this.Work = cgm.Work;
            this.Last_Name = cgm.Last_Name;
            this.First_Name = cgm.First_Name;
            this.email = cgm.email;
            this.unit = cgm.Unit.HasValue?cgm.Unit.Value:new Guid();
        }
        public Guid id = new Guid();
        public string userid = "";
        public string password;
        public string First_Name;
        public string Last_Name;
        public string Mobile;
        public string Home;
        public string Work;
        public string email;
        public bool Skipper;
        public bool Operational;
        public bool Trainee;
        public bool Deleted;
        public bool Senior;
        public bool LogEnabled;
        public int LogLevel;
        public Guid unit;
        public override string ToString()
        {
            //return basea.ToString();
            //Logger.Dump<MemberInfo>(this);
            return String.Format("ID [{0}] UserID [{1}] name[{2} {3}] Password[{4}] Mobile[{5}] Home[{6}] Work[{7}] Email[{8}] Skipper[{9}]", this.id, this.userid, this.First_Name, this.Last_Name, this.password, this.Mobile, this.Home, this.Work, this.email, this.Skipper,this.LogEnabled,this.LogLevel);

            
        }
        public string ToJSV()
        {
            //return basea.ToString();
            return ServiceStack.Text.JsvFormatter.Dump<MemberInfo>(this);            
        }
        public void updateSettings(DBInterface.MemberSetting ms)
        {
            this.LogEnabled = ms.LogEnabled.HasValue ? LogEnabled : false;
            this.LogLevel = ms.LogLevel.HasValue ? ms.LogLevel.Value : 0;
        }

        public DBInterface.MemberSetting getDBIMemberSetting()
        {
            DBInterface.MemberSetting retval = new DBInterface.MemberSetting();
            retval.MemberId = this.id;
            retval.LogEnabled = this.LogEnabled;
            retval.LogLevel = this.LogLevel;
            return retval;
        }

        public DBInterface.CoastGuardMember getDBIMember()
        {
            DBInterface.CoastGuardMember retval = new DBInterface.CoastGuardMember();
            retval.Senior = Senior;
            retval.Skipper = Skipper;
            retval.Trainee = Trainee;
            retval.UserId = userid;
            retval.Work = Work;
            retval.Password = password;
            retval.Operational = Operational;
            retval.Mobile = Mobile;
            retval.Last_Name = Last_Name;
            retval.id = id;
            retval.Home = Home;
            retval.First_Name = First_Name;
            retval.email = email;
            retval.Deleted = Deleted;
            retval.Unit = unit;
            return retval;
        }
        
 
  }
 
  }

  
