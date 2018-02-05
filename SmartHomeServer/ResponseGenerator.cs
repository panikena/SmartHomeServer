using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class ResponseGenerator
    {
        private UnixSocketEndpoint UnixSocket { get; set; }
        private WebSocketEndpoint WebSocket { get; set; }

        public ResponseGenerator(UnixSocketEndpoint unixSocket, WebSocketEndpoint webSocket)
        {
            UnixSocket = unixSocket;
            WebSocket = webSocket;
        }

        public async Task SendResponse(IProcessingResult result)
        {
            if (result.SmartBrickMessages != null && UnixSocket.IsRunning)
            {
                var unixTasks = new List<Task>();

                foreach (var message in result.SmartBrickMessages.Where(x => x != null))
                {
                    unixTasks.Add(UnixSocket.SendCommand(message));
                }
                if (unixTasks.Any())
                {
                    await Task.WhenAll(unixTasks);
                }
            }
                
            if (result.WebSocketMessages != null && WebSocket.IsRunning)
            {
                var webSocketTasks = new List<Task>();

                foreach (var message in result.WebSocketMessages.Where(x => x != null && x.SocketSessionID != null))
                {
                    webSocketTasks.Add(WebSocket.SendMessage(message.SocketSessionID, message.Message));
                }
                if (webSocketTasks.Any())
                {
                    await Task.WhenAll(webSocketTasks);
                }
            }
        }
    }
}
