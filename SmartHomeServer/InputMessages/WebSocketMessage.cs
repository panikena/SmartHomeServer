namespace SmartHomeServer.InputMessages
{
    public class WebSocketMessage : IInputMessage
    {
        public string SocketSessionID { get; set; }

        public MessageSource Source { get { return MessageSource.WebSocket; } }
        
        public string Message { get; set; }
    }
}
