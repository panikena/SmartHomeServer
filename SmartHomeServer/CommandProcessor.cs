using SmartHomeServer.InputMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class CommandProcessor
    {
        private ProcessingStrategyResolver StrategyFactory { get; set; }
        private ResponseGenerator ResponseGenerator { get; set; }

        public CommandProcessor(UnixSocketEndpoint unixEndpoint, WebSocketEndpoint webEndpoint)
        {
            StrategyFactory = new ProcessingStrategyResolver();
            ResponseGenerator = new ResponseGenerator(unixEndpoint, webEndpoint);
            webEndpoint.ProcessCommand = ProcessCommand;
        }

        public async Task ProcessCommand(IInputMessage command)
        {
            var module = StrategyFactory.GetProcessingModule(command);

            IProcessingResult result = await Task.Run(() => module.ProcessCommand(command));

            await ResponseGenerator.SendResponse(result);
        }
    }
}
