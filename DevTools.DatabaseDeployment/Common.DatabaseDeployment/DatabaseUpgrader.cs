using DevTools.DatabaseDeployment.ScriptProviders;
using DevTools.Helpers;
using Utils;
namespace DevTools.DatabaseDeployment
{
    using DatabaseDeployment.ScriptProviders;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class DatabaseUpgrader
    {
        
        protected static Logger Logger = new Logger(typeof(DatabaseUpgrader));
        private IDatabaseConfiguration _configuration;

        public DatabaseUpgrader(IDatabaseConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public DatabaseUpgradeResult PerformUpgrade()
        {
            int original = 0;
            int versionNumber = 0;
            int highestScriptVersion = 0;
            List<IScript> scripts = new List<IScript>();
            try
            {
                string connectionString = this._configuration.ConnectionString;
                TraceHelper.TraceInformation("Beginning database upgrade. Connection string is: '{0}'", new object[] { connectionString });
                Logger.Info("Beginning database upgrade. Connection string is: '{0}'", new object[] { connectionString });
                original = this._configuration.VersionTracker.RecallVersionNumber(connectionString);
                highestScriptVersion = this._configuration.ScriptProvider.GetHighestScriptVersion();
                versionNumber = original;
                while (versionNumber < highestScriptVersion)
                {
                    versionNumber++;
                    TraceHelper.TraceInformation("Upgrading to version: '{0}'", new object[] { versionNumber });
                    Logger.Info("Upgrading to version: '{0}'", new object[] { versionNumber });
                    using (TraceHelper.Indent())
                    {
                        IScript script = this._configuration.ScriptProvider.GetScript(versionNumber);
                        this._configuration.ScriptExecutor.Execute(connectionString, script);
                        this._configuration.VersionTracker.StoreUpgrade(connectionString, script);
                        scripts.Add(script);
                    }
                }
                TraceHelper.TraceInformation("Upgrade successful");

                return new DatabaseUpgradeResult(scripts, original, versionNumber, true, null);
            }
            catch (Exception exception)
            {
                Trace.TraceError("Upgrade failed", new object[] { exception });
                Logger.ErrorEx(exception, "Upgrade Fail");
                return new DatabaseUpgradeResult(scripts, original, versionNumber, false, exception);
            }
        }
    }
}

