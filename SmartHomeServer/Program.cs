using System.ServiceProcess;

namespace SmartHomeServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] {
                new SmartHomeDaemon()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
