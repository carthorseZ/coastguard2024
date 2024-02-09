using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using DevTools.DatabaseDeployment;
using DevTools.DatabaseDeployment.ScriptExecution;
using DevTools.DatabaseDeployment.ScriptProviders;
using DevTools.DatabaseDeployment.VersionTrackers;
using DevTools.Helpers;
using Utils;

namespace DatabaseUpdater
{
    class Program
    {
        protected static Logger Logger = new Logger(typeof(Program));
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Logger.Configure();
            Logger.Debug("Starting");
            // Setup the default settings
            //string connectionString = @"Server=localhost;Database=CoastGuard;Trusted_connection=true;";
            string connectionString = Properties.Settings.Default.Connectionstring;
            // string connectionString = @"Data Source=localhost;Initial Catalog=PDProgram;Integrated Security=True";
            bool create = true;
            bool wait = true;

            // Read the command line arguments
            foreach (string commandLineArgument in args)
            {
                if (commandLineArgument.StartsWith("/connectionstring="))
                {
                    connectionString = commandLineArgument.Substring("/connectionstring=".Length);
                }
            }
            create = !args.Contains("/nocreate");
            wait = !args.Contains("/nowait");

            // Create the database if necessary
            if (create)
            {
                TraceHelper.TraceInformation("Checking to see if database already exists.");
                Logger.Info("Checking to see if database already exists.");
                if (!SqlDatabaseHelper.Exists(connectionString))
                {

                    TraceHelper.TraceInformation("Creating database '{0}'.", connectionString);
                    Logger.Info("Creating database '{0}'.", connectionString);
                    SqlDatabaseHelper.Create(connectionString);
                }
            }
            else
            {
                TraceHelper.TraceInformation("Checking to see if database already exists.");
                Logger.Info("Checking to see if database already exists.");
                if (!SqlDatabaseHelper.Exists(connectionString))
                {
                    TraceHelper.TraceError("The database {0} does not exist, so the upgrade cannot continue.", connectionString);
                    Logger.Error("The database {0} does not exist, so the upgrade cannot continue.", connectionString);

                    return -1;
                }
            }

            // Now perform the upgrade
            DatabaseUpgradeResult result = PerformUpgrade(connectionString);

            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------------------------------------");
            if (result.Error != null)
            {
                Console.WriteLine("The following error occurred:");
                Console.WriteLine(result.Error);
                Logger.Error("The Following Error occured {0}", result.Error);
                Console.WriteLine();
            }
            if (result.OriginalVersion != result.UpgradedVersion)
            {
                Console.WriteLine("The database was upgraded from version {0} to {1}", result.OriginalVersion, result.UpgradedVersion);
                Logger.Info("The database was upgraded from version {0} to {1}", result.OriginalVersion, result.UpgradedVersion);
                Console.WriteLine("The following scripts were executed: ");
                Logger.Info("The following scripts were executed: ");

                foreach (IScript script in result.Scripts)
                {
                    Console.WriteLine("  " + script.Name);
                    Logger.Info("  " + script.Name);
                }
            }
            else
            {
                Console.WriteLine("Your database appears to be up to date. The current version number is {0}", result.UpgradedVersion);
                Logger.Info("Your database appears to be up to date. The current version number is {0}", result.UpgradedVersion);
            }
            Console.WriteLine("DEVELOPERS: Were you expecting any other scripts to be run? Check that they are marked as embedded resources in the assembly.");
            Logger.Info("DEVELOPERS: Were you expecting any other scripts to be run? Check that they are marked as embedded resources in the assembly.");
            if (wait)
            {
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            return 0;
        }
        public static DatabaseUpgradeResult PerformUpgrade(string connectionString)
        {
            DatabaseUpgrader upgrader = new DatabaseUpgrader(new AD3DatabaseConfiguration(connectionString));
            return upgrader.PerformUpgrade();
        }
    }
    public class AD3DatabaseConfiguration : IDatabaseConfiguration
    {
        private string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="AD3DatabaseConfiguration"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AD3DatabaseConfiguration(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        /// <summary>
        /// Gets the script executor.
        /// </summary>
        public IScriptExecutor ScriptExecutor
        {
            get { return new SqlScriptExecutor(); }
        }

        /// <summary>
        /// Gets the script provider.
        /// </summary>
        public IScriptProvider ScriptProvider
        {
            get
            {
                return new EmbeddedSqlScriptProvider(
                    Assembly.GetExecutingAssembly(),
                    i => string.Format("{0}.UpgradeScripts.Script{1}.sql",
                        typeof(AD3DatabaseConfiguration).Namespace,
                        i.ToString().PadLeft(4, '0')));
            }
        }

        /// <summary>
        /// Gets the version tracker.
        /// </summary>
        public IVersionTracker VersionTracker
        {
            get { return new SchemaVersionsTableSqlVersionTracker(); }
        }
    }

}
