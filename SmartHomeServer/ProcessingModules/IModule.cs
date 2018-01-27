using SmartHomeServer.Messages;

namespace SmartHomeServer.ProcessingModules
{
    public interface IModule
    {
        IProcessingResult ProcessCommand(IMessage message);
    }
}
