namespace DevTools.DatabaseDeployment.ScriptProviders
{
    using System;

    public sealed class EmbeddedSqlScript : IScript
    {
        private string _contents;
        private string _name;
        private string _sourceIdentifier;
        private int _versionNumber;

        public EmbeddedSqlScript(string name, string contents, int versionNumber, string sourceIdentifier)
        {
            this._name = name;
            this._contents = contents;
            this._versionNumber = versionNumber;
            this._sourceIdentifier = sourceIdentifier;
        }

        public string Contents
        {
            get
            {
                return this._contents;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
        }

        public string SourceIdentifier
        {
            get
            {
                return this._sourceIdentifier;
            }
        }

        public int VersionNumber
        {
            get
            {
                return this._versionNumber;
            }
        }
    }
}

