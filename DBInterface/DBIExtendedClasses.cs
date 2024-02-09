using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
 
namespace DBInterface
{

    public class EngineStartHours
    {
        public decimal PortEngineStart;
        public decimal StarboardEngineStart;
        public decimal MiddleEngineStart;
    }
    
    public partial class TidePort
    {
        public override string ToString()
        {
            //return base.ToString();
            return String.Format("Name [{1} - {0}] HW[{2}] LW[{3}] MHWN[{4}] MLWN[{5}] MHWS[{6}] MLWS[{7}] MSL[{8}]", this.Name, this.Area, this.HW, this.LW, this.MHWN, this.MLWN, this.MHWS, this.MLWS, this.MSL);
        }
        public TidePort Copy()
        {
            var retval = new TidePort();
            return retval;
        }
    }
        
    public partial class CoastGuardMember
    {
        public override string ToString()
        {
            //return basea.ToString();
            // return String.Format("ID [{0}] UserID [{1}] name[{2} {3}] Password[{4}] Mobile[{5}] Home[{6}] Work[{7}] Email[{8}] Skipper[{9}]", this.id, this.UserId, this.First_Name, this.Last_Name, this.Password, this.Mobile, this.Home, this.Work, this.email,this.Skipper.HasValue?this.Skipper.Value:false);
            return JsonConvert.SerializeObject(this);
        }

    }
    public partial class Drill
    {
        public override string ToString()
        {
            //return basea.ToString();
            //return String.Format("ID [{0}] Code[{1}] Name[{2}] Deleted[{3}]", this.id, this.Code, this.Name, this.Deleted.HasValue?this.Deleted.Value:false );
            return JsonConvert.SerializeObject(this);
        }

    }
    public partial class Procedure
    {
        public override string ToString()
        {
            //return basea.ToString();
            //return String.Format("ID [{0}] Name[{1}] Deleted[{2}]", this.id,  this.Name, this.Deleted.HasValue ? this.Deleted.Value : false);
            return JsonConvert.SerializeObject(this);
        }

    }
    public partial class Alert
    {
        public override string ToString()
        {
            //return basea.ToString();
            //return String.Format("ID [{0}]  Name[{1}] Deleted[{2}] Fields[{3}] Level[{4}] Tables[{5}] Text[{6}] Where[{7}] ", this.id,  this.Name, this.Deleted.HasValue ? this.Deleted.Value : false,this.Fields,this.Level,this.Tables,this.Text,this.Where);
            return JsonConvert.SerializeObject(this);
        }

    }

    public partial class Engine
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public partial class CoastguardRescueVessel
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public partial class CoastGuardRescueVesselDetails
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public partial class PortTideTime
    {
        private static  Utils.Logger myLog = new Utils.Logger(typeof(PortTideTime));
        public override string ToString()
        {
            string json = JsonConvert.SerializeObject(this);
            //return base.ToString();
            //return string.Format("Port[{0}] Date[{1}]", Port, Date);
            return json;
        }
        public PortTideTime Copy()
        {
            // PortTideTime retval = (PortTideTime)this.MemberwiseClone();
            //Log("Copy Object [{0}]", this.ToString());
            myLog.Debug("Copy Object [{0}]",this.ToString());
            try
            {
                var retval = new PortTideTime
                {
                    id = this.id,

                    Date = new DateTime(this.Date.Ticks),
                    H1 = this.H1,
                    H2 = this.H2,
                    H3 = this.H3,
                    H4 = this.H4,
                    NH1 = this.NH1,
                    NH2 = this.NH2,
                    NTime1 = this.NTime1,
                    NTime2 = this.NTime2,
                    PH1 = this.PH1,
                    PH2 = this.PH2,
                    Port = this.Port,
                    PTime1 = this.PTime1.Value,
                    PTime2 = this.PTime2.Value,

                };
                if (this.Time1.HasValue) retval.Time1 = new DateTime(this.Time1.Value.Ticks);
                if (this.Time2.HasValue) retval.Time2 = new DateTime(this.Time2.Value.Ticks);
                if (this.Time3.HasValue) retval.Time3 = new DateTime(this.Time3.Value.Ticks);
                if (this.Time4.HasValue) retval.Time4 = new DateTime(this.Time4.Value.Ticks);
                return retval;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
