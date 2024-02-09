namespace DevTools.DatabaseDeployment
{
    using System;

    public sealed class TemporarySqlDatabase : IDisposable
    {
        private string _connectionString;

        public TemporarySqlDatabase()
        {
            string str = "TestDatabase_" + Guid.NewGuid().ToString();
            this._connectionString = string.Format("Server=(local);Database={0};Trusted_connection=true", str);
            SqlDatabaseHelper.Create(this._connectionString);
        }

        public void Dispose()
        {
            SqlDatabaseHelper.DestroyOrContinue(this._connectionString);
            GC.SuppressFinalize(this);
        }

        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
        }
    }
}

