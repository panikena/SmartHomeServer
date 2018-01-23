using log4net;
using log4net.Repository.Hierarchy;
using System.ServiceProcess;

namespace SmartHomeServer
{
    public class SmartHomeDaemon : ServiceBase
    {   
        private UnixSocketHandler UnixSocketHandler { get; set; }
        private WebSocketHandler WebSocketHandler { get; set; }

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
            EventLog.WriteEntry("SmartHomeDaemon was started");

            UnixSocketHandler = new UnixSocketHandler();
            UnixSocketHandler.Start();

            WebSocketHandler = new WebSocketHandler();
            WebSocketHandler.Start();

            log.Info("SmartHomeDaemon has started");
        }

        protected override void OnStop()
        { 
            log.Info("Stopping SmartHomeDaemon...");

            UnixSocketHandler.Stop();
            WebSocketHandler.Stop();

            log.Info("SmartHomeDaemon was stopped");
        }
    }
}