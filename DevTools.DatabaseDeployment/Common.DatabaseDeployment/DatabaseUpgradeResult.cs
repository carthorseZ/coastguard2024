using DevTools.DatabaseDeployment.ScriptProviders;

namespace DevTools.DatabaseDeployment
{
    using System;
    using System.Collections.Generic;

    public sealed class DatabaseUpgradeResult
    {
        private Exception _error;
        private int _originalVersion;
        private List<IScript> _scripts = new List<IScript>();
        private bool _successful;
        private int _upgradedVersion;

        public DatabaseUpgradeResult(IEnumerable<IScript> scripts, int original, int upgraded, bool successful, Exception error)
        {
            this._scripts.AddRange(scripts);
            this._originalVersion = original;
            this._upgradedVersion = upgraded;
            this._successful = successful;
            this._error = error;
        }

        public Exception Error
        {
            get
            {
                return this._error;
            }
        }

        public int OriginalVersion
        {
            get
            {
                return this._originalVersion;
            }
        }

        public IEnumerable<IScript> Scripts
        {
            get
            {
                return this._scripts;
            }
        }

        public bool Successful
        {
            get
            {
                return this._successful;
            }
        }

        public int UpgradedVersion
        {
            get
            {
                return this._upgradedVersion;
            }
        }
    }
}

