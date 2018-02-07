using log4net;
using SmartHomeServer.Exceptions;
using SmartHomeServer.Messages;
using System;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class CommandProcessor
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");
        private ProcessingStrategyResolver StrategyFactory { get; set; }
        private ResponseGenerator ResponseGenerator { get; set; }

        public CommandProcessor(UnixSocketEndpoint unixEndpoint, WebSocketEndpoint webEndpoint)
        {
            StrategyFactory = new ProcessingStrategyResolver();
            ResponseGenerator = new ResponseGenerator(unixEndpoint, webEndpoint);
            webEndpoint.ProcessCommand = ProcessCommand;
            unixEndpoint.ProcessCommand = ProcessCommand;
        }

        public async void ProcessCommand(IMessage command)
        {
            try
            {
                log.Info("Processing command from " + command.Source.ToString());
                var module = StrategyFactory.GetProcessingModule(command);
                IProcessingResult result = module.ProcessCommand(command);
                await ResponseGenerator.SendResponse(result);
            }
            catch (NoModuleFoundException ex)
            {
                log.Error("No module found", ex);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Unexpected exception", ex);
            }
        }
    }
}
