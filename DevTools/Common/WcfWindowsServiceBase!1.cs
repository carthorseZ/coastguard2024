namespace DevTools
{
    using DevTools.Helpers;
    using System;
    using System.Diagnostics;
    using System.ServiceProcess;

    public class WcfWindowsServiceBase<TService> : ServiceBase where TService: new()
    {
        private ServiceHost<TService> _serviceHost;

        public WcfWindowsServiceBase(string serviceName, params string[] baseAddresses)
        {
            ArgumentValidator.ValidateArgumentIsNotNull("serviceName", serviceName);
            ServiceName = serviceName;
            this._serviceHost = new ServiceHost<TService>(baseAddresses);
        }

        public void InstallService(string[] installArguments)
        {
        }

        protected override void OnStart(string[] args)
        {
            this.StartWcfService();
            base.OnStart(args);
        }

        protected override void OnStop()
        {
            this.StopWcfService();
            base.OnStop();
        }

        public void StartConsoleService()
        {
            TraceHelper.General.Level = TraceLevel.Verbose;
            TextWriterTraceListener listener = new TextWriterTraceListener(Console.Out);
            Trace.Listeners.Add(listener);
            this.StartWcfService();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("WCF service {0} started. Press any key to stop...", ServiceName);
            Console.ReadKey();
            Console.ForegroundColor = ConsoleColor.Gray;
            this.StopWcfService();
        }

        protected virtual void StartWcfService()
        {
            try
            {
                TraceHelper.TraceVerbose("Starting WCF service {0}", new object[] { ServiceName });
                this._serviceHost.Open();
                TraceHelper.TraceInformation("WCF service {0} started successfully", new object[] { ServiceName });
            }
            catch (Exception exception)
            {
                TraceHelper.TraceError(exception, "Failed to start WCF service {0}", new object[] { ServiceName });
                base.Stop();
            }
        }

        public void StartWindowsService()
        {
            ServiceBase.Run(new ServiceBase[] { this });
        }

        protected virtual void StopWcfService()
        {
            try
            {
                TraceHelper.TraceVerbose("Stopping WCF service {0}", new object[] { ServiceName });
                this._serviceHost.Close();
                TraceHelper.TraceInformation("WCF service {0} stopped successfully", new object[] { ServiceName });
            }
            catch (Exception exception)
            {
                TraceHelper.TraceError(exception, "Failed to stop WCF service {0}", new object[] { ServiceName });
            }
        }

        public void UninstallService(string[] uninstallArguments)
        {
        }
    }
}

