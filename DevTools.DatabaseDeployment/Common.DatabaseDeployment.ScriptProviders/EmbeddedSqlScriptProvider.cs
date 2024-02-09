namespace DevTools.DatabaseDeployment.ScriptProviders
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public sealed class EmbeddedSqlScriptProvider : IScriptProvider
    {
        private static Utils.Logger Logger = new Utils.Logger(typeof(SqlDatabaseHelper));
        private Assembly _assembly;
        private GetEmbeddedScriptNameCallback _mapFileNameCallback;

        public EmbeddedSqlScriptProvider(Assembly assembly, GetEmbeddedScriptNameCallback mapFileNameCallback)
        {
            this._assembly = assembly;
            this._mapFileNameCallback = mapFileNameCallback;
        }

        public int GetHighestScriptVersion()
        {
            
            List<string> list = new List<string>();
            list.AddRange(this._assembly.GetManifestResourceNames());
            int num = 0;
            while (true)
            {
                int scriptVersionNumber = num + 1;
                string item = this._mapFileNameCallback(scriptVersionNumber);
                Logger.Debug("Processing Item[{0}]", item);
                if (!list.Contains(item))
                {
                    return num;
                }
                num++;
            }
        }

        public IScript GetScript(int versionNumber)
        {
            string name = this._mapFileNameCallback(versionNumber);
            string contents = null;
            using (StreamReader reader = new StreamReader(this._assembly.GetManifestResourceStream(name)))
            {
                contents = reader.ReadToEnd();
            }
            return new EmbeddedSqlScript(name, contents, versionNumber, this._assembly.FullName);
        }
    }
}

