using DevTools.DatabaseDeployment.ScriptProviders;
using DevTools.Helpers;

namespace DevTools.DatabaseDeployment.VersionTrackers
{
    using DatabaseDeployment.ScriptProviders;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;

    public sealed class SchemaVersionsTableSqlVersionTracker : IVersionTracker
    {
        public int RecallVersionNumber(string connectionString)
        {
            int num2;
            TraceHelper.TraceInformation("Detecting the current database version.");
            using (TraceHelper.Indent())
            {
                SqlConnection connection;
                SqlCommand command;
                int num;
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = "select count(*) from sys.objects where type='U' and name='SchemaVersions'";
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        num = (int) command.ExecuteScalar();
                        if (num == 0)
                        {
                            TraceHelper.TraceInformation("The SchemaVersions table could not be found. The database is assumed to be at version 0.");
                            return 0;
                        }
                    }
                }
                using (connection = new SqlConnection(connectionString))
                {
                    using (command = connection.CreateCommand())
                    {
                        command.CommandText = "GetCurrentVersionNumber";
                        command.CommandType = CommandType.StoredProcedure;
                        connection.Open();
                        num = (int) command.ExecuteScalar();
                        Trace.TraceInformation("Version {0} detected.", new object[] { num });
                        num2 = num;
                    }
                }
            }
            return num2;
        }

        public void StoreUpgrade(string connectionString, IScript script)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "RecordVersionUpgrade";
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@versionNumber", script.VersionNumber);
                    command.Parameters.AddWithValue("@sourceIdentifier", script.SourceIdentifier);
                    command.Parameters.AddWithValue("@scriptName", script.Name);
                    connection.Open();
                    command.ExecuteNonQuery();
                    Trace.TraceInformation("Upgrade to version {0} stored successfully.", new object[] { script.VersionNumber });
                }
            }
        }
    }
}

