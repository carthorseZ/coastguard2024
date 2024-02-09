using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using log4net;
using log4net.Config;

#line hidden
namespace Utils
{
    public class Logger
    {
        private static ILog log;

        public static void Configure()
        {

            XmlConfigurator.Configure();

        }

        public void ReConfigure()
        {
            XmlConfigurator.Configure();
            log.Info(string.Format("Initilising Logger:", log.GetType().ToString()));
            log.Info(string.Format("Debug Enbled:", log.IsDebugEnabled));
            log.Info(string.Format("Info Enbled:", log.IsInfoEnabled));
            log.Info(string.Format("Warn Enbled:", log.IsWarnEnabled));
            log.Info(string.Format("Error Enbled:", log.IsErrorEnabled));


        }
        public Logger(Type t)
        {
            log = LogManager.GetLogger(t);

        }
        #region Formatted Logging

        public void Debug(string message, params object[] args)
        {
            if (log.IsDebugEnabled) log.DebugFormat(message, args);
        }

        public void Info(string message, params object[] args)
        {
            if (log.IsInfoEnabled) log.InfoFormat(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            if (log.IsWarnEnabled) log.WarnFormat(message, args);
        }

        public void Error(string message, params object[] args)
        {
            if (log.IsErrorEnabled) log.ErrorFormat(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            if (log.IsFatalEnabled) log.FatalFormat(message, args);
        }

        #endregion

        #region Exception Logging

        public void DebugEx(Exception e, string message, params object[] args)
        {
            if (log.IsDebugEnabled) log.Debug(string.Format(message, args), e);
        }

        public void InfoEx(Exception e, string message, params object[] args)
        {
            if (log.IsInfoEnabled) log.Info(string.Format(message, args), e);
        }

        public void WarnEx(Exception e, string message, params object[] args)
        {
            if (log.IsWarnEnabled) log.Warn(string.Format(message, args), e);
        }

        public void ErrorEx(Exception e, string message, params object[] args)
        {
            if (log.IsErrorEnabled) log.Error(string.Format(message, args), e);
        }
        public void ErrorEx(Exception e, string message)
        {
            if (log.IsErrorEnabled) log.Error(string.Format(message), e);
        }

        public void Error(Exception e)
        {
            if (log.IsErrorEnabled)
            {

                log.Error(string.Format("An Exception {1} has occured in {0}", e.Source, e.Message));
                if (e.Data != null && e.Data.Count > 0)
                {
                    
                    log.Error("Addtional Data:");
                    foreach (DictionaryEntry de in e.Data)
                    {
                        log.Error(string.Format("The key is '{0}' and the value is: '{1}'", de.Key, de.Value));
                    }

                }
                if (e.Source != null)
                {
                    log.Error("Source=" + e.Source);
                }
                if (e.InnerException != null)
                {
                    log.Error("Inner exception:");
                    log.Error(e);
                }
                if (e.HelpLink != null)
                {
                    log.Error("Help Link:");
                    log.Error(e.HelpLink);
                }
            }
            
        }

        public void FatalEx(Exception e, string message, params object[] args)
        {
            if (log.IsFatalEnabled) log.Fatal(string.Format(message, args), e);
        }

        #endregion
    }

    public class LoggingScope : Logger, IDisposable
    {
        public bool IsVerbose { get; set; }
        Stopwatch sw = new Stopwatch();
        public LoggingScope(bool isVerbose, string message, params object[] args)
            : base(new StackFrame(1).GetMethod().DeclaringType)
        {
            IsVerbose = isVerbose;
            Message = message;
            Arguments = args;
            if (IsVerbose)
                Debug("Start: " + Message, Arguments);
            else
                Info("Start: " + Message, Arguments);
            sw.Start();
        }

        public LoggingScope(string message, params object[] args)
            : base(new StackFrame(1).GetMethod().DeclaringType)
        {
            IsVerbose = true;
            Message = message;
            Arguments = args;
            if (IsVerbose)
                Debug("Start: " + Message, Arguments);
            else
                Info("Start: " + Message, Arguments);
            sw.Start();
        }

        public string Message { get; set; }
        public object[] Arguments { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            sw.Stop();
            if (IsVerbose)
                Debug("End: " + Message + "(" + sw.ElapsedMilliseconds + "ms)", Arguments);
            else
                Info("End: " + Message + "(" + sw.ElapsedMilliseconds + "ms)", Arguments);
        }

        #endregion
    }

    public class LogWriter : TextWriter
    {
        private readonly Logger logger;

        public LogWriter(Logger logger)
        {
            this.logger = logger;
        }
       
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(string msg)
        {
            logger.Debug(msg);
        }

        public override void WriteLine(string msg)
        {
            logger.Debug(msg);
        }
    }

    public static class PayloadDumper
    {
        private static Dictionary<Type, XmlSerializer> serializers = new Dictionary<Type, XmlSerializer>();
        //[Conditional("DEBUG")]
        public static void Dump<T>(this Logger logger, T payload) where T : class
        {
            try
            {
                if (payload == null)
                {
                    logger.Warn("No Object Found");
                    return;
                }
                XmlSerializer xs;
                Type payloadType = payload.GetType();
                logger.Info("Dumping Payload for Object [{0}]", payloadType);
                if (!serializers.ContainsKey(payloadType))
                {
                    serializers[payloadType] = xs = new XmlSerializer(payloadType);
                }
                else
                {
                    xs = serializers[payloadType];
                }
                var sb = new StringBuilder("");
                var sw = new StringWriter(sb);
                try
                {
                    logger.Debug("Payload for '{0}':", payloadType.Name);
                    xs.Serialize(sw, payload);
                    logger.Debug(sb.ToString());
                }
                catch (Exception e)
                {
                    logger.WarnEx(e, "Failed to serialize payload for type '{0}'", payloadType.Name);
                }
            }catch  {
                // Try Json Dumper
                //logger.Debug(Newtonsoft.Json.JsonConvert.SerializeObject(payload));

                    logger.Warn("Failed to Dump the Object");
                }
        }
    }

}
#line default