namespace DevTools.DatabaseDeployment.ScriptProviders
{
    using System;

    public interface IScript
    {
        string Contents { get; }

        string Name { get; }

        string SourceIdentifier { get; }

        int VersionNumber { get; }
    }
}

