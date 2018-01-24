using SmartHomeServer.InputMessages;

namespace SmartHomeServer.ProcessingModules
{
    public interface IModule
    {
        IProcessingResult ProcessCommand(IInputMessage message);
    }
}
