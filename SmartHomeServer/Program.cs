using System.ServiceProcess;

namespace SmartHomeServer
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
	        var smartHomeservice =  new SmartHomeDaemon();
			smartHomeservice.OnDebug();
			System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
			ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] {
                new SmartHomeDaemon()
            };
            ServiceBase.Run(ServicesToRun);			
#endif
		}
	}
}
