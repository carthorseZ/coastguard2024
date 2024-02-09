using DevTools.DatabaseDeployment.ScriptProviders;

namespace DevTools.DatabaseDeployment.ScriptExecution
{
    using DatabaseDeployment.ScriptProviders;
    using System;

    public interface IScriptExecutor
    {
        void Execute(string connectionString, IScript script);
    }
}

