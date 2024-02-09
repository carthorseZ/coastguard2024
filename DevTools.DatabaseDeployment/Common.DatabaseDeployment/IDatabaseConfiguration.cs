using DevTools.DatabaseDeployment.ScriptExecution;
using DevTools.DatabaseDeployment.ScriptProviders;
using DevTools.DatabaseDeployment.VersionTrackers;

namespace DevTools.DatabaseDeployment
{
    using DatabaseDeployment.ScriptExecution;
    using DatabaseDeployment.ScriptProviders;
    using DatabaseDeployment.VersionTrackers;
    using System;

    public interface IDatabaseConfiguration
    {
        string ConnectionString { get; }

        IScriptExecutor ScriptExecutor { get; }

        IScriptProvider ScriptProvider { get; }

        IVersionTracker VersionTracker { get; }
    }
}

