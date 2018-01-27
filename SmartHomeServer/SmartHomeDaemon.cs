using log4net;
using System.ServiceProcess;

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
            log.Info("SmartHomeDaemon was started");


            UnixSocketEndpoint = new UnixSocketEndpoint();
            UnixSocketEndpoint.Open();

            WebSocketEndpoint = new WebSocketEndpoint();
            WebSocketEndpoint.Open();

            CommandProcessor = new CommandProcessor(UnixSocketEndpoint, WebSocketEndpoint);

            log.Info("SmartHomeDaemon has started");
        }

        protected override void OnStop()
        { 
            log.Info("Stopping SmartHomeDaemon...");

            UnixSocketEndpoint.Close();
            WebSocketEndpoint.Close();

            log.Info("SmartHomeDaemon was stopped");
        }
    }
}