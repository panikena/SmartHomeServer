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


#if WINDEBUG
        public ResponseGenerator(WebSocketEndpoint webSocket)
		{
			WebSocket = webSocket;
		}
#else
        public ResponseGenerator(UnixSocketEndpoint unixSocket, WebSocketEndpoint webSocket)
        {
            UnixSocket = unixSocket;
            WebSocket = webSocket;
        }
#endif

        public async Task SendResponse(IProcessingResult result)
        {
#if WINDEBUG
			if (result.WebSocketMessages != null && WebSocket.IsRunning)
			{
				var sendingTask = SendWebSocketMessages(result.WebSocketMessages);
				await sendingTask;
			}
#else
            var sendingTasks = new Task[2] { Task.FromResult(true), Task.FromResult(true) };

            if (!UnixSocket.IsRunning)
            {
                log.Error("Unix socket is not running!");
            }



			if (result.SmartBrickMessages != null && UnixSocket.IsRunning)
			{
				sendingTasks[0] = SendUnixSocketMessages(result.SmartBrickMessages);
			}
			if (result.WebSocketMessages != null && WebSocket.IsRunning)
            {
                sendingTasks[1] = SendWebSocketMessages(result.WebSocketMessages);
            }

            await Task.WhenAll(sendingTasks);
#endif
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
            var tasks = new List<Task>();
            var msg = messages.Where(x => x != null && x.SocketSessionID != null);
            //log.Info("WS list: " + string.Join("", msg.Select(x => x.SocketSessionID)));

            foreach (var message in msg)
            {
                tasks.Add(Task.Run(() => WebSocket.SendMessage(message.SocketSessionID, message.Message)));
            }

            await Task.WhenAll(tasks);
        }
    }
}
