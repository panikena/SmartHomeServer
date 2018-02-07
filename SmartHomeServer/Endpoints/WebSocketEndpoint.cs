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
        public Action<IMessage> ProcessCommand { get; set; }

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
            SocketServer.SessionClosed += new SessionHandler<WebSocketSession, CloseReason>();

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
            foreach (var session in SocketServer.GetAllSessions())
            {
                session.CloseWithHandshake("Server is stopping");
            }
            SocketServer.Stop();

            log.Info("WebSocket server was closed");
        }

        private void OnMessageReceived(WebSocketSession session, string message)
        {
            var msg = new WebSocketMessage()
            {
                SocketSessionID = session.SessionID,
                Message = message
            };
            //Start thread for processing
            Task.Run(() => ProcessCommand(msg));
        }

        private void OnSessionClosed(WebSocketSession session, CloseReason reason)
        {
            log.Info("WebSocket server was opened");

            if (SocketDict.ContainsKey(session.SessionID))
            {
                SocketDict.Remove(session.SessionID);
            }
        }


        private void RegisterSocket(WebSocketSession session)
        {
            log.InfoFormat("WebSocket session was opened, session ID: {0}, {1}", session.SessionID, session.Connection);
           
            if (!SocketDict.ContainsKey(session.SessionID))
            {
                SocketDict.Add(session.SessionID, "Web");
            }

        }

        public void SendMessage(string sessionId, string message)
        {
            var session = SocketServer.GetSessionByID(sessionId);
            if (session != null && session.Connected)
            {
                try
                {
                    session.Send(message);
                }
                catch (TimeoutException ex)
                {
                    log.Error(string.Format("Timeout sending to socket {0}", session.SessionID), ex);
                }
            }
            
        }
    }
}
