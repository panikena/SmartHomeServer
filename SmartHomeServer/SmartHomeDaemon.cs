using log4net;
using Mono.Unix;
using System;
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

            // Catch SIGINT and SIGUSR1
            UnixSignal[] signals = new UnixSignal[] {
                //SIGINT interrupts on Ctrl+C, this is for console debugging
                new UnixSignal (Mono.Unix.Native.Signum.SIGINT),
                new UnixSignal (Mono.Unix.Native.Signum.SIGTERM),
            };

            //Run this thread to handle POSIX signals and terminate gracefully
            Thread signalThread = new Thread(delegate ()
            {
                int index;
                while (true)
                {
                    // Wait for a signal to be delivered
                    //-1 means "wait indefinitely"
                    index = UnixSignal.WaitAny(signals, -1);

                    Mono.Unix.Native.Signum signal = signals[index].Signum;

                    if (signal == Mono.Unix.Native.Signum.SIGTERM || signal == Mono.Unix.Native.Signum.SIGINT)
                    {
                        OnStop();
                    }
                    
                }
            });

            signalThread.Start();
        }


        protected override void OnStop()
        {
            log.Info("Stopping SmartHomeDaemon...");

            UnixSocketEndpoint.Close();
            WebSocketEndpoint.Close();

            log.Info("SmartHomeDaemon was stopped");

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