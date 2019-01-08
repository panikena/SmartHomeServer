using log4net;
using Mono.Unix;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace SmartHomeServer
{
    public class SmartHomeDaemon : ServiceBase
    {
        private UnixSocketEndpoint UnixSocketEndpoint { get; set; }
        private WebSocketEndpoint WebSocketEndpoint { get; set; }
        private CommandProcessor CommandProcessor { get; set; }

        private static readonly ILog log = LogManager.GetLogger("LOGGER");


        public SmartHomeDaemon()
        {
            log4net.Config.XmlConfigurator.Configure();
            ServiceName = "SmartHomeDaemon";
            log.Info("SmartHomeDaemon object was created");
        }

        protected override void OnStart(string[] args)
        {
            log.Info("Starting SmartHomeDaemon...");
            AppDomain.CurrentDomain.UnhandledException +=
                new UnhandledExceptionEventHandler(OnUnhandledException);


            WebSocketEndpoint = new WebSocketEndpoint();

#if WINDEBUG
			CommandProcessor = new CommandProcessor(WebSocketEndpoint);
			
#else
            UnixSocketEndpoint = new UnixSocketEndpoint();
            CommandProcessor = new CommandProcessor(UnixSocketEndpoint, WebSocketEndpoint);

            UnixSocketEndpoint.Open();
#endif

            WebSocketEndpoint.Open();

            log.Info("SmartHomeDaemon was started");           
        }


        protected override void OnStop()
        {
            log.Info("Stopping SmartHomeDaemon...");

            UnixSocketEndpoint.Close();
            WebSocketEndpoint.Close();

            log.Info("SmartHomeDaemon was stopped");
            try
            {
                if (File.Exists("/tmp/SmartHomeServer.exe.lock"))
                {
                    File.Delete("/tmp/SmartHomeServer.exe.lock");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            Environment.Exit(0);
        }


        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            log.Error("An unhandled exception was thrown in " + sender.ToString(), (Exception)e.ExceptionObject);
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}