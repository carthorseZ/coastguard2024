namespace DevTools.DatabaseDeployment
{
    using Microsoft.SqlServer.Management.Smo;
    using System;
    using System.Data.SqlClient;

    public static class SqlDatabaseHelper
    {
        private static Utils.Logger Logger = new Utils.Logger(typeof(SqlDatabaseHelper));
        public static void BackupDatabase(string connectionString, bool isPrivate)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            Server server = new Server(builder.DataSource);
            string str = string.Format(@"F:\Backups\{0}", isPrivate ? "Private" : "Public");
            string str2 = string.Format("BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'AllBackup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10", builder.InitialCatalog, str);
            server.Databases[builder.InitialCatalog].ExecuteNonQuery(str2);
        }

        public static void Create(string connectionString)
        {
            if (Exists(connectionString))
            {
                throw new Exception(string.Format("The database specified by the connection string '{0}' already exists.", connectionString));
            }
            CreateInternal(connectionString);
        }

        private static void CreateInternal(string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                Server server = new Server(builder.DataSource);
                new Database(server, builder.InitialCatalog).Create();
            }
            catch (Exception ex)
            {
                Logger.ErrorEx(ex, "Creating Database");
                throw (ex);
            }
        }

        public static void CreateOrContinue(string connectionString)
        {
            if (!Exists(connectionString))
            {
                CreateInternal(connectionString);
            }
        }

        public static void Destroy(string connectionString)
        {
            if (!Exists(connectionString))
            {
                throw new Exception(string.Format("The database specified by the connection string '{0}' does not exists.", connectionString));
            }
            DestroyInternal(connectionString);
        }

        private static void DestroyInternal(string connectionString)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            Server server = new Server(builder.DataSource);
            server.KillAllProcesses(builder.InitialCatalog);
            server.KillDatabase(builder.InitialCatalog);
        }

        public static void DestroyOrContinue(string connectionString)
        {
            if (Exists(connectionString))
            {
                DestroyInternal(connectionString);
            }
        }

        public static bool Exists(string connectionString)
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                Server server = new Server(builder.DataSource);
                return server.Databases.Contains(builder.InitialCatalog);
            }
            catch (Exception ex)
            {
                Logger.WarnEx(ex, "");
                return false;
            }
        }
    }
}

