using SmartHomeServer.Messages;
using System.Collections.Generic;

namespace SmartHomeServer
{
    public interface IProcessingResult
    {
        IEnumerable<SmartBrickMessage> SmartBrickMessages { get; set; }
        IEnumerable<WebSocketMessage> WebSocketMessages { get; set; }
    }

    public class ProcessingResult : IProcessingResult
    {
        public IEnumerable<SmartBrickMessage> SmartBrickMessages { get; set; }
        public IEnumerable<WebSocketMessage> WebSocketMessages { get; set; }

        public ProcessingResult(IEnumerable<SmartBrickMessage> brickMessages, IEnumerable<WebSocketMessage> webSocketMessages)
        {
            SmartBrickMessages = brickMessages;
            WebSocketMessages = webSocketMessages;
        }
    }
}
