namespace DevTools.DatabaseDeployment.ScriptProviders
{
    using System;

    public interface IScriptProvider
    {
        int GetHighestScriptVersion();
        IScript GetScript(int versionNumber);
    }
}

