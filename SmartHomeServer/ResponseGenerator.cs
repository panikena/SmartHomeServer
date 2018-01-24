using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            if (result.SmartBrickCommand != null)
            {
                await UnixSocket.SendCommand(result.SmartBrickCommand);
            }
            if (result.WebMessage != null)
            {
                await WebSocket.SendMessage(result.SocketSessionID, result.WebMessage);
            }
        }
    }
}
