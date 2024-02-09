using DevTools.DatabaseDeployment.ScriptProviders;

namespace DevTools.DatabaseDeployment.VersionTrackers
{
    using DatabaseDeployment.ScriptProviders;
    using System;

    public interface IVersionTracker
    {
        int RecallVersionNumber(string connectionString);
        void StoreUpgrade(string connectionString, IScript script);
    }
}

