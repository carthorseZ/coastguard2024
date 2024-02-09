using DevTools.DatabaseDeployment.ScriptProviders;
using DevTools.Helpers;

namespace DevTools.DatabaseDeployment.ScriptExecution
{

    using Microsoft.SqlServer.Management.Common;

    using Microsoft.SqlServer.Management.Smo;
    using DatabaseDeployment.ScriptProviders;
    using System.Data.SqlClient;

    public sealed class SqlScriptExecutor : IScriptExecutor
    {
        static Utils.Logger Logger = new Utils.Logger(typeof(DatabaseUpgrader));
        public void Execute(string connectionString, IScript script)
        {
            try
            {
                var AppPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                var AppDir = System.IO.Path.GetDirectoryName(AppPath);

                TraceHelper.TraceInformation("Executing SQL Server script '{0}'", new object[] { script.Name });
                Logger.Debug("Executing SQL Server script '{0}'", new object[] { script.Name });
                SqlConnection connection = new SqlConnection(connectionString);
                Server server = new Server(new ServerConnection(connection));
                string mySql = script.Contents.Replace("{AppPath}", AppDir);


                Logger.Debug("Script:[{0}]", mySql);
                var temp = server.ConnectionContext.ExecuteNonQuery(mySql);
                Logger.Debug("SQL Result:{0}", temp);
            }
            catch (System.Exception ex)
            {
                Logger.ErrorEx(ex, "Executing Script {0}", script.Name);
                throw ex;
            }
        }
    }
}

