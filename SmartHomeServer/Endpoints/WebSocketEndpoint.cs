using log4net;
using SmartHomeServer.Messages;
using SuperSocket.SocketBase;
using SuperSocket.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHomeServer
{
    public class WebSocketEndpoint
    {

        public static Dictionary<string, string> SocketDict = new Dictionary<string, string>();
        
        private WebSocketServer SocketServer { get; set; }
        public Func<IMessage, Task> ProcessCommand { get; set; }

        private static readonly ILog log = LogManager.GetLogger("LOGGER");

        public bool IsRunning
        {
            get
            {
                return SocketServer.State == ServerState.Running;
            }
        }

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
            SocketServer.NewSessionConnected += new SessionHandler<WebSocketSession>(RegisterSocket);

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
            var msg = new WebSocketMessage()
            {
                SocketSessionID = session.SessionID,
                Message = message
            };
            await ProcessCommand(msg);
        }

        private void RegisterSocket(WebSocketSession session)
        {
            log.Info("WebSocket server was opened");
           
            if (!SocketDict.ContainsKey("Web"))
            {
                SocketDict.Add("Web", session.SessionID);
            }

        }

        public async Task SendMessage(string sessionId, string message)
        {
            var session = SocketServer.GetSessionByID(sessionId);
            if (session != null)
            {
                try
                {
                    await Task.Run(() => session.Send(message));
                }
                catch (TimeoutException ex)
                {
                    log.Error(string.Format("Timeout sending to socket {0}", session.SessionID));
                }
            }
            
        }
    }
}
