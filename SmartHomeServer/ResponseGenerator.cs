using log4net;
using SmartHomeServer.Messages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class ResponseGenerator
    {
        private static readonly ILog log = LogManager.GetLogger("LOGGER");
        private UnixSocketEndpoint UnixSocket { get; set; }
        private WebSocketEndpoint WebSocket { get; set; }

        public ResponseGenerator(UnixSocketEndpoint unixSocket, WebSocketEndpoint webSocket)
        {
            UnixSocket = unixSocket;
            WebSocket = webSocket;
        }

        public async Task SendResponse(IProcessingResult result)
        {
            var sendingTasks = new Task[2];

            if (result.SmartBrickMessages != null && UnixSocket.IsRunning)
            {
                sendingTasks[0] = SendUnixSocketMessages(result.SmartBrickMessages);
            }
            if (result.WebSocketMessages != null && WebSocket.IsRunning)
            {
                sendingTasks[1] = SendWebSocketMessages(result.WebSocketMessages);
            }

            await Task.WhenAll(sendingTasks);
        }


        private async Task SendUnixSocketMessages(IEnumerable<SmartBrickMessage> messages)
        {
            foreach (var message in messages.Where(x => x != null))
            {
                //Unix socket might close during processing
                if (!UnixSocket.IsRunning)
                {
                    log.Warn("Unix socket was closed during sending");
                    return;
                }
                await UnixSocket.SendCommand(message);
            }
        }

        private async Task SendWebSocketMessages(IEnumerable<WebSocketMessage> messages)
        {
            foreach (var message in messages.Where(x => x != null && x.SocketSessionID != null))
            {
                await Task.Run(() => WebSocket.SendMessage(message.SocketSessionID, message.Message));
            }
        }


    }
}
