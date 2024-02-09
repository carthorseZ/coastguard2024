using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Utils;

namespace BackEnd
{
    public class ActivityInfo
    {
        private static readonly Logger Logger = new Logger(typeof(ActivityInfo));
        //public Guid id = Guid.NewGuid();
        public ActivityInfo()
        {
            using (DBInterface.DBIInterface DBI = new DBInterface.DBIInterface(Properties.Settings.Default.ConnectionString))
            {
               
                this.LogNo = string.Format("T{0}", DBI.getNextActivityID());
            }
        }
        public ActivityInfo(int SessionId)
        {
            using (DBInterface.DBIInterface DBI = new DBInterface.DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                this.LogNo = string.Format("T{0}", DBI.getNextActivityID(DBI.getUnit(SessionId)));
            }
        }
        public ActivityInfo(DBInterface.Activity t)
        {
            if (t == null) return;
            if (t.AirTemp.HasValue) this.AirTemp = t.AirTemp.Value;
            if (t.BarometicPresure.HasValue) this.BarometricPreasure = t.BarometicPresure.Value;
            //this.Crew = t.TripCrews;
            this.date = t.Date;
            if (t.DepartTime.HasValue) this.DepartTime = t.DepartTime.Value;
            this.id = t.id;
            if (t.Maintenance.HasValue) this.Maintenance = t.Maintenance.Value;
            this.MaintenanceComment = t.MaintenanceComment;
            this.MaintenanceInitial = t.MaintenanceInitial;
            if (t.MonthlyChecks.HasValue) this.MonthlyChecks = t.MonthlyChecks.Value;
            this.MonthlyChecksComment = t.MonthlyChecksComment;
            this.MonthlychecksInitial = t.MonthlyChecksInitial;
            if (t.POB.HasValue) this.POB = t.POB.Value;
            if (t.RadioCheck.HasValue) this.RadioCheck = t.RadioCheck.Value;
            this.RadioCheckComment = t.RadioCheckComments;
            this.RadioCheckInitial = t.RadioCheckInitial;
            if (t.ReturnTime.HasValue)this.ReturnTime = t.ReturnTime.Value;
            if (t.SafetyCheck.HasValue) this.SafetyCheck = t.SafetyCheck.Value;
            this.SafetyCheckComment = t.SafetyCheckComment;
            this.SafetyCheckInitial = t.SafetyCheckInitial;
            if (t.Skipper.HasValue) this.skipper = t.Skipper.Value;
            if (t.ActivityType.HasValue) this.ActivityType = t.ActivityType.Value;
            if (t.StartupProcedures.HasValue) this.StartupProcedures = t.StartupProcedures.Value;
            this.StartupProceduresComment = t.StartupProceduresComment;
            this.StartupProceduresInitial = t.StatupProceduresInitial;
            this.UnusualWeather = t.UnusualWeather;
            if (t.WaterTemp.HasValue) this.WaterTemp = t.WaterTemp.Value;
            this.Weather = t.Weather;
            this.SecondaryWeather = t.SecondaryWeather;
            if (t.WeeklyChecks.HasValue)this.WeeklyChecks = t.WeeklyChecks.Value;
            this.WeeklyChecksComment = t.WeeklyChecksComment;
            this.WeeklyChecksInitial = t.WeeklyChecksInitial;
            this.LogNo = t.LogNo.ToString();
            this.Actions = t.Actions;
            if (t.CompletedBy.HasValue) this.CompletedBy = t.CompletedBy.Value;
            this.DateSafetyOfficerAdvised = t.DateSafetyOfficerAdvised;
            if (t.EngineFuelLitres.HasValue) this.EngineFuelLitres = t.EngineFuelLitres.Value;
            if (t.EnginePortOilAdded.HasValue) this.EnginePortOilAdded = t.EnginePortOilAdded.Value;
            if (t.EnginePortRunHours.HasValue) this.EnginePortRunHours = t.EnginePortRunHours.Value;
            if (t.EnginePortTotalHours.HasValue) this.EnginePortTotalHours = t.EnginePortTotalHours.Value;
            if (t.EngineFuelDollars.HasValue) this.EngineFuelDollars = t.EngineFuelDollars.Value;
            if (t.EngineStarboardOilAdded.HasValue) this.EngineStarboardOilAdded = t.EngineStarboardOilAdded.Value;
            if (t.EngineStarboardRunHours.HasValue) this.EngineStarboardRunHours = t.EngineStarboardRunHours.Value;
            if (t.EngineStarboardTotalHours.HasValue) this.EngineStarboardTotalHours = t.EngineStarboardTotalHours.Value;
            this.Faults = t.Faults;
            if (t.FirepumpStarted.HasValue) this.FirepumpStarted = t.FirepumpStarted.Value;
            this.NavHazards = t.NavHazards.Value;
            if (t.NavHazardsDetails != null)
            {
                this.NazHazardsDetails = t.NavHazardsDetails;
            }

            if (t.VoyageDetails != null) this.VoyageDetails = t.VoyageDetails;
            if (t.Notes != null) this.Notes = t.Notes.ToString();
            if (t.Trainer != null) this.Trainer = t.Trainer.Value;
            if (t.Unit.HasValue)  this.Unit = t.Unit.Value;
            
            this.Crew = (from c in t.ActivityMembers select c.MemberId).ToList<Guid>();
            if (t.CRV.HasValue) this.CRV = t.CRV;
        }

        public Guid id = new Guid();
        public DateTime date = System.DateTime.Now;
        public Guid skipper;
        public Guid Trainer;
        public Guid? CRV; 
        public DateTime DepartTime = System.DateTime.Now;
        public DateTime ReturnTime = System.DateTime.Now;
        public Guid ActivityType;
        public int POB;
        public string LogNo;
        public string Weather;
        public string SecondaryWeather;
        public string UnusualWeather;
        public decimal WaterTemp;
        public decimal AirTemp;
        public decimal BarometricPreasure;
        public bool StartupProcedures;
        public string StartupProceduresComment;
        public string StartupProceduresInitial;
        public bool WeeklyChecks;
        public string WeeklyChecksComment;
        public string WeeklyChecksInitial;
        public bool MonthlyChecks;
        public string MonthlyChecksComment;
        public string MonthlychecksInitial;
        public bool Maintenance;
        public string MaintenanceComment;
        public string MaintenanceInitial;
        public bool SafetyCheck;
        public string SafetyCheckComment;
        public string SafetyCheckInitial;
        public bool RadioCheck;
        public string RadioCheckComment;
        public string RadioCheckInitial;
        public string VoyageDetails;
        public decimal EngineFuelDollars;
        public decimal EnginePortTotalHours;
        public decimal EnginePortRunHours;
        public decimal EnginePortOilAdded;
        public decimal EngineFuelLitres;
        public decimal EngineStarboardTotalHours;
        public decimal EngineStarboardRunHours;
        public decimal EngineStarboardOilAdded;
        public bool FirepumpStarted;
        public bool PostOperationChecks;
        public int NavHazards;
        public string NazHazardsDetails;
        public string Faults;
        public string Actions;
        public bool SafetyOfficerAdvised;
        public string Notes;
        public Guid Unit;
        //public DateTime DateSafetyOfficerAdvised = DateTime.MinValue;
        public DateTime? DateSafetyOfficerAdvised;

        public Guid CompletedBy;

        public List<Guid> Crew = new List<Guid>();
        public List<Guid> Courses = new List<Guid>();

        public override String ToString()
        {
 	        return base.ToString();
            
        }

        public string ToJSV()
        {
            //return basea.ToString();

            return ServiceStack.Text.JsvFormatter.Dump<ActivityInfo>(this);



        }

        public DBInterface.Activity getDBIActivity(int SessionID)
        {
            Guid unit;
            using (DBInterface.DBIInterface DBI = new DBInterface.DBIInterface(Properties.Settings.Default.ConnectionString))
            {

                unit = DBI.getUnit(SessionID).HasValue ? DBI.getUnit(SessionID).Value : Unit;


            }
            DBInterface.Activity retval = new DBInterface.Activity();
            if (id.CompareTo(new Guid()) == 0) { retval.id = Guid.NewGuid(); } else { retval.id = id; };
       //     retval.Actions = Actions;
            retval.AirTemp = AirTemp;
            retval.BarometicPresure = BarometricPreasure;
         //   retval.DateSafetyOfficerAdvised = DateSafetyOfficerAdvised;
            
            retval.Date = date;
            
            retval.DepartTime = DepartTime;
            retval.EngineFuelDollars = EngineFuelDollars;
            retval.EnginePortOilAdded = EnginePortOilAdded;
            retval.EnginePortRunHours = EnginePortRunHours;
            retval.EnginePortTotalHours = EnginePortTotalHours;
            retval.EngineFuelLitres = EngineFuelLitres;
            retval.EngineStarboardOilAdded = EngineStarboardOilAdded;
            retval.EngineStarboardRunHours = EngineStarboardRunHours;
            retval.EngineStarboardTotalHours = EngineStarboardTotalHours;
            retval.Faults = Faults;
            retval.FirepumpStarted = FirepumpStarted;
            retval.NavHazards = NavHazards;
            retval.NavHazardsDetails = NazHazardsDetails;
            retval.PostOperationChecks = PostOperationChecks;
            retval.SecondaryWeather = SecondaryWeather;
            retval.ReturnTime = ReturnTime;
            retval.POB = POB;
            retval.Weather = Weather;
            retval.UnusualWeather = UnusualWeather;
            retval.WaterTemp = WaterTemp;
            retval.Skipper = skipper;
            retval.ActivityType = ActivityType;
            retval.StartupProcedures = StartupProcedures;
            retval.StartupProceduresComment = StartupProceduresComment;
            retval.StatupProceduresInitial = StartupProceduresInitial;
            retval.WeeklyChecks = WeeklyChecks;
            retval.WeeklyChecksComment = WeeklyChecksComment;
            retval.WeeklyChecksInitial = WeeklyChecksInitial;
            retval.MonthlyChecks = MonthlyChecks;
            retval.MonthlyChecksComment = MonthlyChecksComment;
            retval.MonthlyChecksInitial = MonthlychecksInitial;
            retval.Maintenance = Maintenance;
            retval.MaintenanceComment = MaintenanceComment;
            retval.MaintenanceInitial = MaintenanceInitial;
            retval.SafetyCheck = SafetyCheck;
            retval.SafetyCheckComment = SafetyCheckComment;
            retval.SafetyCheckInitial = SafetyCheckInitial;
            retval.RadioCheck = RadioCheck;
            retval.RadioCheckComments = RadioCheckComment;
            retval.RadioCheckInitial = RadioCheckInitial;
            retval.DateSafetyOfficerAdvised = DateSafetyOfficerAdvised;
            retval.Notes = Notes;
            retval.Trainer = Trainer;
            retval.Unit = unit;
            retval.CRV = CRV;
            if (Crew != null)
            {
                Logger.Debug("Trip info crew count[{0}]", Crew.Count);
                foreach (Guid g in Crew)
                {
                    retval.ActivityMembers.Add(new DBInterface.ActivityMember { id = Guid.NewGuid(), MemberId = g, Tripid = retval.id });
                }
            }
            else
            {
                Logger.Debug("No Crew");
            }
            Logger.Debug("Courses");
            try
            {
                if (Courses != null)
                {
                    //Logger.Debug("Trip info crew count[{0}]", Crew.Count);
                    //foreach (Guid g in Courses)
                    //{
                    //    retval.ActivityMembers.Add(new DBInterface.ActivityMember { id = Guid.NewGuid(),Course = g, Trip = retval.id });
                    //}
                }
                else
                {
                    Logger.Debug("No Courses");
                }
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "");
            }
            retval.LogNo = 0;
            Logger.Debug("Processing LogNO");
            try
            {
                retval.LogNo = int.Parse(this.LogNo);
            }
            catch
            //catch (Exception ex)
            {
                //if (1!=1)
                //{

                //    Logger.ErrorEx(ex, "Converting to Integer");
                //}
            }

            return retval;

 
  }
 
  }

  }
