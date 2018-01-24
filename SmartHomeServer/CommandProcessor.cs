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
            ResponseGenerator = new ResponseGenerator();
        }

        public async void ProcessCommand(IInputCommand command)
        {
            var module = StrategyFactory.GetProcessingModule(command);

            IProcessingResult result = await module.ProcessCommand();
        }
    }
}
