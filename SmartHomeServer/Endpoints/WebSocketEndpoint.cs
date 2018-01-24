using log4net;
using SmartHomeServer.InputMessages;
using SuperSocket.SocketBase;
using SuperSocket.WebSocket;
using System;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class WebSocketEndpoint
    {
        private WebSocketServer SocketServer { get; set; }
        public Func<IInputMessage, Task> ProcessCommand { get; set; }

        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public WebSocketEndpoint()
        {
            SocketServer = new WebSocketServer();
        }

        public void Open()
        {
            //Setup the appServer
            if (!SocketServer.Setup(2012)) //Setup with listening port
            {
                log.Error("Could not setup WebSocket server");
                return;
            }

            SocketServer.NewMessageReceived += new SessionHandler<WebSocketSession, string>(OnMessageReceived);
            

            //Try to start the appServer
            if (!SocketServer.Start())
            {
                log.Error("Could not start WebSocket server");
                return;
            }

            log.Info("WebSocket server was opened");
        }

        public void Close()
        {
            //Console.WriteLine("The server was stopped!");
            SocketServer.Stop();
            log.Info("WebSocket server was closed");
        }

        private async void OnMessageReceived(WebSocketSession session, string message)
        {
            //Send the received message back
            //session.Send("Server: " + message);

            var msg = new WebSocketMessage()
            {
                SocketSessionID = session.SessionID,
                Message = message
            };
           await ProcessCommand(msg);
        }

        public async Task SendMessage(string sessionId, string message)
        {
            var session = SocketServer.GetSessionByID(sessionId);
            await Task.Run(() => session.Send(message));
        }
        
    
    }
}
