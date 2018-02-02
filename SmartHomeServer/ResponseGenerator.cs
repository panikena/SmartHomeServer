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
            if (result.SmartBrickMessages != null)
            {
                var unixTasks = new List<Task>();

                foreach (var message in result.SmartBrickMessages)
                {
                    unixTasks.Add(UnixSocket.SendCommand(message));
                }
                await Task.WhenAll(unixTasks);
            }
                
            if (result.WebSocketMessages != null)
            {
                var webSocketTasks = new List<Task>();

                foreach (var message in result.WebSocketMessages.Where(x => x.SocketSessionID != null))
                {
                    webSocketTasks.Add(WebSocket.SendMessage(message.SocketSessionID, message.Message));
                }

                await Task.WhenAll(webSocketTasks);
            }
        }
    }
}
